using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using SGP.Components.Notifications.PostApp.Model;

namespace SGP.Components.Notifications.PostApp
{
    class Program
    {
        //Congiguration value fields
        private static string _issuerName;
        private static string _issuerKey;
        private static string _serviceNamespace;
        private static string _inputQueue;
        
        private static MessagingFactory _factory;

        //Objects for comunicating with the queues
        private static QueueClient _inputQueueClient;

        

        static void Main(string[] args)
        {
            //generate messages -> collect input -> send -> free resources
            Setup();
            CollectInput();
            //Send();
            //End();    

            Console.ReadLine();
        }

        private static void End()
        {
            _inputQueueClient.Close();
        }

        private static void CollectInput()
        {
            var usersChoise = string.Empty;
            while (usersChoise != "1" && usersChoise != "2")
            {
                Console.WriteLine("Press \"1\" for random generated messages and \"2\" for direct typing.");
                usersChoise = Console.ReadLine();
            }

            switch (usersChoise)
            {
                case "1":

                    var messageCount = 0;
                    while (messageCount < 1 || messageCount > 50)
                    {
                        Console.WriteLine("How many messages do you want to send to the queue? [1 - 50] ");
                        int.TryParse(Console.ReadLine(), out messageCount);
                    }

                    //Generate or input message
                    Console.WriteLine("\r\nJSON Messages to be send:");
                    Console.WriteLine("=========================================================================\r\n");
                    var fixture = new Fixture();
                    foreach (var messageJson in fixture.CreateMany<Message>(messageCount).Select(message => JsonConvert.SerializeObject(message)))
                    {
                        Console.WriteLine(messageJson + "\r\n");
                    }

                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("Oppsss! This is not supposed to happend!");
                    break;
            }
        }

        static void Setup()
        {
            Console.WriteLine("Reading configuration...");

            _serviceNamespace = ConfigurationManager.AppSettings["ServiceBusNamespace"]; 
            Console.WriteLine(String.Format("* Service bus namespace is \"{0}\"", _serviceNamespace));

            _issuerName = ConfigurationManager.AppSettings["Issuer"];
            Console.WriteLine(String.Format("* Issuer \"{0}\"", _issuerName));

            _issuerKey = ConfigurationManager.AppSettings["Key"];
            Console.WriteLine(String.Format("* Key \"{0}\"", _issuerKey));

            _inputQueue = ConfigurationManager.AppSettings["InputQueueIdentifier"];
            Console.WriteLine(String.Format("* Input queue identifier \"{0}\" \r\n", _inputQueue));
            
            var credentials = TokenProvider.CreateSharedSecretTokenProvider(_issuerName, _issuerKey);
            _factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", _serviceNamespace, string.Empty), credentials);
            _inputQueueClient = _factory.CreateQueueClient(_inputQueue);
        }

        static void Send()
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
