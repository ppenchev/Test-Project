﻿using Newtonsoft.Json;

namespace NotificationRole.Model
{
    [JsonObject]
    public class Message
    {
        [JsonProperty]
        public string UserId { get; set; }
        [JsonProperty]
        public string NotificationType { get; set; }
        [JsonProperty]
        public string BrowserMessageType { get; set; }
        [JsonProperty]
        public string Payload { get; set; }
    }
}