using System.Collections.Generic;
using NotificationRole.Model;

namespace NotificationRole
{
    public interface IPublishNotificationMessageManager
    {
        List<object> Publish(IPushNotificationMessage message);
    }
}
