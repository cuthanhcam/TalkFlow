using TalkFlow.Entities;

namespace TalkFlow.Interfaces
{
    public interface IStrangerRepository
    {
        Task<List<List<AppUser>>> StrangerFindMatch();
    }
}


