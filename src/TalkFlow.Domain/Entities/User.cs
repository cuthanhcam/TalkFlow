using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Preferences { get; set; }
    }
}