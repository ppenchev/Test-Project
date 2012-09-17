using NotificationRole.Model;

namespace NotificationRole
{
    public interface IPublishNotificationMessageManager
    {
        bool Publish(Message message);
    }
}
