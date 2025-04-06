using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalkFlow.Domain.Entities;

namespace TalkFlow.Infrastructure.Data
{
    public class TalkFlowDbContext : DbContext
    {
        public TalkFlowDbContext(DbContextOptions<TalkFlowDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}