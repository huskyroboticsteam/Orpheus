namespace Science_Base
{
    partial class MainWindow
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.EmergencyStopBtn = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Control = new System.Windows.Forms.TabPage();
			this.Debugging = new System.Windows.Forms.TabPage();
			this.DebugList = new System.Windows.Forms.TableLayoutPanel();
			this.PacketGenBox = new System.Windows.Forms.GroupBox();
			this.PacketGenLayout = new System.Windows.Forms.TableLayoutPanel();
			this.TimestampTitle = new DarkUI.Controls.DarkLabel();
			this.IDTitle = new DarkUI.Controls.DarkLabel();
			this.IDTextbox = new DarkUI.Controls.DarkTextBox();
			this.DataTextbox = new DarkUI.Controls.DarkTextBox();
			this.DataTitle = new DarkUI.Controls.DarkLabel();
			this.SendPacketBtn = new DarkUI.Controls.DarkButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.UseCurrentTime = new DarkUI.Controls.DarkCheckBox();
			this.TimestampTextbox = new DarkUI.Controls.DarkTextBox();
			this.InterpretationTimestamp = new DarkUI.Controls.DarkLabel();
			this.InterpretationID = new DarkUI.Controls.DarkLabel();
			this.InterpretationData = new DarkUI.Controls.DarkLabel();
			this.PacketConstructStatus = new DarkUI.Controls.DarkLabel();
			this.SecTimer = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.Debugging.SuspendLayout();
			this.DebugList.SuspendLayout();
			this.PacketGenBox.SuspendLayout();
			this.PacketGenLayout.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.EmergencyStopBtn, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 561);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// EmergencyStopBtn
			// 
			this.EmergencyStopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.EmergencyStopBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
			this.EmergencyStopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EmergencyStopBtn.ForeColor = System.Drawing.Color.White;
			this.EmergencyStopBtn.Location = new System.Drawing.Point(322, 525);
			this.EmergencyStopBtn.Name = "EmergencyStopBtn";
			this.EmergencyStopBtn.Padding = new System.Windows.Forms.Padding(5);
			this.EmergencyStopBtn.Size = new System.Drawing.Size(139, 33);
			this.EmergencyStopBtn.TabIndex = 999;
			this.EmergencyStopBtn.Text = "EMERGENCY STOP";
			this.EmergencyStopBtn.UseVisualStyleBackColor = false;
			this.EmergencyStopBtn.Click += new System.EventHandler(this.EmergencyStopClick);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Control);
			this.tabControl1.Controls.Add(this.Debugging);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(0, 0);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(784, 522);
			this.tabControl1.TabIndex = 0;
			// 
			// Control
			// 
			this.Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
			this.Control.Location = new System.Drawing.Point(4, 22);
			this.Control.Margin = new System.Windows.Forms.Padding(0);
			this.Control.Name = "Control";
			this.Control.Size = new System.Drawing.Size(776, 496);
			this.Control.TabIndex = 0;
			this.Control.Text = "Control";
			// 
			// Debugging
			// 
			this.Debugging.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
			this.Debugging.Controls.Add(this.DebugList);
			this.Debugging.Location = new System.Drawing.Point(4, 22);
			this.Debugging.Margin = new System.Windows.Forms.Padding(0);
			this.Debugging.Name = "Debugging";
			this.Debugging.Size = new System.Drawing.Size(776, 496);
			this.Debugging.TabIndex = 1;
			this.Debugging.Text = "Debugging";
			// 
			// DebugList
			// 
			this.DebugList.ColumnCount = 1;
			this.DebugList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.DebugList.Controls.Add(this.PacketGenBox, 0, 0);
			this.DebugList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DebugList.Location = new System.Drawing.Point(0, 0);
			this.DebugList.Margin = new System.Windows.Forms.Padding(0);
			this.DebugList.Name = "DebugList";
			this.DebugList.RowCount = 4;
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.DebugList.Size = new System.Drawing.Size(776, 496);
			this.DebugList.TabIndex = 0;
			// 
			// PacketGenBox
			// 
			this.PacketGenBox.Controls.Add(this.PacketGenLayout);
			this.PacketGenBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PacketGenBox.ForeColor = System.Drawing.Color.Gainsboro;
			this.PacketGenBox.Location = new System.Drawing.Point(5, 5);
			this.PacketGenBox.Margin = new System.Windows.Forms.Padding(5);
			this.PacketGenBox.MinimumSize = new System.Drawing.Size(0, 100);
			this.PacketGenBox.Name = "PacketGenBox";
			this.PacketGenBox.Size = new System.Drawing.Size(766, 114);
			this.PacketGenBox.TabIndex = 1;
			this.PacketGenBox.TabStop = false;
			this.PacketGenBox.Text = "Packet Builder";
			// 
			// PacketGenLayout
			// 
			this.PacketGenLayout.ColumnCount = 4;
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
			this.PacketGenLayout.Controls.Add(this.TimestampTitle, 0, 0);
			this.PacketGenLayout.Controls.Add(this.IDTitle, 1, 0);
			this.PacketGenLayout.Controls.Add(this.IDTextbox, 1, 1);
			this.PacketGenLayout.Controls.Add(this.DataTextbox, 2, 1);
			this.PacketGenLayout.Controls.Add(this.DataTitle, 2, 0);
			this.PacketGenLayout.Controls.Add(this.SendPacketBtn, 3, 1);
			this.PacketGenLayout.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.PacketGenLayout.Controls.Add(this.InterpretationTimestamp, 0, 2);
			this.PacketGenLayout.Controls.Add(this.InterpretationID, 1, 2);
			this.PacketGenLayout.Controls.Add(this.InterpretationData, 2, 2);
			this.PacketGenLayout.Controls.Add(this.PacketConstructStatus, 3, 2);
			this.PacketGenLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PacketGenLayout.Location = new System.Drawing.Point(3, 16);
			this.PacketGenLayout.Margin = new System.Windows.Forms.Padding(0);
			this.PacketGenLayout.Name = "PacketGenLayout";
			this.PacketGenLayout.RowCount = 3;
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.PacketGenLayout.Size = new System.Drawing.Size(760, 95);
			this.PacketGenLayout.TabIndex = 0;
			// 
			// TimestampTitle
			// 
			this.TimestampTitle.AutoSize = true;
			this.TimestampTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TimestampTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.TimestampTitle.Location = new System.Drawing.Point(3, 0);
			this.TimestampTitle.Name = "TimestampTitle";
			this.TimestampTitle.Size = new System.Drawing.Size(190, 13);
			this.TimestampTitle.TabIndex = 0;
			this.TimestampTitle.Text = "Timestamp";
			// 
			// IDTitle
			// 
			this.IDTitle.AutoSize = true;
			this.IDTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.IDTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.IDTitle.Location = new System.Drawing.Point(199, 0);
			this.IDTitle.Name = "IDTitle";
			this.IDTitle.Size = new System.Drawing.Size(190, 13);
			this.IDTitle.TabIndex = 2;
			this.IDTitle.Text = "ID";
			// 
			// IDTextbox
			// 
			this.IDTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IDTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
			this.IDTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.IDTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.IDTextbox.Location = new System.Drawing.Point(199, 16);
			this.IDTextbox.Name = "IDTextbox";
			this.IDTextbox.Size = new System.Drawing.Size(190, 20);
			this.IDTextbox.TabIndex = 3;
			this.IDTextbox.TextChanged += new System.EventHandler(this.IDTextbox_TextChanged);
			// 
			// DataTextbox
			// 
			this.DataTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DataTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
			this.DataTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DataTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.DataTextbox.Location = new System.Drawing.Point(395, 16);
			this.DataTextbox.Name = "DataTextbox";
			this.DataTextbox.Size = new System.Drawing.Size(256, 20);
			this.DataTextbox.TabIndex = 4;
			this.DataTextbox.TextChanged += new System.EventHandler(this.DataTextbox_TextChanged);
			// 
			// DataTitle
			// 
			this.DataTitle.AutoSize = true;
			this.DataTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.DataTitle.Location = new System.Drawing.Point(395, 0);
			this.DataTitle.Name = "DataTitle";
			this.DataTitle.Size = new System.Drawing.Size(256, 13);
			this.DataTitle.TabIndex = 5;
			this.DataTitle.Text = "Data";
			// 
			// SendPacketBtn
			// 
			this.SendPacketBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SendPacketBtn.Location = new System.Drawing.Point(657, 16);
			this.SendPacketBtn.Name = "SendPacketBtn";
			this.SendPacketBtn.Padding = new System.Windows.Forms.Padding(5);
			this.SendPacketBtn.Size = new System.Drawing.Size(100, 25);
			this.SendPacketBtn.TabIndex = 5;
			this.SendPacketBtn.Text = "Send";
			this.SendPacketBtn.Click += new System.EventHandler(this.SendPacketBtn_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.UseCurrentTime, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.TimestampTextbox, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 13);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(196, 69);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// UseCurrentTime
			// 
			this.UseCurrentTime.AutoSize = true;
			this.UseCurrentTime.Location = new System.Drawing.Point(3, 29);
			this.UseCurrentTime.Name = "UseCurrentTime";
			this.UseCurrentTime.Size = new System.Drawing.Size(86, 17);
			this.UseCurrentTime.TabIndex = 1;
			this.UseCurrentTime.Text = "Current Time";
			this.UseCurrentTime.CheckedChanged += new System.EventHandler(this.UseCurrentTime_CheckedChanged);
			// 
			// TimestampTextbox
			// 
			this.TimestampTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TimestampTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
			this.TimestampTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TimestampTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.TimestampTextbox.Location = new System.Drawing.Point(3, 3);
			this.TimestampTextbox.Name = "TimestampTextbox";
			this.TimestampTextbox.Size = new System.Drawing.Size(190, 20);
			this.TimestampTextbox.TabIndex = 2;
			this.TimestampTextbox.TextChanged += new System.EventHandler(this.TimestampTextbox_TextChanged);
			// 
			// InterpretationTimestamp
			// 
			this.InterpretationTimestamp.AutoSize = true;
			this.InterpretationTimestamp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InterpretationTimestamp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.InterpretationTimestamp.Location = new System.Drawing.Point(3, 82);
			this.InterpretationTimestamp.Name = "InterpretationTimestamp";
			this.InterpretationTimestamp.Size = new System.Drawing.Size(190, 13);
			this.InterpretationTimestamp.TabIndex = 8;
			this.InterpretationTimestamp.Text = "Unknown";
			// 
			// InterpretationID
			// 
			this.InterpretationID.AutoSize = true;
			this.InterpretationID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InterpretationID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.InterpretationID.Location = new System.Drawing.Point(199, 82);
			this.InterpretationID.Name = "InterpretationID";
			this.InterpretationID.Size = new System.Drawing.Size(190, 13);
			this.InterpretationID.TabIndex = 9;
			this.InterpretationID.Text = "Unknown";
			// 
			// InterpretationData
			// 
			this.InterpretationData.AutoSize = true;
			this.InterpretationData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InterpretationData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.InterpretationData.Location = new System.Drawing.Point(395, 82);
			this.InterpretationData.Name = "InterpretationData";
			this.InterpretationData.Size = new System.Drawing.Size(256, 13);
			this.InterpretationData.TabIndex = 10;
			this.InterpretationData.Text = "Unknown";
			// 
			// PacketConstructStatus
			// 
			this.PacketConstructStatus.AutoSize = true;
			this.PacketConstructStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PacketConstructStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.PacketConstructStatus.Location = new System.Drawing.Point(657, 82);
			this.PacketConstructStatus.Name = "PacketConstructStatus";
			this.PacketConstructStatus.Size = new System.Drawing.Size(100, 13);
			this.PacketConstructStatus.TabIndex = 11;
			this.PacketConstructStatus.Text = "Unknown";
			// 
			// SecTimer
			// 
			this.SecTimer.Interval = 1000;
			this.SecTimer.Tick += new System.EventHandler(this.SecTimer_Tick);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(540, 358);
			this.Name = "MainWindow";
			this.Text = "Science!";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.Debugging.ResumeLayout(false);
			this.DebugList.ResumeLayout(false);
			this.PacketGenBox.ResumeLayout(false);
			this.PacketGenLayout.ResumeLayout(false);
			this.PacketGenLayout.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Control;
        private System.Windows.Forms.TabPage Debugging;
        private System.Windows.Forms.Button EmergencyStopBtn;
        private System.Windows.Forms.TableLayoutPanel DebugList;
        private System.Windows.Forms.GroupBox PacketGenBox;
        private System.Windows.Forms.TableLayoutPanel PacketGenLayout;
        private DarkUI.Controls.DarkLabel TimestampTitle;
        private DarkUI.Controls.DarkCheckBox UseCurrentTime;
        private DarkUI.Controls.DarkTextBox TimestampTextbox;
        private DarkUI.Controls.DarkLabel IDTitle;
        private DarkUI.Controls.DarkTextBox IDTextbox;
        private DarkUI.Controls.DarkTextBox DataTextbox;
        private DarkUI.Controls.DarkLabel DataTitle;
        private DarkUI.Controls.DarkButton SendPacketBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DarkUI.Controls.DarkLabel InterpretationTimestamp;
        private DarkUI.Controls.DarkLabel InterpretationID;
        private DarkUI.Controls.DarkLabel InterpretationData;
        private DarkUI.Controls.DarkLabel PacketConstructStatus;
        private System.Windows.Forms.Timer SecTimer;
    }
}