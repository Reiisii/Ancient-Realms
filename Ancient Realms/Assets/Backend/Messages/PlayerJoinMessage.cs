using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Entities;
using Unisave.Facades;
using Unisave.Broadcasting;
using ESDatabase.Entities;
namespace ESDatabase.Messages
{
    public class ChatMessage : BroadcastingMessage
    {
        public string playerName;
        public string message;
    }
    public class PlayerJoinMessage : BroadcastingMessage
    {
        public string playerName;
    }
    public class NewExistingSession : BroadcastingMessage
    {
        public string message;
    }
}
