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
			this.button1 = new System.Windows.Forms.Button();
			this.DebugList = new System.Windows.Forms.TableLayoutPanel();
			this.PacketGenLayout = new System.Windows.Forms.TableLayoutPanel();
			this.PacketGenBox = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.Debugging.SuspendLayout();
			this.DebugList.SuspendLayout();
			this.PacketGenLayout.SuspendLayout();
			this.PacketGenBox.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 561);
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
			this.tabControl1.Size = new System.Drawing.Size(784, 526);
			this.tabControl1.TabIndex = 0;
			// 
			// Control
			// 
			this.Control.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.Control.Location = new System.Drawing.Point(4, 22);
			this.Control.Name = "Control";
			this.Control.Size = new System.Drawing.Size(776, 500);
			this.Control.TabIndex = 0;
			this.Control.Text = "Control";
			// 
			// Debugging
			// 
			this.Debugging.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.Debugging.Controls.Add(this.DebugList);
			this.Debugging.Location = new System.Drawing.Point(4, 22);
			this.Debugging.Name = "Debugging";
			this.Debugging.Size = new System.Drawing.Size(776, 500);
			this.Debugging.TabIndex = 1;
			this.Debugging.Text = "Debugging";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.Red;
			this.button1.Location = new System.Drawing.Point(322, 529);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(139, 29);
			this.button1.TabIndex = 1;
			this.button1.Text = "EMERGENCY STOP";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
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
			this.DebugList.Size = new System.Drawing.Size(776, 500);
			this.DebugList.TabIndex = 0;
			// 
			// PacketGenLayout
			// 
			this.PacketGenLayout.ColumnCount = 4;
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.94118F));
			this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.PacketGenLayout.Controls.Add(this.label1, 0, 0);
			this.PacketGenLayout.Controls.Add(this.label2, 1, 0);
			this.PacketGenLayout.Controls.Add(this.textBox2, 1, 1);
			this.PacketGenLayout.Controls.Add(this.textBox3, 2, 1);
			this.PacketGenLayout.Controls.Add(this.label3, 2, 0);
			this.PacketGenLayout.Controls.Add(this.button2, 3, 1);
			this.PacketGenLayout.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.PacketGenLayout.Controls.Add(this.label4, 0, 2);
			this.PacketGenLayout.Controls.Add(this.label5, 1, 2);
			this.PacketGenLayout.Controls.Add(this.label6, 2, 2);
			this.PacketGenLayout.Controls.Add(this.label7, 3, 2);
			this.PacketGenLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PacketGenLayout.Location = new System.Drawing.Point(3, 16);
			this.PacketGenLayout.Margin = new System.Windows.Forms.Padding(0);
			this.PacketGenLayout.Name = "PacketGenLayout";
			this.PacketGenLayout.RowCount = 3;
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.PacketGenLayout.Size = new System.Drawing.Size(770, 106);
			this.PacketGenLayout.TabIndex = 0;
			// 
			// PacketGenBox
			// 
			this.PacketGenBox.Controls.Add(this.PacketGenLayout);
			this.PacketGenBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PacketGenBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.PacketGenBox.Location = new System.Drawing.Point(0, 0);
			this.PacketGenBox.Margin = new System.Windows.Forms.Padding(0);
			this.PacketGenBox.MinimumSize = new System.Drawing.Size(0, 100);
			this.PacketGenBox.Name = "PacketGenBox";
			this.PacketGenBox.Size = new System.Drawing.Size(776, 125);
			this.PacketGenBox.TabIndex = 1;
			this.PacketGenBox.TabStop = false;
			this.PacketGenBox.Text = "Packet Builder";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Timestamp";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(3, 36);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(86, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Current Time";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.textBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.textBox1.Location = new System.Drawing.Point(3, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(153, 20);
			this.textBox1.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(162, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "ID";
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.textBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.textBox2.Location = new System.Drawing.Point(162, 23);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(153, 20);
			this.textBox2.TabIndex = 3;
			// 
			// textBox3
			// 
			this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.textBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.textBox3.Location = new System.Drawing.Point(321, 23);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(354, 20);
			this.textBox3.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(321, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Data";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.button2.Location = new System.Drawing.Point(681, 23);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(86, 25);
			this.button2.TabIndex = 6;
			this.button2.Text = "Send";
			this.button2.UseVisualStyleBackColor = false;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.checkBox1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(159, 66);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Unknown";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(162, 86);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Unknown";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(321, 86);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Unknown";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(681, 86);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 13);
			this.label7.TabIndex = 11;
			this.label7.Text = "Unknown";
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(540, 360);
			this.Name = "MainWindow";
			this.Text = "Science!";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.Debugging.ResumeLayout(false);
			this.DebugList.ResumeLayout(false);
			this.PacketGenLayout.ResumeLayout(false);
			this.PacketGenLayout.PerformLayout();
			this.PacketGenBox.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Control;
        private System.Windows.Forms.TabPage Debugging;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel DebugList;
        private System.Windows.Forms.GroupBox PacketGenBox;
        private System.Windows.Forms.TableLayoutPanel PacketGenLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}