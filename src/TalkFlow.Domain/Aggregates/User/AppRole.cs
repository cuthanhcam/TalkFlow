using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Aggregates.User
{
    public class AppRole : IdentityRole<Guid> // entity
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
