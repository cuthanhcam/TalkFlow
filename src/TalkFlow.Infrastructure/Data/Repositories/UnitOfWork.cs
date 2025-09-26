using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Repositories;

namespace TalkFlow.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Rooms = new RoomRepository(_context);
            Messages = new MessageRepository(_context);
            StrangerFilters = new StrangerFilterRepository(_context);
            Matches = new MatchRepository(_context);
            StrangerMatchings = new StrangerMatchingRepository(_context);
        }

        public IUserRepository Users { get; }
        public IRoomRepository Rooms { get; }
        public IMessageRepository Messages { get; }
        public IStrangerFilterRepository StrangerFilters { get; }
        public IMatchRepository Matches { get; }
        public IStrangerMatchingRepository StrangerMatchings { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
