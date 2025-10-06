using TalkFlow.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace TalkFlow.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}


