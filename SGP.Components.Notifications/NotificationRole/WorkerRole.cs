using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using NotificationRole.Model;

namespace NotificationRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("NotificationRole entry point called", "Information");
            SimulateNotification();

            //while (true)
            //{
            //    Thread.Sleep(10000);
            //    //Trace.WriteLine("Working", "Information");

            //    //Push message

            //    //Read meessage
            //}
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
            //Push message
            string issuerName = CloudConfigurationManager.GetSetting("Issuer"),
                   issuerKey = CloudConfigurationManager.GetSetting("Key"),
                   serviceNamespace = CloudConfigurationManager.GetSetting("ServiceBusNamespace"),
                   inputQueue = CloudConfigurationManager.GetSetting("InputQueueIdentifier"),
                   errorQueue = CloudConfigurationManager.GetSetting("ErrorQueueIdentifier");
            var message = new BrokeredMessage();

            var credentials = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);

            var namespaceClient =
                new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, string.Empty),
                                     credentials);

            var factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, string.Empty), credentials);

            var myQueueClient = factory.CreateQueueClient(inputQueue);


            using (var textReader = new StreamReader(File.Open(@"..\Json\notification-message.txt",
                                                    FileMode.Open)))
            {
                message.Properties.Add("message-" + Guid.NewGuid(), textReader.ReadToEnd());
                myQueueClient.Send(message);
            }

            //Read messages
            while ((message = myQueueClient.Receive(new TimeSpan(0, 0, 5))) != null)
            {
                Trace.WriteLine(string.Format("Message received: {0}, {1}, {2}", message.SequenceNumber, message.GetBody<string>(), message.MessageId));
                message.Complete();

                Trace.WriteLine("Processing message (sleeping...)");
                Thread.Sleep(1000);
            }

            factory.Close();
            myQueueClient.Close();
        }
    }
}
