namespace SlotskyDXCluster
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            btnStart1 = new Button();
            comboBoxStart1 = new ComboBox();
            textBox1 = new TextBox();
            lblStart1 = new Label();
            button1 = new Button();
            lblCurrentCount = new Label();
            lblConnectedStatus = new Label();
            btnExit = new Button();
            btnHelp = new Button();
            button2 = new Button();
            panel1 = new Panel();
            radioOlder10days = new RadioButton();
            radioOlder6days = new RadioButton();
            radioOlder4days = new RadioButton();
            radioOlder2days = new RadioButton();
            btnMaintainClusterFile = new Button();
            richTextBox1 = new RichTextBox();
            chkShowMessages = new CheckBox();
            btnSendToCluster = new Button();
            cmboSendToCluster = new ComboBox();
            richTextBoxMessages = new RichTextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart1
            // 
            btnStart1.Location = new Point(3, 29);
            btnStart1.Name = "btnStart1";
            btnStart1.Size = new Size(70, 23);
            btnStart1.TabIndex = 0;
            btnStart1.Text = "Start";
            btnStart1.UseVisualStyleBackColor = true;
            btnStart1.Click += btnStart1_Click;
            // 
            // comboBoxStart1
            // 
            comboBoxStart1.FormattingEnabled = true;
            comboBoxStart1.Location = new Point(79, 29);
            comboBoxStart1.Name = "comboBoxStart1";
            comboBoxStart1.Size = new Size(327, 23);
            comboBoxStart1.TabIndex = 2;
            comboBoxStart1.Text = "dxc.ve7cc.net,23,VE7CC,CCCluster";
            comboBoxStart1.SelectedIndexChanged += comboBoxStart1_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(53, 501);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(644, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "\"Data Source=BOYA-PC; Initial Catalog = SLOTSKY;TrustServerCertificate = true; User Id = ianacook; Password=ianacook\";";
            // 
            // lblStart1
            // 
            lblStart1.AutoSize = true;
            lblStart1.Location = new Point(6, 7);
            lblStart1.Name = "lblStart1";
            lblStart1.Size = new Size(59, 15);
            lblStart1.TabIndex = 6;
            lblStart1.Text = "DXCluster";
            // 
            // button1
            // 
            button1.Location = new Point(317, 3);
            button1.Name = "button1";
            button1.Size = new Size(136, 23);
            button1.TabIndex = 9;
            button1.Text = "Get Current Count";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // lblCurrentCount
            // 
            lblCurrentCount.AutoSize = true;
            lblCurrentCount.Location = new Point(457, 7);
            lblCurrentCount.Name = "lblCurrentCount";
            lblCurrentCount.Size = new Size(76, 15);
            lblCurrentCount.TabIndex = 10;
            lblCurrentCount.Text = "currentcount";
            // 
            // lblConnectedStatus
            // 
            lblConnectedStatus.AutoSize = true;
            lblConnectedStatus.Location = new Point(161, 7);
            lblConnectedStatus.Name = "lblConnectedStatus";
            lblConnectedStatus.Size = new Size(39, 15);
            lblConnectedStatus.TabIndex = 11;
            lblConnectedStatus.Text = "Status";
            // 
            // btnExit
            // 
            btnExit.Location = new Point(684, 3);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(62, 23);
            btnExit.TabIndex = 12;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnHelp
            // 
            btnHelp.Location = new Point(755, 3);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(38, 23);
            btnHelp.TabIndex = 13;
            btnHelp.Text = "?";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += btnHelp_Click;
            // 
            // button2
            // 
            button2.Location = new Point(317, 530);
            button2.Name = "button2";
            button2.Size = new Size(62, 26);
            button2.TabIndex = 14;
            button2.Text = "Remove";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(radioOlder10days);
            panel1.Controls.Add(radioOlder6days);
            panel1.Controls.Add(radioOlder4days);
            panel1.Controls.Add(radioOlder2days);
            panel1.Location = new Point(53, 532);
            panel1.Name = "panel1";
            panel1.Size = new Size(258, 23);
            panel1.TabIndex = 15;
            panel1.Paint += panel1_Paint;
            // 
            // radioOlder10days
            // 
            radioOlder10days.AutoSize = true;
            radioOlder10days.Location = new Point(185, 1);
            radioOlder10days.Name = "radioOlder10days";
            radioOlder10days.Size = new Size(64, 19);
            radioOlder10days.TabIndex = 3;
            radioOlder10days.TabStop = true;
            radioOlder10days.Text = "10 days";
            radioOlder10days.UseVisualStyleBackColor = true;
            // 
            // radioOlder6days
            // 
            radioOlder6days.AutoSize = true;
            radioOlder6days.Location = new Point(121, 1);
            radioOlder6days.Name = "radioOlder6days";
            radioOlder6days.Size = new Size(58, 19);
            radioOlder6days.TabIndex = 2;
            radioOlder6days.TabStop = true;
            radioOlder6days.Text = "6 days";
            radioOlder6days.UseVisualStyleBackColor = true;
            radioOlder6days.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioOlder4days
            // 
            radioOlder4days.AutoSize = true;
            radioOlder4days.Location = new Point(57, 2);
            radioOlder4days.Name = "radioOlder4days";
            radioOlder4days.Size = new Size(58, 19);
            radioOlder4days.TabIndex = 1;
            radioOlder4days.TabStop = true;
            radioOlder4days.Text = "4 days";
            radioOlder4days.UseVisualStyleBackColor = true;
            // 
            // radioOlder2days
            // 
            radioOlder2days.AutoSize = true;
            radioOlder2days.Location = new Point(3, 2);
            radioOlder2days.Name = "radioOlder2days";
            radioOlder2days.Size = new Size(53, 19);
            radioOlder2days.TabIndex = 0;
            radioOlder2days.TabStop = true;
            radioOlder2days.Text = "2 day";
            radioOlder2days.UseVisualStyleBackColor = true;
            // 
            // btnMaintainClusterFile
            // 
            btnMaintainClusterFile.Location = new Point(654, 53);
            btnMaintainClusterFile.Name = "btnMaintainClusterFile";
            btnMaintainClusterFile.Size = new Size(142, 23);
            btnMaintainClusterFile.TabIndex = 16;
            btnMaintainClusterFile.Text = "Maintain Cluster File";
            btnMaintainClusterFile.UseVisualStyleBackColor = true;
            btnMaintainClusterFile.Click += btnMaintainClusterFile_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(3, 197);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(787, 286);
            richTextBox1.TabIndex = 17;
            richTextBox1.Text = "";
            // 
            // chkShowMessages
            // 
            chkShowMessages.AutoSize = true;
            chkShowMessages.Location = new Point(412, 33);
            chkShowMessages.Name = "chkShowMessages";
            chkShowMessages.Size = new Size(178, 19);
            chkShowMessages.TabIndex = 18;
            chkShowMessages.Text = "Show Messages from Cluster";
            chkShowMessages.UseVisualStyleBackColor = true;
            chkShowMessages.CheckedChanged += chkShowMessages_CheckedChanged;
            // 
            // btnSendToCluster
            // 
            btnSendToCluster.Location = new Point(7, 57);
            btnSendToCluster.Name = "btnSendToCluster";
            btnSendToCluster.Size = new Size(101, 23);
            btnSendToCluster.TabIndex = 19;
            btnSendToCluster.Text = "SendToCluster";
            btnSendToCluster.UseVisualStyleBackColor = true;
            btnSendToCluster.Click += btnSendToCluster_Click;
            // 
            // cmboSendToCluster
            // 
            cmboSendToCluster.FormattingEnabled = true;
            cmboSendToCluster.Items.AddRange(new object[] { "SH/WWV", "BYE", "DX <call> <frequency> <any Commant>", "SH/DX", "SET/SKIMMER", "SET/NOSKIMMER", "SH/DX <call>", "SH/DX <band eg 20>", "SET/OWN", "SET/NOOWN", "SET/SELF", "SET/NOSELF", "This where the commands are   http://www.on5jv.com/dx-cluster.html" });
            cmboSendToCluster.Location = new Point(126, 58);
            cmboSendToCluster.Name = "cmboSendToCluster";
            cmboSendToCluster.Size = new Size(280, 23);
            cmboSendToCluster.TabIndex = 20;
            cmboSendToCluster.Text = "BYE";
            // 
            // richTextBoxMessages
            // 
            richTextBoxMessages.Location = new Point(6, 82);
            richTextBoxMessages.Name = "richTextBoxMessages";
            richTextBoxMessages.Size = new Size(790, 109);
            richTextBoxMessages.TabIndex = 21;
            richTextBoxMessages.Text = "";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 608);
            Controls.Add(richTextBoxMessages);
            Controls.Add(cmboSendToCluster);
            Controls.Add(btnSendToCluster);
            Controls.Add(chkShowMessages);
            Controls.Add(richTextBox1);
            Controls.Add(btnMaintainClusterFile);
            Controls.Add(panel1);
            Controls.Add(button2);
            Controls.Add(btnHelp);
            Controls.Add(btnExit);
            Controls.Add(lblConnectedStatus);
            Controls.Add(lblCurrentCount);
            Controls.Add(button1);
            Controls.Add(lblStart1);
            Controls.Add(textBox1);
            Controls.Add(comboBoxStart1);
            Controls.Add(btnStart1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmMain";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "SlotskyDXCluster";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart1;
        private ComboBox comboBoxStart1;
        private TextBox textBox1;
        private Label lblStart1;
        private Button button1;
        private Label lblCurrentCount;
        private Label lblConnectedStatus;
        private Button btnExit;
        private Button btnHelp;
        private Button button2;
        private Panel panel1;
        private RadioButton radioOlder4days;
        private RadioButton radioOlder2days;
        private RadioButton radioOlder6days;
        private RadioButton radioOlder10days;
        private Button btnMaintainClusterFile;
        private RichTextBox richTextBox1;
        private CheckBox chkShowMessages;
        private Button btnSendToCluster;
        private ComboBox cmboSendToCluster;
        private RichTextBox richTextBoxMessages;
    }
}