using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using PubNub_Messaging;

namespace SGP.Components.Notifications.Client
{
    public partial class PubNubClientForm : Form
    {
        //API key for secure publishing and subscibing to PubNub messaging service
        private string _publishKey;
        private string _subscribeKey;
        private string _secretKey;

        //Message channel
        private string _channel;

        //PubNub Messaging Service
        private Pubnub _pubNubService;

        public PubNubClientForm()
        {
            InitializeComponent();

            _publishKey = ConfigurationManager.AppSettings["PubNubPublishKey"];
            _subscribeKey = ConfigurationManager.AppSettings["PubNubSubscribeKey"];
            _secretKey = ConfigurationManager.AppSettings["PubNubSecretKey"];

            _pubNubService = new Pubnub(_publishKey, _subscribeKey, _secretKey);

            cmbUserIds.SelectedIndex = 0;
        }

        private void BtnSubClick(object sender, System.EventArgs e)
        {
            //Should be user id
            _channel = cmbUserIds.Text;

            if (string.IsNullOrEmpty(_channel))
                MessageBox.Show("Empty channel name field!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            _pubNubService.PropertyChanged += delegate(object pubNubSender, PropertyChangedEventArgs eventArgs)
            {
                if (eventArgs.PropertyName == "History")
                {
                    rtbMessages.Invoke((MethodInvoker)delegate
                                                          {
                                               rtbMessages.Text += "Retrieving message history\n";
                                               foreach (var t in ((Pubnub)pubNubSender).History)
                                               {
                                                   rtbMessages.Text += "* Message: " + t + "\n";
                                               }  
                                           });
                }
            };

            //Fetch last 10 messages from pubnub 
            _pubNubService.history(_channel, 10);
            
            //Start background worker if it's not busy
            if (!bckgPubNubSubscriptionWorker.IsBusy)
                bckgPubNubSubscriptionWorker.RunWorkerAsync();
        }

        private void BtnStopSubClick(object sender, System.EventArgs e)
        {

        }

        private void BckgPubNubSubscriptionWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            _pubNubService.PropertyChanged += delegate(object pubNubeSender, PropertyChangedEventArgs eventArgs)
            {
                if (eventArgs.PropertyName == "ReturnMessage")
                {
                    rtbMessages.Invoke((MethodInvoker)delegate
                                                          {
                        rtbMessages.Text += "* Message retrieved: ";
                        rtbMessages.Text += (((Pubnub)pubNubeSender).ReturnMessage);
                        rtbMessages.Update();
                    });
                }
            };
            //Subscribe to the messaging api
            _pubNubService.subscribe(_channel);
        }

        public bool SubscriptionUpdates(object message)
        {
            if (rtbMessages != null)
            {
                rtbMessages.Text += "* Message retrieved!";
                rtbMessages.Text += message + "\r\n";
                rtbMessages.Update();
            }
            return true;
        }
    }
}
