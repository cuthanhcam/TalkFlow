using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Shared.Constants
{
    public static class SignalREvents
    {
        public static class Chat
        {
            public const string JoinRoom = "JoinRoom";
            public const string LeaveRoom = "LeaveRoom";
            public const string UserJoined = "UserJoined";
            public const string UserLeft = "UserLeft";
            public const string SendMessage = "SendMessage";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string MicrophoneMuted = "MicrophoneMuted";
            public const string CameraMuted = "CameraMuted";
            public const string ScreenSharing = "ScreenSharing";
        }

        public static class WebRTC
        {
            public const string SendOffer = "SendWebRTCOffer";
            public const string ReceiveOffer = "ReceiveWebRTCOffer";
            public const string SendAnswer = "SendWebRTCAnswer";
            public const string ReceiveAnswer = "ReceiveWebRTCAnswer";
            public const string SendIceCandidate = "SendIceCandidate";
            public const string ReceiveIceCandidate = "ReceiveIceCandidate";
        }

        public static class Stranger
        {
            public const string StartMatching = "StartMatching";
            public const string StopMatching = "StopMatching";
            public const string MatchingStarted = "MatchingStarted";
            public const string MatchingStopped = "MatchingStopped";
            public const string MatchFound = "MatchFound";
        }
    }
}
