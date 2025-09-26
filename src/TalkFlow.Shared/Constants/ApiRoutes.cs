using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Shared.Constants
{
    public static class ApiRoutes
    {
        public const string BaseUrl = "api";

        public static class User
        {
            public const string Base = $"{BaseUrl}/user";
            public const string GetById = "{userId}";
            public const string GetAll = "";
            public const string Create = "";
            public const string Update = "{userId}";
            public const string Lock = "{userId}/lock";
            public const string Unlock = "{userId}/unlock";
            public const string Delete = "{userId}";
        }

        public static class Room
        {
            public const string Base = $"{BaseUrl}/room";
            public const string GetById = "{roomId}";
            public const string GetAll = "";
            public const string Create = "";
            public const string Join = "join";
            public const string Update = "{roomId}";
            public const string Delete = "{roomId}";
            public const string BlockChat = "{roomId}/block-chat";
            public const string UnblockChat = "{roomId}/unblock-chat";
        }

        public static class Message
        {
            public const string Base = $"{BaseUrl}/message";
            public const string GetByRoom = "room/{roomId}";
            public const string Send = "";
            public const string Delete = "{messageId}";
        }
    }
}
