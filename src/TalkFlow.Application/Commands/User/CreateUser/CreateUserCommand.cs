using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.User;

namespace TalkFlow.Application.Commands.User.CreateUser
{
    public record CreateUserCommand(CreateUserDto UserData) : ICommand<Result<UserDto>>;
}
