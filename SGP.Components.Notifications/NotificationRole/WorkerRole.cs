using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;

namespace NotificationRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.WriteLine("NotificationRole entry point called", "Information");
            
            SimulateNotification();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        static void SimulateNotification()
        {

            //Configuration values retriving
            string issuerName = CloudConfigurationManager.GetSetting("Issuer"),
                   issuerKey = CloudConfigurationManager.GetSetting("Key"),
                   serviceNamespace = CloudConfigurationManager.GetSetting("ServiceBusNamespace"),
                   inputQueue = CloudConfigurationManager.GetSetting("InputQueueIdentifier"),
                   errorQueue = CloudConfigurationManager.GetSetting("ErrorQueueIdentifier");
            var message = new BrokeredMessage();

            //Setting up service bus 
            var credentials = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);

            var factory =
                MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("bede", serviceNamespace, string.Empty),
                                        credentials);

            //Input queue client is created
            var inputQueueClient = factory.CreateQueueClient(inputQueue);

            //Get sample json message
            using (var textReader = new StreamReader(File.Open(@".\Json\notification-message.txt",
                                                               FileMode.Open)))
            {
                message.Properties.Add("message-" + Guid.NewGuid(), textReader.ReadToEnd());
                inputQueueClient.Send(message);
            }

            try
            {
                //Read messages
                while ((message = inputQueueClient.Receive(new TimeSpan(0, 0, 5))) != null)
                {
                    Trace.WriteLine(string.Format("Message received: {0}, {1}, {2}", message.SequenceNumber,
                                                  message.GetBody<string>(), message.MessageId));
                    message.Complete();

                    //Perform request to third-party notification service

                    Trace.WriteLine("Processing message (sleeping...)");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                //Handle failure and send message to error queue
                
                var errorQueueClient = factory.CreateQueueClient(errorQueue);
                var errorBrokerMessage = new BrokeredMessage(ex);
                errorQueueClient.Send(errorBrokerMessage);

                //Acknowledge the message
                if (message != null) message.Abandon();
            }

            factory.Close();
            inputQueueClient.Close();
        }

    }
}
