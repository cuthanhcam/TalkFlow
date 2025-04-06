using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Thêm để hỗ trợ JWT Authentication
using Microsoft.IdentityModel.Tokens; // Thêm để cấu hình TokenValidationParameters
using System.Text; // Thêm để xử lý Encoding cho JWT key
using Microsoft.OpenApi.Models;
using TalkFlow.API.Hubs; // Thêm để cấu hình Swagger với JWT

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Logging (ghi log để debug dễ hơn)
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Ghi log ra console
    logging.AddDebug();   // Ghi log cho debug trong VSCode
});

// 2. Cấu hình DbContext với SQL Server
// Kết nối tới database sử dụng chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<TalkFlow.Infrastructure.Data.TalkFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Cấu hình JWT Authentication
// Sử dụng JwtBearer để xác thực token, lấy secret key từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentNullException(nameof(secretKey), "JWT Secret key is missing in appsettings.json");
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,           // Không kiểm tra issuer (có thể bật trong production)
            ValidateAudience = false,         // Không kiểm tra audience (có thể bật trong production)
            ValidateLifetime = true,          // Kiểm tra thời hạn token
            ValidateIssuerSigningKey = true,  // Kiểm tra khóa ký
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Secret"])) // Khóa bí mật từ config
        };
    });

// 4. Cấu hình SignalR cho chat thời gian thực
// Đăng ký SignalR để sử dụng ChatHub
builder.Services.AddSignalR();

// 5. Cấu hình CORS
// Cho phép tất cả origin trong môi trường dev (có thể giới hạn trong production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// 6. Đăng ký Controllers
// Hỗ trợ các API endpoints trong thư mục Controllers
builder.Services.AddControllers();

// 7. Cấu hình Swagger
// Thêm Swagger để kiểm thử API, tích hợp với JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TalkFlow API", Version = "v1" }); // Thông tin API
    // Thêm định nghĩa bảo mật cho JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field (e.g., 'Bearer {token}')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// 8. Cấu hình Middleware Pipeline
// Sắp xếp thứ tự: CORS -> Authentication -> Authorization -> Controllers
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      // Kích hoạt Swagger JSON
    app.UseSwaggerUI();    // Kích hoạt Swagger UI
}

app.UseCors("AllowAll");       // Áp dụng chính sách CORS
app.UseAuthentication();       // Middleware xác thực JWT
app.UseAuthorization();        // Middleware phân quyền
app.MapControllers();          // Định tuyến tới các controller
app.MapHub<ChatHub>("/chatHub"); // Định tuyến tới SignalR Hub

// 9. Seed dữ liệu (tùy chọn)
// Tự động áp dụng migrations và thêm dữ liệu mẫu khi ứng dụng khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Starting database migration...");
        var context = services.GetRequiredService<TalkFlow.Infrastructure.Data.TalkFlowDbContext>();
        await context.Database.MigrateAsync(); // Áp dụng migrations tự động
        logger.LogInformation("Migrations applied successfully.");
        // Bạn có thể thêm SeedData.InitializeAsync(services) nếu muốn seed dữ liệu
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

app.Run();