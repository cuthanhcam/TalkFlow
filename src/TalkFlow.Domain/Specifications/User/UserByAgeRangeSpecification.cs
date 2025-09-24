using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Specifications.User
{
    public class UserByAgeRangeSpecification : BaseSpecification<TalkFlow.Domain.Aggregates.User.User>
    {
        public UserByAgeRangeSpecification(int minAge, int maxAge)
            : base(u => u.Age != null && u.Age.Value >= minAge && u.Age.Value <= maxAge)
        {
        }
    }
}
