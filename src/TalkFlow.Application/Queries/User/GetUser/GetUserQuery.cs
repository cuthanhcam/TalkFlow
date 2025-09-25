using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Queries.User.GetUser
{
    public record GetUserQuery(UserId UserId) : IQuery<Result<UserDto>>;
}
