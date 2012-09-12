using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using NotificationRole.Model;

namespace NotificationRole
{
    public class WorkerRole : RoleEntryPoint
    {
        //Congiguration value fields
        private string _issuerName;
        private string _issuerKey;
        private string _serviceNamespace;
        private string _inputQueue;
        private string _errorQueue;

        private MessagingFactory _factory;

        //Objects for comunicating with the queues
        private QueueClient _inputQueueClient;
        private QueueClient _errorQueueClient;

        public WorkerRole()
        {
            //Configuration values retrieving
            _issuerName = CloudConfigurationManager.GetSetting("Issuer");
            _issuerKey = CloudConfigurationManager.GetSetting("Key");
            _serviceNamespace = CloudConfigurationManager.GetSetting("ServiceBusNamespace");
            _inputQueue = CloudConfigurationManager.GetSetting("InputQueueIdentifier");
            _errorQueue = CloudConfigurationManager.GetSetting("ErrorQueueIdentifier");
        }

        public override void Run()
        {
            Trace.WriteLine("NotificationRole entry point called", "Information");
            
            //Read messages
            while (true)
            {
                //For experimental purposes we send message and right after that read it from the queue.
                MockSend();

                try
                {
                    var inputMessage = _inputQueueClient.Receive();

                    Trace.WriteLine(string.Format("Message received: {0}, {1}", inputMessage.SequenceNumber, inputMessage.MessageId));
                    inputMessage.Complete();

                    //Perform request to third-party notification service. Skeleton implementation
                    IPushMessageNotification notifier = new DummyMessageNotification();
                    notifier.Send(new Message
                                      {
                                          BrowserMessageType = "top-right-panel-id",
                                          NotificationType = "browser",
                                          Payload = "Additional information here.",
                                          UserId = "abc-123"
                                      });
                }
                catch (Exception ex)
                {
                    //Handle failure and send message to error queue
                    var errorBrokerMessage = new BrokeredMessage();
                    errorBrokerMessage.Properties.Add(ex.GetType().ToString(), ex.Message);
                    _errorQueueClient.Send(errorBrokerMessage);
                }
                
                Thread.Sleep(10000);
            }
        }

        public override void OnStop()
        {
            base.OnStop();

            _factory.Close();
            _inputQueueClient.Close();
            _errorQueueClient.Close();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //Setting up service bus 
            var credentials = TokenProvider.CreateSharedSecretTokenProvider(_issuerName, _issuerKey);

            _factory = 
                MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", _serviceNamespace, string.Empty), credentials);

            //Input queue client and error queue client creation
            _inputQueueClient = _factory.CreateQueueClient(_inputQueue);
            _errorQueueClient = _factory.CreateQueueClient(_errorQueue);            
            
            return base.OnStart();
        }

        void MockSend()
        {
            using (var fileStrem = File.Open(@".\Json\notification-message.txt", FileMode.Open))
            {
                  //Mock sending notification to queue. Get sample json file representing notification message
                var message = new BrokeredMessage(fileStrem, true);
                //message.Properties.Add("message-" + Guid.NewGuid(), textReader.ReadToEnd());
                _inputQueueClient.Send(message);
            }
          
        }
    }

    public interface IPushMessageNotification
    {
        void Send(Message message);
    }

    public class DummyMessageNotification : IPushMessageNotification
    {
        public void Send(Message message)
        {
            //nothing goes here
        }
    }
}
