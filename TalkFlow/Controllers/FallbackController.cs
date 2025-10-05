using Microsoft.AspNetCore.Mvc;

namespace TalkFlow.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FallbackController : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}


