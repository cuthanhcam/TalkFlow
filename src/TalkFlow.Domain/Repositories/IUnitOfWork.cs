using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoomRepository Rooms { get; }
        IMessageRepository Messages { get; }
        IStrangerFilterRepository StrangerFilters { get; }
        IMatchRepository Matches { get; }
        IStrangerMatchingRepository StrangerMatchings { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
