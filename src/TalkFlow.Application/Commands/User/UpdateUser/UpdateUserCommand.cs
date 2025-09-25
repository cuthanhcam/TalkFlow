using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Application.Commands.User.UpdateUser
{
    public record UpdateUserCommand(UserId UserId, UpdateUserDto UserData) : ICommand<Result<UserDto>>;
}
