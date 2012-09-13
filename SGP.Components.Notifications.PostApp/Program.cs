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

        //Array that holds the messages that will be generated / entered
        private static string[] _messages;

        static void Main(string[] args)
        {
            try
            {
                //generate messages -> collect input -> send -> free resources
                Setup();
                ComposeMessages();
                Send();
                End();    

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                
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

        private static void ComposeMessages()
        {
            var usersChoise = string.Empty;
            while (usersChoise != "1" && usersChoise != "2")
            {
                Console.WriteLine("Press \"1\" for random generated messages and \"2\" for manual input.");
                usersChoise = Console.ReadLine();
            }

            switch (usersChoise)
            {
                case "1":

                    var messagesCount = 0;
                    while (messagesCount < 1 || messagesCount > 50)
                    {
                        Console.WriteLine("How many messages do you want to send to the queue? [1 - 50] ");
                        int.TryParse(Console.ReadLine(), out messagesCount);
                    }

                    //Generate or input message
                    Console.WriteLine("\r\nJSON Messages to be send:");
                    Console.WriteLine("=========================================================================\r\n");
                    
                    var fixture = new Fixture();
                    _messages = new string[messagesCount];

                    _messages =
                        fixture.CreateMany<Message>(messagesCount).Select(message => JsonConvert.SerializeObject(message))
                            .ToArray();

                    foreach (var messageJson in _messages)
                    {
                        Console.WriteLine(messageJson + "\r\n");
                    }

                    Console.WriteLine("=========================================================================\r\n");

                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("Oppsss! This is not supposed to happened!");
                    break;
            }
        }
        
        static void Send()
        {
            Console.Write(String.Format("Sending {0} message(s)", _messages.Length));

            if (_messages == null) 
                throw new ArgumentNullException("Messages is collection is null.");

            foreach (var message in _messages)
            {
                var queueMessage = new BrokeredMessage(message);
                _inputQueueClient.Send(queueMessage);
                Console.Write(".");
            }

            Console.Write("\r\nMessage(s) were send successfully!");
        }

        private static void End()
        {
            _inputQueueClient.Close();
        }
    }
}
