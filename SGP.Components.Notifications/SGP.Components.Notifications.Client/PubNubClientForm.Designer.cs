namespace SGP.Components.Notifications.Client
{
    partial class PubNubClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PubNubClientForm));
            this.label2 = new System.Windows.Forms.Label();
            this.btnStopSub = new System.Windows.Forms.Button();
            this.btnSub = new System.Windows.Forms.Button();
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.bckgPubNubSubscriptionWorker = new System.ComponentModel.BackgroundWorker();
            this.cmbUserIds = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Channel Name: ";
            // 
            // btnStopSub
            // 
            this.btnStopSub.Location = new System.Drawing.Point(156, 54);
            this.btnStopSub.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStopSub.Name = "btnStopSub";
            this.btnStopSub.Size = new System.Drawing.Size(119, 33);
            this.btnStopSub.TabIndex = 3;
            this.btnStopSub.Text = "Close";
            this.btnStopSub.UseVisualStyleBackColor = true;
            this.btnStopSub.Click += new System.EventHandler(this.BtnStopSubClick);
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(29, 54);
            this.btnSub.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(119, 33);
            this.btnSub.TabIndex = 2;
            this.btnSub.Text = "Subscribe";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.BtnSubClick);
            // 
            // rtbMessages
            // 
            this.rtbMessages.Location = new System.Drawing.Point(24, 97);
            this.rtbMessages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.ReadOnly = true;
            this.rtbMessages.Size = new System.Drawing.Size(757, 486);
            this.rtbMessages.TabIndex = 4;
            this.rtbMessages.Text = resources.GetString("rtbMessages.Text");
            // 
            // bckgPubNubSubscriptionWorker
            // 
            this.bckgPubNubSubscriptionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BckgPubNubSubscriptionWorkerDoWork);
            // 
            // cmbUserIds
            // 
            this.cmbUserIds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserIds.FormattingEnabled = true;
            this.cmbUserIds.Items.AddRange(new object[] {
            "userChannel-1",
            "userChannel-2",
            "userChannel-3",
            "userChannel-4",
            "userChannel-5",
            "userChannel-6",
            "userChannel-7",
            "userChannel-8",
            "userChannel-9",
            "userChannel-10"});
            this.cmbUserIds.Location = new System.Drawing.Point(144, 17);
            this.cmbUserIds.Name = "cmbUserIds";
            this.cmbUserIds.Size = new System.Drawing.Size(637, 27);
            this.cmbUserIds.TabIndex = 1;
            // 
            // PubNubClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 602);
            this.Controls.Add(this.cmbUserIds);
            this.Controls.Add(this.btnStopSub);
            this.Controls.Add(this.btnSub);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(817, 640);
            this.MinimumSize = new System.Drawing.Size(817, 640);
            this.Name = "PubNubClientForm";
            this.Text = "PubNub Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStopSub;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.ComponentModel.BackgroundWorker bckgPubNubSubscriptionWorker;
        private System.Windows.Forms.ComboBox cmbUserIds;
    }
}

