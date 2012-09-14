using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

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
        }

        private void btnSub_Click(object sender, System.EventArgs e)
        {
            _channel = txtChannel.Text;

            if (string.IsNullOrEmpty(_channel))
                MessageBox.Show("Empty channel name field!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //Fetch last 10 messages from pubnub 
            var topMessages = _pubNubService.History(_channel, 10);
            
            //Start background worker 
            bckgPubNubSubscriptionWorker.RunWorkerAsync();
        }

        private void btnStopSub_Click(object sender, System.EventArgs e)
        {

        }

        private void bckgPubNubSubscriptionWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //Subscribe to the messaging api
            _pubNubService.Subscribe(_channel, SubscriptionUpdates);
        }

        private bool SubscriptionUpdates(object message)
        {
            rtbMessages.Text += message + @"\r\n";
            rtbMessages.Update();
            return true;
        }
    }
}
