using System.Runtime.Serialization;

namespace SGP.Components.Notifications.PostApp.Model
{
    [DataContract(Namespace = "")]
    public class Message 
    {
        [DataMember(Order = 1)]
        public string UserId { get; set; }
        [DataMember(Order = 2)]
        public string NotificationType { get; set; }
        [DataMember(Order = 3)]
        public string BrowserMessageType { get; set; }
        [DataMember(Order = 4)]
        public string Payload { get; set; }
    }
}