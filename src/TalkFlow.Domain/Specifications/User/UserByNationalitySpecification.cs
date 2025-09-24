using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Specifications.User
{
    public class UserByNationalitySpecification : BaseSpecification<TalkFlow.Domain.Aggregates.User.User>
    {
        public UserByNationalitySpecification(Nationality nationality)
            : base(u => u.Nationality != null && u.Nationality.Value == nationality.Value)
        {
        }
    }
}
