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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Control = new System.Windows.Forms.TabPage();
            this.Debugging = new System.Windows.Forms.TabPage();
            this.DebugList = new System.Windows.Forms.TableLayoutPanel();
            this.PacketGenBox = new System.Windows.Forms.GroupBox();
            this.PacketGenLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TimestampTitle = new System.Windows.Forms.Label();
            this.IDTitle = new System.Windows.Forms.Label();
            this.IDTextbox = new System.Windows.Forms.TextBox();
            this.DataTextbox = new System.Windows.Forms.TextBox();
            this.DataTitle = new System.Windows.Forms.Label();
            this.SendPacketBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.UseCurrentTime = new System.Windows.Forms.CheckBox();
            this.TimestampTextbox = new System.Windows.Forms.TextBox();
            this.InterpretationTimestamp = new System.Windows.Forms.Label();
            this.InterpretationID = new System.Windows.Forms.Label();
            this.InterpretationData = new System.Windows.Forms.Label();
            this.PacketConstructStatus = new System.Windows.Forms.Label();
            this.EmergencyStopBtn = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EmergencyStopBtn, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1045, 690);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Control);
            this.tabControl1.Controls.Add(this.Debugging);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1045, 647);
            this.tabControl1.TabIndex = 0;
            // 
            // Control
            // 
            this.Control.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Control.Location = new System.Drawing.Point(4, 25);
            this.Control.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(1037, 618);
            this.Control.TabIndex = 0;
            this.Control.Text = "Control";
            // 
            // Debugging
            // 
            this.Debugging.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Debugging.Controls.Add(this.DebugList);
            this.Debugging.Location = new System.Drawing.Point(4, 25);
            this.Debugging.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Debugging.Name = "Debugging";
            this.Debugging.Size = new System.Drawing.Size(1037, 618);
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
            this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.DebugList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.DebugList.Size = new System.Drawing.Size(1037, 618);
            this.DebugList.TabIndex = 0;
            // 
            // PacketGenBox
            // 
            this.PacketGenBox.Controls.Add(this.PacketGenLayout);
            this.PacketGenBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketGenBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PacketGenBox.Location = new System.Drawing.Point(0, 0);
            this.PacketGenBox.Margin = new System.Windows.Forms.Padding(0);
            this.PacketGenBox.MinimumSize = new System.Drawing.Size(0, 123);
            this.PacketGenBox.Name = "PacketGenBox";
            this.PacketGenBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PacketGenBox.Size = new System.Drawing.Size(1037, 154);
            this.PacketGenBox.TabIndex = 1;
            this.PacketGenBox.TabStop = false;
            this.PacketGenBox.Text = "Packet Builder";
            // 
            // PacketGenLayout
            // 
            this.PacketGenLayout.ColumnCount = 4;
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.94118F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
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
            this.PacketGenLayout.Location = new System.Drawing.Point(4, 19);
            this.PacketGenLayout.Margin = new System.Windows.Forms.Padding(0);
            this.PacketGenLayout.Name = "PacketGenLayout";
            this.PacketGenLayout.RowCount = 3;
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.PacketGenLayout.Size = new System.Drawing.Size(1029, 131);
            this.PacketGenLayout.TabIndex = 0;
            // 
            // TimestampTitle
            // 
            this.TimestampTitle.AutoSize = true;
            this.TimestampTitle.Location = new System.Drawing.Point(4, 0);
            this.TimestampTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TimestampTitle.Name = "TimestampTitle";
            this.TimestampTitle.Size = new System.Drawing.Size(77, 17);
            this.TimestampTitle.TabIndex = 0;
            this.TimestampTitle.Text = "Timestamp";
            // 
            // IDTitle
            // 
            this.IDTitle.AutoSize = true;
            this.IDTitle.Location = new System.Drawing.Point(215, 0);
            this.IDTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.IDTitle.Name = "IDTitle";
            this.IDTitle.Size = new System.Drawing.Size(21, 17);
            this.IDTitle.TabIndex = 2;
            this.IDTitle.Text = "ID";
            // 
            // IDTextbox
            // 
            this.IDTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IDTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.IDTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IDTextbox.Location = new System.Drawing.Point(215, 29);
            this.IDTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IDTextbox.Name = "IDTextbox";
            this.IDTextbox.Size = new System.Drawing.Size(203, 22);
            this.IDTextbox.TabIndex = 3;
            this.IDTextbox.TextChanged += new System.EventHandler(this.IDTextbox_TextChanged);
            // 
            // DataTextbox
            // 
            this.DataTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DataTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.DataTextbox.Location = new System.Drawing.Point(426, 29);
            this.DataTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DataTextbox.Name = "DataTextbox";
            this.DataTextbox.Size = new System.Drawing.Size(468, 22);
            this.DataTextbox.TabIndex = 4;
            this.DataTextbox.TextChanged += new System.EventHandler(this.DataTextbox_TextChanged);
            // 
            // DataTitle
            // 
            this.DataTitle.AutoSize = true;
            this.DataTitle.Location = new System.Drawing.Point(426, 0);
            this.DataTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DataTitle.Name = "DataTitle";
            this.DataTitle.Size = new System.Drawing.Size(38, 17);
            this.DataTitle.TabIndex = 5;
            this.DataTitle.Text = "Data";
            // 
            // SendPacketBtn
            // 
            this.SendPacketBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendPacketBtn.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.SendPacketBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SendPacketBtn.Location = new System.Drawing.Point(902, 29);
            this.SendPacketBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SendPacketBtn.Name = "SendPacketBtn";
            this.SendPacketBtn.Size = new System.Drawing.Size(123, 31);
            this.SendPacketBtn.TabIndex = 5;
            this.SendPacketBtn.Text = "Send";
            this.SendPacketBtn.UseVisualStyleBackColor = false;
            this.SendPacketBtn.Click += new System.EventHandler(this.SendPacketBtn_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.UseCurrentTime, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.TimestampTextbox, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(211, 81);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // UseCurrentTime
            // 
            this.UseCurrentTime.AutoSize = true;
            this.UseCurrentTime.Location = new System.Drawing.Point(4, 44);
            this.UseCurrentTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.UseCurrentTime.Name = "UseCurrentTime";
            this.UseCurrentTime.Size = new System.Drawing.Size(112, 21);
            this.UseCurrentTime.TabIndex = 1;
            this.UseCurrentTime.Text = "Current Time";
            this.UseCurrentTime.UseVisualStyleBackColor = true;
            this.UseCurrentTime.CheckedChanged += new System.EventHandler(this.UseCurrentTime_CheckedChanged);
            // 
            // TimestampTextbox
            // 
            this.TimestampTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TimestampTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TimestampTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TimestampTextbox.Location = new System.Drawing.Point(4, 4);
            this.TimestampTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TimestampTextbox.Name = "TimestampTextbox";
            this.TimestampTextbox.Size = new System.Drawing.Size(203, 22);
            this.TimestampTextbox.TabIndex = 2;
            this.TimestampTextbox.TextChanged += new System.EventHandler(this.TimestampTextbox_TextChanged);
            // 
            // InterpretationTimestamp
            // 
            this.InterpretationTimestamp.AutoSize = true;
            this.InterpretationTimestamp.Location = new System.Drawing.Point(4, 106);
            this.InterpretationTimestamp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InterpretationTimestamp.Name = "InterpretationTimestamp";
            this.InterpretationTimestamp.Size = new System.Drawing.Size(66, 17);
            this.InterpretationTimestamp.TabIndex = 8;
            this.InterpretationTimestamp.Text = "Unknown";
            // 
            // InterpretationID
            // 
            this.InterpretationID.AutoSize = true;
            this.InterpretationID.Location = new System.Drawing.Point(215, 106);
            this.InterpretationID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InterpretationID.Name = "InterpretationID";
            this.InterpretationID.Size = new System.Drawing.Size(66, 17);
            this.InterpretationID.TabIndex = 9;
            this.InterpretationID.Text = "Unknown";
            // 
            // InterpretationData
            // 
            this.InterpretationData.AutoSize = true;
            this.InterpretationData.Location = new System.Drawing.Point(426, 106);
            this.InterpretationData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InterpretationData.Name = "InterpretationData";
            this.InterpretationData.Size = new System.Drawing.Size(66, 17);
            this.InterpretationData.TabIndex = 10;
            this.InterpretationData.Text = "Unknown";
            // 
            // PacketConstructStatus
            // 
            this.PacketConstructStatus.AutoSize = true;
            this.PacketConstructStatus.Location = new System.Drawing.Point(902, 106);
            this.PacketConstructStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PacketConstructStatus.Name = "PacketConstructStatus";
            this.PacketConstructStatus.Size = new System.Drawing.Size(66, 17);
            this.PacketConstructStatus.TabIndex = 11;
            this.PacketConstructStatus.Text = "Unknown";
            // 
            // EmergencyStopBtn
            // 
            this.EmergencyStopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.EmergencyStopBtn.BackColor = System.Drawing.SystemColors.Control;
            this.EmergencyStopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmergencyStopBtn.ForeColor = System.Drawing.Color.Red;
            this.EmergencyStopBtn.Location = new System.Drawing.Point(430, 651);
            this.EmergencyStopBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EmergencyStopBtn.Name = "EmergencyStopBtn";
            this.EmergencyStopBtn.Size = new System.Drawing.Size(185, 35);
            this.EmergencyStopBtn.TabIndex = 999;
            this.EmergencyStopBtn.Text = "EMERGENCY STOP";
            this.EmergencyStopBtn.UseVisualStyleBackColor = false;
            this.EmergencyStopBtn.Click += new System.EventHandler(this.EmergencyStopClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(1045, 690);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(714, 432);
            this.Name = "MainWindow";
            this.Text = "Science!";
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
        private System.Windows.Forms.Label TimestampTitle;
        private System.Windows.Forms.CheckBox UseCurrentTime;
        private System.Windows.Forms.TextBox TimestampTextbox;
        private System.Windows.Forms.Label IDTitle;
        private System.Windows.Forms.TextBox IDTextbox;
        private System.Windows.Forms.TextBox DataTextbox;
        private System.Windows.Forms.Label DataTitle;
        private System.Windows.Forms.Button SendPacketBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label InterpretationTimestamp;
        private System.Windows.Forms.Label InterpretationID;
        private System.Windows.Forms.Label InterpretationData;
        private System.Windows.Forms.Label PacketConstructStatus;
    }
}