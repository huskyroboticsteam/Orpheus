namespace TestArmControllerGUI
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.SendBtn = new System.Windows.Forms.Button();
            this.statusBox = new System.Windows.Forms.RichTextBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.packetLabel = new System.Windows.Forms.Label();
            this.devicecombobox = new System.Windows.Forms.ComboBox();
            this.packetcombobox = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.payloadtextbox = new System.Windows.Forms.TextBox();
            this.payloadLabel = new System.Windows.Forms.Label();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(12, 177);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(467, 76);
            this.SendBtn.TabIndex = 0;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // statusBox
            // 
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(486, 12);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(776, 1305);
            this.statusBox.TabIndex = 1;
            this.statusBox.Text = "";
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Location = new System.Drawing.Point(12, 16);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(78, 25);
            this.deviceLabel.TabIndex = 6;
            this.deviceLabel.Text = "Device";
            // 
            // packetLabel
            // 
            this.packetLabel.AutoSize = true;
            this.packetLabel.Location = new System.Drawing.Point(12, 56);
            this.packetLabel.Name = "packetLabel";
            this.packetLabel.Size = new System.Drawing.Size(78, 25);
            this.packetLabel.TabIndex = 7;
            this.packetLabel.Text = "Packet";
            // 
            // devicecombobox
            // 
            this.devicecombobox.FormattingEnabled = true;
            this.devicecombobox.Location = new System.Drawing.Point(154, 13);
            this.devicecombobox.Name = "devicecombobox";
            this.devicecombobox.Size = new System.Drawing.Size(326, 33);
            this.devicecombobox.TabIndex = 10;
            // 
            // packetcombobox
            // 
            this.packetcombobox.FormattingEnabled = true;
            this.packetcombobox.Location = new System.Drawing.Point(154, 53);
            this.packetcombobox.Name = "packetcombobox";
            this.packetcombobox.Size = new System.Drawing.Size(326, 33);
            this.packetcombobox.TabIndex = 11;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(316, 98);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(161, 29);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "High Priority";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // payloadtextbox
            // 
            this.payloadtextbox.Location = new System.Drawing.Point(153, 133);
            this.payloadtextbox.Name = "payloadtextbox";
            this.payloadtextbox.Size = new System.Drawing.Size(325, 31);
            this.payloadtextbox.TabIndex = 13;
            // 
            // payloadLabel
            // 
            this.payloadLabel.AutoSize = true;
            this.payloadLabel.Location = new System.Drawing.Point(12, 139);
            this.payloadLabel.Name = "payloadLabel";
            this.payloadLabel.Size = new System.Drawing.Size(90, 25);
            this.payloadLabel.TabIndex = 14;
            this.payloadLabel.Text = "Payload";
            // 
            // connectionLabel
            // 
            this.connectionLabel.AutoSize = true;
            this.connectionLabel.ForeColor = System.Drawing.Color.Red;
            this.connectionLabel.Location = new System.Drawing.Point(12, 256);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(194, 25);
            this.connectionLabel.TabIndex = 23;
            this.connectionLabel.Text = "NOT CONNECTED";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 1329);
            this.Controls.Add(this.connectionLabel);
            this.Controls.Add(this.payloadLabel);
            this.Controls.Add(this.payloadtextbox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.packetcombobox);
            this.Controls.Add(this.devicecombobox);
            this.Controls.Add(this.packetLabel);
            this.Controls.Add(this.deviceLabel);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.SendBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "CAN GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.RichTextBox statusBox;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label packetLabel;
        private System.Windows.Forms.ComboBox devicecombobox;
        private System.Windows.Forms.ComboBox packetcombobox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox payloadtextbox;
        private System.Windows.Forms.Label payloadLabel;
        private System.Windows.Forms.Label connectionLabel;
    }
}

