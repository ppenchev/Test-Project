using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using NotificationRole.Model;
using PubNub_Messaging;

namespace NotificationRole
{
    public class PubNubNotificationMessageManager : IPublishNotificationMessageManager
    {
        //API key for secure publishing and subscibing to PubNub messaging service
        private string _publishKey;
        private string _subscribeKey;
        private string _secretKey;

        //Message channel
        private string _channel;

        //PubNub Messaging Service
        private Pubnub _pubNubService;

        public PubNubNotificationMessageManager()
        {
            //Retrieve configuration values
            _publishKey = CloudConfigurationManager.GetSetting("PubNubPublishKey");
            _subscribeKey = CloudConfigurationManager.GetSetting("PubNubSubscribeKey");
            _secretKey = CloudConfigurationManager.GetSetting("PubNubSecretKey");
            _channel = CloudConfigurationManager.GetSetting("PubNubMessageChannel");
            
            _pubNubService = new Pubnub(_publishKey, _subscribeKey, _secretKey);
        }

        //
        public PubNubNotificationMessageManager(Pubnub pubnub)
        {
            _pubNubService = pubnub;
        }

        public bool Publish(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message", "The message you want to publish to PubNub is null!");

            //Create json from given message
            //var jsonMessage = JsonConvert.SerializeObject(message);
            _pubNubService.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Publish")
                {
                    Trace.WriteLine(
                        "Publish Success: " + ((Pubnub)sender).Publish[0] +
                        "\nPublish Info: " + ((Pubnub)sender).Publish[1]
                        );
                }
            };

            //Every user should have own channel. The name of the channel is the user id.
            _channel = message.UserId;

            return _pubNubService.publish(_channel, message);
        }
    }
}
