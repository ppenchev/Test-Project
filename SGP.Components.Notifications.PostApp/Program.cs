﻿using System;
using System.Configuration;
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

        static void Main()
        {
            try
            {
                //generate messages -> collect input -> send -> free resources
                Setup();

                var exit = string.Empty;
                while (exit != "exit")
                {
                    ComposeMessages();
                    Send();

                    Console.WriteLine("Type \"exit\" if you want to quit, else just press \"Enter\".");
                    exit = Console.ReadLine();
                }

                End();        
            }
            catch (Exception ex)
            {
                Console.WriteLine("***An exception has occured***");
                Console.WriteLine("TYPE: {0}", ex.GetType());
                Console.WriteLine(string.Format("STACK TRACE {0}", ex.StackTrace));
            }
        }

        static void Setup()
        {
            Console.WriteLine("Reading configuration...");

            _serviceNamespace = ConfigurationManager.AppSettings["ServiceBusNamespace"];
            Console.WriteLine(value: String.Format("* Service bus namespace is \"{0}\"", _serviceNamespace));

            _issuerName = ConfigurationManager.AppSettings["Issuer"];
            Console.WriteLine(String.Format("* Issuer \"{0}\"", _issuerName));

            _issuerKey = ConfigurationManager.AppSettings["Key"];
            Console.WriteLine(String.Format("* Key \"{0}\"", _issuerKey));

            _inputQueue = ConfigurationManager.AppSettings["InputQueueIdentifier"];
            Console.WriteLine(String.Format("* Input queue identifier \"{0}\" \r\n", _inputQueue));

            //Create queue client for posting to the input queue
            var credentials = TokenProvider.CreateSharedSecretTokenProvider(_issuerName, _issuerKey);
            _factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", _serviceNamespace, string.Empty), credentials);
            _inputQueueClient = _factory.CreateQueueClient(_inputQueue);
        }

        private static void ComposeMessages()
        {
            //User could use automation approach for sending messages or could create some message on his own.
            var usersChoise = string.Empty;
            while (usersChoise != "1" && usersChoise != "2")
            {
                Console.WriteLine("Press \"1\" for random generated messages and \"2\" for manual input.");
                usersChoise = Console.ReadLine();
            }

            switch (usersChoise)
            {
                //automation 
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

                    //Display messages in the console
                    foreach (var messageJson in _messages)
                    {
                        Console.WriteLine(messageJson + "\r\n");
                    }

                    Console.WriteLine("=========================================================================\r\n");

                    break;
                //manual 
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
                //Create message based on generated or typed-in ones -> Send them to the input queue
                var queueMessage = new BrokeredMessage(message);
                _inputQueueClient.Send(queueMessage);
                Console.Write(".");
            }

            Console.WriteLine("\r\nMessage(s) were send successfully!");
        }

        private static void End()
        {
            _inputQueueClient.Close();
        }
    }
}