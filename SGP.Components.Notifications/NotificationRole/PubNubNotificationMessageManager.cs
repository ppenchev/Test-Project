using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using NotificationRole.Model;

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
            
            _pubNubService = new Pubnub(_publishKey, _subscribeKey, _secretKey, false);
        }

        //
        public PubNubNotificationMessageManager(Pubnub pubnub)
        {
            _pubNubService = pubnub;
        }

        public List<object> Publish(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message", "The message you want to publish to PubNub is null!");

            //Create json from given message
            //var jsonMessage = JsonConvert.SerializeObject(message);

            return _pubNubService.Publish(_channel, message);
        }
    }
}
