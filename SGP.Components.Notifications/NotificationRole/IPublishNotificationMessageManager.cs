using NotificationRole.Model;

namespace NotificationRole
{
    public interface IPublishNotificationMessageManager
    {
        void Publish(IPushNotificationMessage message);
    }
}
