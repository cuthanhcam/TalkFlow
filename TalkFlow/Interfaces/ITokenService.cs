using TalkFlow.Entities;

namespace TalkFlow.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser appUser);
    }
}


