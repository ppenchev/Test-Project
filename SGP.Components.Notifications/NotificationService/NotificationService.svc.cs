using System;
using System.IO;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificationService.Model;

namespace NotificationService
{
    public class NotificationService : INotificationService
    {
        public Message GetMessages()
        {
            //Push message for mocking and read after that
            Queue();
            
            //Push to the browser

            //if something wrong happens push to error queue
        }

        void Queue()
        {
            //Push message
            string IssuerName = string.Empty, IssuerKey = string.Empty, ServiceNamespace = string.Empty;
            var credentials = TokenProvider.CreateSharedSecretTokenProvider(IssuerName, IssuerKey);

            var nameSpaceClient =
                new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty),
                                     credentials);

            var myQueue = nameSpaceClient.GetQueue("");

            var factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);

            var myQueueClient = factory.CreateQueueClient("");

            using (var textReader = new StreamReader(File.Open(@"H:\Projects\git-remote\Test-Project\SGP.Components.Notifications\NotificationService\Json\notification-message.txt",
                                                    FileMode.Open)))
            {
                var resultMessage = JsonConvert.DeserializeObject<Message>(textReader.ReadToEnd());
                //var message = new BrokeredMessage();
                //message.
                //myQueueClient.Send(resultMessage);
            }

            //Read messages
            BrokeredMessage message;
            while ((message = myQueueClient.Receive(new TimeSpan(hours: 0, minutes: 0, seconds: 5))) != null)
            {
                Console.WriteLine(string.Format("Message received: {0}, {1}, {2}", message.SequenceNumber, message.Label, message.MessageId));
                message.Complete();

                Console.WriteLine("Processing message (sleeping...)");
                Thread.Sleep(1000);
            }

            factory.Close();
            myQueueClient.Close();
            namespaceClient.DeleteQueue("IssueTrackingQueue");
        }
    }
}
