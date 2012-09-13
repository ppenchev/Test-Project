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
                    errorBrokerMessage.Properties.Add("Exception", ex.Message);
                    errorBrokerMessage.Properties.Add("ExceptionStackTrace", ex.StackTrace);
                    _errorQueueClient.Send(errorBrokerMessage);
                }
                
                Thread.Sleep(10000);
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

        public override void OnStop()
        {
            base.OnStop();

            _factory.Close();
            _inputQueueClient.Close();
            _errorQueueClient.Close();

            //
            _factory = null;
            _inputQueueClient = null;
            _errorQueueClient = null;
        }

        public override bool OnStart()
        {
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
                //This line will be included when we have clearance on what descriptors should a message has.
                //message.Properties.Add("message-" + Guid.NewGuid(), textReader.ReadToEnd());
                _inputQueueClient.Send(message);
            }
        }
    }
}
