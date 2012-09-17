using System;
using System.Diagnostics;
using System.IO;
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
            //Read messages
            while (true)
            {
                BrokeredMessage inputMessage = null;
                Stream messageStream = null;
                string messageBody;
                try
                {

                    inputMessage = _inputQueueClient.Receive();

                    Trace.WriteLine(string.Format("Message received: {0}, {1}", inputMessage.SequenceNumber, inputMessage.MessageId));

                    inputMessage.Complete();

                    //Retrieve meessage body 
                    //Message inputMessageObject;
                    messageStream = inputMessage.GetBody<Stream>();
                    TextReader reader = new StreamReader(messageStream, false);
                    //Problem with schema
                    messageBody = reader.ReadToEnd();
                    //inputMessageObject = JsonConvert.DeserializeObject<Message>(messageBody);
                    
                    //Perform request to third-party notification service. Skeleton implementation
                    IPublishNotificationMessageManager messageManager = new PubNubNotificationMessageManager();
                    messageManager.Publish(messageBody);

                    //Trace.WriteLine(string.Format("* Message.UserId:{0}, Status(PubNub):{1}", inputMessageObject.UserId, info[1]));
                }
                catch (Exception ex)
                {
                    //Handle failure and send message. Post a copy of the input message to error queue.
                    if (inputMessage != null)
                    {
                        var errorMessage = new BrokeredMessage(messageStream, true);
                        //We are adding and information about the occured exeption.     
                        errorMessage.Properties.Add("Exception", ex.Message);
                        errorMessage.Properties.Add("ExceptionStackTrace", ex.StackTrace);

                        _errorQueueClient.Send(errorMessage);
                    }
                }
                finally
                {
                    if (messageStream != null) messageStream.Close();
                }

                Thread.Sleep(3000);
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
    }
}
