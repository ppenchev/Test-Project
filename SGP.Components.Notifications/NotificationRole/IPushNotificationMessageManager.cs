using NotificationRole.Model;

namespace NotificationRole
{
    public interface IPushNotificationMessageManager
    {
        void Send(IPushNotificationMessage message);
    }
}
