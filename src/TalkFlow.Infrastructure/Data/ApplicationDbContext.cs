using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
using TalkFlow.Domain.Aggregates.User;
using TalkFlow.Domain.Aggregates.Matching.Match;
using TalkFlow.Domain.Aggregates.Matching.StrangerMatching;
using TalkFlow.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TalkFlow.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<StrangerFilter> StrangerFilters { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<StrangerMatching> StrangerMatchings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new RoomConfiguration());
            builder.ApplyConfiguration(new ConnectionConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new StrangerFilterConfiguration());
            builder.ApplyConfiguration(new MatchConfiguration());
            builder.ApplyConfiguration(new StrangerMatchingConfiguration());
        }
    }
}
