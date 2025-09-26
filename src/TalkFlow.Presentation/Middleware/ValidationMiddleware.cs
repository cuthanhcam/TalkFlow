using System.Text.Json;

namespace TalkFlow.Presentation.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(body))
                {
                    try
                    {
                        var jsonDocument = JsonDocument.Parse(body);
                        var validationErrors = ValidateJsonDocument(jsonDocument.RootElement);

                        if (validationErrors.Any())
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                errors = validationErrors
                            }));
                            return;
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning("Invalid JSON in request: {Error}", ex.Message);
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = "Invalid JSON format"
                        }));
                        return;
                    }
                }
            }

            await _next(context);
        }

        private static List<string> ValidateJsonDocument(JsonElement element)
        {
            var errors = new List<string>();

            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in element.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.String &&
                        string.IsNullOrWhiteSpace(property.Value.GetString()))
                    {
                        errors.Add($"Property '{property.Name}' cannot be empty");
                    }
                }
            }

            return errors;
        }
    }
}
