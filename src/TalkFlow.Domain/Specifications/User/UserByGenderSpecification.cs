using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Specifications.User
{
    public class UserByGenderSpecification : BaseSpecification<TalkFlow.Domain.Aggregates.User.User>
    {
        public UserByGenderSpecification(Gender gender)
            : base(u => u.Gender != null && u.Gender.Value == gender.Value)
        {
        }
    }
}
