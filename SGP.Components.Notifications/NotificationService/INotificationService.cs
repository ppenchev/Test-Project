using System.ServiceModel;
using System.ServiceModel.Web;
using NotificationService.Model;

namespace NotificationService
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/message", ResponseFormat = WebMessageFormat.Json)]
        Message GetMessages();
    }
}
