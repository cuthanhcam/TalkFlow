using System.Security.Claims;

namespace TalkFlow.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue("name");
        }
    }
}


