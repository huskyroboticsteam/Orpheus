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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Control = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.ChartLeft = new LiveCharts.WinForms.CartesianChart();
            this.ChartRight = new LiveCharts.WinForms.CartesianChart();
            this.GaugeTable = new System.Windows.Forms.TableLayoutPanel();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.LabSysVoltage = new DarkUI.Controls.DarkLabel();
            this.GaugeRailCurrent = new LiveCharts.WinForms.AngularGauge();
            this.GaugeDrillCurrent = new LiveCharts.WinForms.AngularGauge();
            this.GaugeSysCurrent = new LiveCharts.WinForms.AngularGauge();
            this.GaugeSysVoltage = new LiveCharts.WinForms.AngularGauge();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.darkLabel4 = new DarkUI.Controls.DarkLabel();
            this.darkLabel5 = new DarkUI.Controls.DarkLabel();
            this.SampleTubeStatus = new DarkUI.Controls.DarkLabel();
            this.SampleCupStatus = new DarkUI.Controls.DarkLabel();
            this.SampleTubeToggle = new DarkUI.Controls.DarkButton();
            this.SampleCupToggle = new DarkUI.Controls.DarkButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.DrillSpeed = new System.Windows.Forms.TrackBar();
            this.DrillToggle = new DarkUI.Controls.DarkButton();
            this.DrillReverse = new DarkUI.Controls.DarkCheckBox();
            this.darkLabel6 = new DarkUI.Controls.DarkLabel();
            this.darkLabel7 = new DarkUI.Controls.DarkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.RailGoCustomBottom = new DarkUI.Controls.DarkButton();
            this.RailSpeed = new System.Windows.Forms.TrackBar();
            this.darkLabel8 = new DarkUI.Controls.DarkLabel();
            this.DistTopMeasurementLabel = new DarkUI.Controls.DarkLabel();
            this.darkLabel10 = new DarkUI.Controls.DarkLabel();
            this.RailGoTop = new DarkUI.Controls.DarkButton();
            this.RailGoGround = new DarkUI.Controls.DarkButton();
            this.darkLabel11 = new DarkUI.Controls.DarkLabel();
            this.DistBottomMeasurementLabel = new DarkUI.Controls.DarkLabel();
            this.RailGoCustomTop = new DarkUI.Controls.DarkButton();
            this.RailDistEntry = new DarkUI.Controls.DarkTextBox();
            this.darkLabel13 = new DarkUI.Controls.DarkLabel();
            this.darkLabel14 = new DarkUI.Controls.DarkLabel();
            this.railDisplay1 = new Science_Base.RailDisplay();
            this.ChartClearRight = new DarkUI.Controls.DarkButton();
            this.ChartClearLeft = new DarkUI.Controls.DarkButton();
            this.ChartDataChooser = new System.Windows.Forms.ListView();
            this.ChartAddLeft = new DarkUI.Controls.DarkButton();
            this.ChartAddRight = new DarkUI.Controls.DarkButton();
            this.Debugging = new System.Windows.Forms.TabPage();
            this.DebugList = new System.Windows.Forms.TableLayoutPanel();
            this.PacketGenBox = new System.Windows.Forms.GroupBox();
            this.PacketGenLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TimestampTitle = new DarkUI.Controls.DarkLabel();
            this.IDTitle = new DarkUI.Controls.DarkLabel();
            this.IDTextbox = new DarkUI.Controls.DarkTextBox();
            this.DataTextbox = new DarkUI.Controls.DarkTextBox();
            this.DataTitle = new DarkUI.Controls.DarkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.UseCurrentTime = new DarkUI.Controls.DarkCheckBox();
            this.TimestampTextbox = new DarkUI.Controls.DarkTextBox();
            this.InterpretationTimestamp = new DarkUI.Controls.DarkLabel();
            this.InterpretationID = new DarkUI.Controls.DarkLabel();
            this.InterpretationData = new DarkUI.Controls.DarkLabel();
            this.PacketConstructStatus = new DarkUI.Controls.DarkLabel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.SendPacketBtn = new DarkUI.Controls.DarkButton();
            this.ClientSelector = new System.Windows.Forms.ComboBox();
            this.SendAsUDP = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.StatsPacketQueueOut = new DarkUI.Controls.DarkLabel();
            this.StatsPacketQueueIn = new DarkUI.Controls.DarkLabel();
            this.EmergencyStopBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.StatusImgNetwork = new System.Windows.Forms.PictureBox();
            this.StatusImgPower = new System.Windows.Forms.PictureBox();
            this.StatusImgSystem = new System.Windows.Forms.PictureBox();
            this.SecTimer = new System.Windows.Forms.Timer(this.components);
            this.UIUpdate = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Control.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.GaugeTable.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrillSpeed)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RailSpeed)).BeginInit();
            this.Debugging.SuspendLayout();
            this.DebugList.SuspendLayout();
            this.PacketGenBox.SuspendLayout();
            this.PacketGenLayout.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgNetwork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgSystem)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EmergencyStopBtn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 729);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.Control);
            this.tabControl1.Controls.Add(this.Debugging);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 690);
            this.tabControl1.TabIndex = 0;
            // 
            // Control
            // 
            this.Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Control.Controls.Add(this.tableLayoutPanel5);
            this.Control.Location = new System.Drawing.Point(4, 22);
            this.Control.Margin = new System.Windows.Forms.Padding(0);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(1000, 664);
            this.Control.TabIndex = 0;
            this.Control.Text = "Control";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.ChartLeft, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.ChartRight, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.GaugeTable, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1000, 664);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // ChartLeft
            // 
            this.ChartLeft.AllowDrop = true;
            this.ChartLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartLeft.Location = new System.Drawing.Point(3, 418);
            this.ChartLeft.Name = "ChartLeft";
            this.ChartLeft.Size = new System.Drawing.Size(494, 243);
            this.ChartLeft.TabIndex = 4;
            this.ChartLeft.Text = "DataGraph";
            // 
            // ChartRight
            // 
            this.ChartRight.AllowDrop = true;
            this.ChartRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartRight.Location = new System.Drawing.Point(503, 418);
            this.ChartRight.Name = "ChartRight";
            this.ChartRight.Size = new System.Drawing.Size(494, 243);
            this.ChartRight.TabIndex = 3;
            this.ChartRight.Text = "Graph2";
            // 
            // GaugeTable
            // 
            this.GaugeTable.ColumnCount = 2;
            this.GaugeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.Controls.Add(this.darkLabel3, 1, 2);
            this.GaugeTable.Controls.Add(this.darkLabel2, 1, 0);
            this.GaugeTable.Controls.Add(this.darkLabel1, 0, 2);
            this.GaugeTable.Controls.Add(this.LabSysVoltage, 0, 0);
            this.GaugeTable.Controls.Add(this.GaugeRailCurrent, 1, 3);
            this.GaugeTable.Controls.Add(this.GaugeDrillCurrent, 0, 3);
            this.GaugeTable.Controls.Add(this.GaugeSysCurrent, 1, 1);
            this.GaugeTable.Controls.Add(this.GaugeSysVoltage, 0, 1);
            this.GaugeTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeTable.Location = new System.Drawing.Point(500, 0);
            this.GaugeTable.Margin = new System.Windows.Forms.Padding(0);
            this.GaugeTable.Name = "GaugeTable";
            this.GaugeTable.RowCount = 4;
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.GaugeTable.Size = new System.Drawing.Size(500, 415);
            this.GaugeTable.TabIndex = 0;
            // 
            // darkLabel3
            // 
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel3.Location = new System.Drawing.Point(253, 207);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(244, 20);
            this.darkLabel3.TabIndex = 12;
            this.darkLabel3.Text = "Rail (A)";
            this.darkLabel3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(253, 0);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(244, 20);
            this.darkLabel2.TabIndex = 11;
            this.darkLabel2.Text = "System (A)";
            this.darkLabel2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(3, 207);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(244, 20);
            this.darkLabel1.TabIndex = 10;
            this.darkLabel1.Text = "Drill (A)";
            this.darkLabel1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // LabSysVoltage
            // 
            this.LabSysVoltage.AutoSize = true;
            this.LabSysVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabSysVoltage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.LabSysVoltage.Location = new System.Drawing.Point(3, 0);
            this.LabSysVoltage.Name = "LabSysVoltage";
            this.LabSysVoltage.Size = new System.Drawing.Size(244, 20);
            this.LabSysVoltage.TabIndex = 9;
            this.LabSysVoltage.Text = "Supply (V)";
            this.LabSysVoltage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // GaugeRailCurrent
            // 
            this.GaugeRailCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeRailCurrent.Location = new System.Drawing.Point(253, 230);
            this.GaugeRailCurrent.Name = "GaugeRailCurrent";
            this.GaugeRailCurrent.Size = new System.Drawing.Size(244, 182);
            this.GaugeRailCurrent.TabIndex = 7;
            this.GaugeRailCurrent.Text = "angularGauge1";
            // 
            // GaugeDrillCurrent
            // 
            this.GaugeDrillCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeDrillCurrent.Location = new System.Drawing.Point(3, 230);
            this.GaugeDrillCurrent.Name = "GaugeDrillCurrent";
            this.GaugeDrillCurrent.Size = new System.Drawing.Size(244, 182);
            this.GaugeDrillCurrent.TabIndex = 6;
            this.GaugeDrillCurrent.Text = "angularGauge1";
            // 
            // GaugeSysCurrent
            // 
            this.GaugeSysCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeSysCurrent.Location = new System.Drawing.Point(253, 23);
            this.GaugeSysCurrent.Name = "GaugeSysCurrent";
            this.GaugeSysCurrent.Size = new System.Drawing.Size(244, 181);
            this.GaugeSysCurrent.TabIndex = 5;
            this.GaugeSysCurrent.Text = "angularGauge1";
            // 
            // GaugeSysVoltage
            // 
            this.GaugeSysVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeSysVoltage.ForeColor = System.Drawing.Color.White;
            this.GaugeSysVoltage.Location = new System.Drawing.Point(3, 23);
            this.GaugeSysVoltage.Name = "GaugeSysVoltage";
            this.GaugeSysVoltage.Size = new System.Drawing.Size(244, 181);
            this.GaugeSysVoltage.TabIndex = 4;
            this.GaugeSysVoltage.Text = "angularGauge1";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.groupBox3, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.ChartClearRight, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.ChartClearLeft, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.ChartDataChooser, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.ChartAddLeft, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.ChartAddRight, 1, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 4;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(500, 415);
            this.tableLayoutPanel7.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.tableLayoutPanel7.SetColumnSpan(this.groupBox4, 2);
            this.groupBox4.Controls.Add(this.tableLayoutPanel10);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 103);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sample";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.darkLabel4, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.darkLabel5, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.SampleTubeStatus, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.SampleCupStatus, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.SampleTubeToggle, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.SampleCupToggle, 1, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(238, 84);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // darkLabel4
            // 
            this.darkLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel4.AutoSize = true;
            this.darkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel4.Location = new System.Drawing.Point(3, 0);
            this.darkLabel4.Name = "darkLabel4";
            this.darkLabel4.Size = new System.Drawing.Size(113, 13);
            this.darkLabel4.TabIndex = 0;
            this.darkLabel4.Text = "Tube";
            this.darkLabel4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // darkLabel5
            // 
            this.darkLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel5.AutoSize = true;
            this.darkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel5.Location = new System.Drawing.Point(122, 0);
            this.darkLabel5.Name = "darkLabel5";
            this.darkLabel5.Size = new System.Drawing.Size(113, 13);
            this.darkLabel5.TabIndex = 1;
            this.darkLabel5.Text = "Cup";
            this.darkLabel5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SampleTubeStatus
            // 
            this.SampleTubeStatus.AutoSize = true;
            this.SampleTubeStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SampleTubeStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SampleTubeStatus.Location = new System.Drawing.Point(3, 43);
            this.SampleTubeStatus.Name = "SampleTubeStatus";
            this.SampleTubeStatus.Size = new System.Drawing.Size(113, 41);
            this.SampleTubeStatus.TabIndex = 2;
            this.SampleTubeStatus.Text = "Unknown";
            // 
            // SampleCupStatus
            // 
            this.SampleCupStatus.AutoSize = true;
            this.SampleCupStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SampleCupStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SampleCupStatus.Location = new System.Drawing.Point(122, 43);
            this.SampleCupStatus.Name = "SampleCupStatus";
            this.SampleCupStatus.Size = new System.Drawing.Size(113, 41);
            this.SampleCupStatus.TabIndex = 3;
            this.SampleCupStatus.Text = "Unknown";
            // 
            // SampleTubeToggle
            // 
            this.SampleTubeToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SampleTubeToggle.Location = new System.Drawing.Point(2, 19);
            this.SampleTubeToggle.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.SampleTubeToggle.Name = "SampleTubeToggle";
            this.SampleTubeToggle.Padding = new System.Windows.Forms.Padding(5);
            this.SampleTubeToggle.Size = new System.Drawing.Size(115, 23);
            this.SampleTubeToggle.TabIndex = 4;
            this.SampleTubeToggle.Text = "Toggle";
            this.SampleTubeToggle.Click += new System.EventHandler(this.SampleTubeToggle_Click);
            // 
            // SampleCupToggle
            // 
            this.SampleCupToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SampleCupToggle.Location = new System.Drawing.Point(121, 19);
            this.SampleCupToggle.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.SampleCupToggle.Name = "SampleCupToggle";
            this.SampleCupToggle.Padding = new System.Windows.Forms.Padding(5);
            this.SampleCupToggle.Size = new System.Drawing.Size(115, 23);
            this.SampleCupToggle.TabIndex = 5;
            this.SampleCupToggle.Text = "Toggle";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(253, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 103);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Drill";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.DrillSpeed, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.DrillToggle, 1, 2);
            this.tableLayoutPanel8.Controls.Add(this.DrillReverse, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.darkLabel6, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.darkLabel7, 1, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 3;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(238, 84);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // DrillSpeed
            // 
            this.DrillSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.SetColumnSpan(this.DrillSpeed, 2);
            this.DrillSpeed.Location = new System.Drawing.Point(3, 3);
            this.DrillSpeed.Maximum = 100;
            this.DrillSpeed.Name = "DrillSpeed";
            this.DrillSpeed.Size = new System.Drawing.Size(232, 29);
            this.DrillSpeed.TabIndex = 0;
            this.DrillSpeed.TickFrequency = 10;
            this.DrillSpeed.ValueChanged += new System.EventHandler(this.DrillSpeed_ValueChanged);
            // 
            // DrillToggle
            // 
            this.DrillToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrillToggle.Location = new System.Drawing.Point(120, 60);
            this.DrillToggle.Margin = new System.Windows.Forms.Padding(1);
            this.DrillToggle.Name = "DrillToggle";
            this.DrillToggle.Padding = new System.Windows.Forms.Padding(5);
            this.DrillToggle.Size = new System.Drawing.Size(117, 23);
            this.DrillToggle.TabIndex = 1;
            this.DrillToggle.Text = "START";
            this.DrillToggle.Click += new System.EventHandler(this.DrillToggle_Click);
            // 
            // DrillReverse
            // 
            this.DrillReverse.AutoSize = true;
            this.DrillReverse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrillReverse.Location = new System.Drawing.Point(3, 62);
            this.DrillReverse.Name = "DrillReverse";
            this.DrillReverse.Size = new System.Drawing.Size(113, 19);
            this.DrillReverse.TabIndex = 2;
            this.DrillReverse.Text = "Reverse";
            // 
            // darkLabel6
            // 
            this.darkLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel6.AutoSize = true;
            this.darkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel6.Location = new System.Drawing.Point(3, 35);
            this.darkLabel6.Name = "darkLabel6";
            this.darkLabel6.Size = new System.Drawing.Size(113, 13);
            this.darkLabel6.TabIndex = 3;
            this.darkLabel6.Text = "0 RPM";
            // 
            // darkLabel7
            // 
            this.darkLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel7.AutoSize = true;
            this.darkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel7.Location = new System.Drawing.Point(122, 35);
            this.darkLabel7.Name = "darkLabel7";
            this.darkLabel7.Size = new System.Drawing.Size(113, 13);
            this.darkLabel7.TabIndex = 4;
            this.darkLabel7.Text = "55 RPM";
            this.darkLabel7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel9);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(253, 112);
            this.groupBox3.Name = "groupBox3";
            this.tableLayoutPanel7.SetRowSpan(this.groupBox3, 3);
            this.groupBox3.Size = new System.Drawing.Size(244, 300);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rail";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel9.Controls.Add(this.RailGoCustomBottom, 2, 9);
            this.tableLayoutPanel9.Controls.Add(this.RailSpeed, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.darkLabel8, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.DistTopMeasurementLabel, 1, 3);
            this.tableLayoutPanel9.Controls.Add(this.darkLabel10, 1, 5);
            this.tableLayoutPanel9.Controls.Add(this.RailGoTop, 1, 6);
            this.tableLayoutPanel9.Controls.Add(this.RailGoGround, 2, 6);
            this.tableLayoutPanel9.Controls.Add(this.darkLabel11, 1, 11);
            this.tableLayoutPanel9.Controls.Add(this.DistBottomMeasurementLabel, 1, 12);
            this.tableLayoutPanel9.Controls.Add(this.RailGoCustomTop, 1, 9);
            this.tableLayoutPanel9.Controls.Add(this.RailDistEntry, 1, 8);
            this.tableLayoutPanel9.Controls.Add(this.darkLabel13, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.darkLabel14, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.railDisplay1, 0, 2);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 13;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(238, 281);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // RailGoCustomBottom
            // 
            this.RailGoCustomBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailGoCustomBottom.Location = new System.Drawing.Point(167, 192);
            this.RailGoCustomBottom.Margin = new System.Windows.Forms.Padding(1);
            this.RailGoCustomBottom.Name = "RailGoCustomBottom";
            this.RailGoCustomBottom.Padding = new System.Windows.Forms.Padding(5);
            this.RailGoCustomBottom.Size = new System.Drawing.Size(70, 23);
            this.RailGoCustomBottom.TabIndex = 13;
            this.RailGoCustomBottom.Text = "From Gnd";
            this.RailGoCustomBottom.Click += new System.EventHandler(this.RailGoCustomBottom_Click);
            // 
            // RailSpeed
            // 
            this.tableLayoutPanel9.SetColumnSpan(this.RailSpeed, 3);
            this.RailSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailSpeed.LargeChange = 10;
            this.RailSpeed.Location = new System.Drawing.Point(3, 3);
            this.RailSpeed.Maximum = 100;
            this.RailSpeed.Name = "RailSpeed";
            this.RailSpeed.Size = new System.Drawing.Size(232, 29);
            this.RailSpeed.SmallChange = 5;
            this.RailSpeed.TabIndex = 0;
            this.RailSpeed.TickFrequency = 5;
            this.RailSpeed.Value = 30;
            this.RailSpeed.ValueChanged += new System.EventHandler(this.RailSpeed_ValueChanged);
            // 
            // darkLabel8
            // 
            this.darkLabel8.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.darkLabel8, 2);
            this.darkLabel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel8.Location = new System.Drawing.Point(98, 53);
            this.darkLabel8.Name = "darkLabel8";
            this.darkLabel8.Size = new System.Drawing.Size(137, 18);
            this.darkLabel8.TabIndex = 1;
            this.darkLabel8.Text = "Distance from top:";
            this.darkLabel8.MouseEnter += new System.EventHandler(this.TopDepth_MouseEnter);
            this.darkLabel8.MouseLeave += new System.EventHandler(this.TopDepth_MouseLeave);
            // 
            // DistTopMeasurementLabel
            // 
            this.DistTopMeasurementLabel.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.DistTopMeasurementLabel, 2);
            this.DistTopMeasurementLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DistTopMeasurementLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DistTopMeasurementLabel.Location = new System.Drawing.Point(98, 71);
            this.DistTopMeasurementLabel.Name = "DistTopMeasurementLabel";
            this.DistTopMeasurementLabel.Size = new System.Drawing.Size(137, 18);
            this.DistTopMeasurementLabel.TabIndex = 2;
            this.DistTopMeasurementLabel.Text = "?? mm";
            this.DistTopMeasurementLabel.MouseEnter += new System.EventHandler(this.TopDepth_MouseEnter);
            this.DistTopMeasurementLabel.MouseLeave += new System.EventHandler(this.TopDepth_MouseLeave);
            // 
            // darkLabel10
            // 
            this.darkLabel10.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.darkLabel10, 2);
            this.darkLabel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel10.Location = new System.Drawing.Point(98, 117);
            this.darkLabel10.Name = "darkLabel10";
            this.darkLabel10.Size = new System.Drawing.Size(137, 18);
            this.darkLabel10.TabIndex = 3;
            this.darkLabel10.Text = "Go To:";
            // 
            // RailGoTop
            // 
            this.RailGoTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailGoTop.Location = new System.Drawing.Point(96, 136);
            this.RailGoTop.Margin = new System.Windows.Forms.Padding(1);
            this.RailGoTop.Name = "RailGoTop";
            this.RailGoTop.Padding = new System.Windows.Forms.Padding(5);
            this.RailGoTop.Size = new System.Drawing.Size(69, 23);
            this.RailGoTop.TabIndex = 4;
            this.RailGoTop.Text = "Top";
            this.RailGoTop.Click += new System.EventHandler(this.GoToTop_Click);
            // 
            // RailGoGround
            // 
            this.RailGoGround.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailGoGround.Location = new System.Drawing.Point(167, 136);
            this.RailGoGround.Margin = new System.Windows.Forms.Padding(1);
            this.RailGoGround.Name = "RailGoGround";
            this.RailGoGround.Padding = new System.Windows.Forms.Padding(5);
            this.RailGoGround.Size = new System.Drawing.Size(70, 23);
            this.RailGoGround.TabIndex = 5;
            this.RailGoGround.Text = "Ground";
            this.RailGoGround.Click += new System.EventHandler(this.GoToGround_Click);
            // 
            // darkLabel11
            // 
            this.darkLabel11.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.darkLabel11, 2);
            this.darkLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel11.Location = new System.Drawing.Point(98, 244);
            this.darkLabel11.Name = "darkLabel11";
            this.darkLabel11.Size = new System.Drawing.Size(137, 18);
            this.darkLabel11.TabIndex = 6;
            this.darkLabel11.Text = "Distance above ground:";
            this.darkLabel11.MouseEnter += new System.EventHandler(this.BottomDepth_MouseEnter);
            this.darkLabel11.MouseLeave += new System.EventHandler(this.BottomDepth_MouseLeave);
            // 
            // DistBottomMeasurementLabel
            // 
            this.DistBottomMeasurementLabel.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.DistBottomMeasurementLabel, 2);
            this.DistBottomMeasurementLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DistBottomMeasurementLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DistBottomMeasurementLabel.Location = new System.Drawing.Point(98, 262);
            this.DistBottomMeasurementLabel.Name = "DistBottomMeasurementLabel";
            this.DistBottomMeasurementLabel.Size = new System.Drawing.Size(137, 19);
            this.DistBottomMeasurementLabel.TabIndex = 7;
            this.DistBottomMeasurementLabel.Text = "?? mm";
            this.DistBottomMeasurementLabel.MouseEnter += new System.EventHandler(this.BottomDepth_MouseEnter);
            this.DistBottomMeasurementLabel.MouseLeave += new System.EventHandler(this.BottomDepth_MouseLeave);
            // 
            // RailGoCustomTop
            // 
            this.RailGoCustomTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailGoCustomTop.Location = new System.Drawing.Point(96, 192);
            this.RailGoCustomTop.Margin = new System.Windows.Forms.Padding(1);
            this.RailGoCustomTop.Name = "RailGoCustomTop";
            this.RailGoCustomTop.Padding = new System.Windows.Forms.Padding(5);
            this.RailGoCustomTop.Size = new System.Drawing.Size(69, 23);
            this.RailGoCustomTop.TabIndex = 8;
            this.RailGoCustomTop.Text = "From Top";
            this.RailGoCustomTop.Click += new System.EventHandler(this.RailGoCustom_Click);
            // 
            // RailDistEntry
            // 
            this.RailDistEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.RailDistEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel9.SetColumnSpan(this.RailDistEntry, 2);
            this.RailDistEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RailDistEntry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.RailDistEntry.Location = new System.Drawing.Point(96, 170);
            this.RailDistEntry.Margin = new System.Windows.Forms.Padding(1);
            this.RailDistEntry.Name = "RailDistEntry";
            this.RailDistEntry.Size = new System.Drawing.Size(141, 20);
            this.RailDistEntry.TabIndex = 9;
            this.RailDistEntry.TextChanged += new System.EventHandler(this.RailDistEntry_TextChanged);
            // 
            // darkLabel13
            // 
            this.darkLabel13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel13.AutoSize = true;
            this.darkLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel13.Location = new System.Drawing.Point(3, 35);
            this.darkLabel13.Name = "darkLabel13";
            this.darkLabel13.Size = new System.Drawing.Size(89, 13);
            this.darkLabel13.TabIndex = 10;
            this.darkLabel13.Text = "0 mm/s";
            // 
            // darkLabel14
            // 
            this.darkLabel14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkLabel14.AutoSize = true;
            this.tableLayoutPanel9.SetColumnSpan(this.darkLabel14, 2);
            this.darkLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel14.Location = new System.Drawing.Point(98, 35);
            this.darkLabel14.Name = "darkLabel14";
            this.darkLabel14.Size = new System.Drawing.Size(137, 13);
            this.darkLabel14.TabIndex = 11;
            this.darkLabel14.Text = "?? mm/s";
            this.darkLabel14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // railDisplay1
            // 
            this.railDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.railDisplay1.DrillLocation = 75;
            this.railDisplay1.ForeColor = System.Drawing.Color.Gainsboro;
            this.railDisplay1.InitStatus = ((byte)(0));
            this.railDisplay1.Location = new System.Drawing.Point(3, 56);
            this.railDisplay1.Name = "railDisplay1";
            this.tableLayoutPanel9.SetRowSpan(this.railDisplay1, 11);
            this.railDisplay1.ShowDistanceBottom = false;
            this.railDisplay1.ShowDistanceTop = false;
            this.railDisplay1.Size = new System.Drawing.Size(89, 222);
            this.railDisplay1.TabIndex = 12;
            this.railDisplay1.Text = "railDisplay1";
            // 
            // ChartClearRight
            // 
            this.ChartClearRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartClearRight.Location = new System.Drawing.Point(127, 390);
            this.ChartClearRight.Margin = new System.Windows.Forms.Padding(2, 1, 5, 1);
            this.ChartClearRight.Name = "ChartClearRight";
            this.ChartClearRight.Padding = new System.Windows.Forms.Padding(5);
            this.ChartClearRight.Size = new System.Drawing.Size(118, 24);
            this.ChartClearRight.TabIndex = 7;
            this.ChartClearRight.Text = "Clear Right";
            this.ChartClearRight.Click += new System.EventHandler(this.ChartClearRight_Click);
            // 
            // ChartClearLeft
            // 
            this.ChartClearLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartClearLeft.Location = new System.Drawing.Point(5, 390);
            this.ChartClearLeft.Margin = new System.Windows.Forms.Padding(5, 1, 2, 1);
            this.ChartClearLeft.Name = "ChartClearLeft";
            this.ChartClearLeft.Padding = new System.Windows.Forms.Padding(5);
            this.ChartClearLeft.Size = new System.Drawing.Size(118, 24);
            this.ChartClearLeft.TabIndex = 8;
            this.ChartClearLeft.Text = "Clear Left";
            this.ChartClearLeft.Click += new System.EventHandler(this.ChartClearLeft_Click);
            // 
            // ChartDataChooser
            // 
            this.ChartDataChooser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tableLayoutPanel7.SetColumnSpan(this.ChartDataChooser, 2);
            this.ChartDataChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartDataChooser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ChartDataChooser.Location = new System.Drawing.Point(3, 118);
            this.ChartDataChooser.Margin = new System.Windows.Forms.Padding(3, 9, 3, 1);
            this.ChartDataChooser.Name = "ChartDataChooser";
            this.ChartDataChooser.Size = new System.Drawing.Size(244, 245);
            this.ChartDataChooser.TabIndex = 10;
            this.ChartDataChooser.UseCompatibleStateImageBehavior = false;
            this.ChartDataChooser.View = System.Windows.Forms.View.List;
            // 
            // ChartAddLeft
            // 
            this.ChartAddLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartAddLeft.Location = new System.Drawing.Point(5, 365);
            this.ChartAddLeft.Margin = new System.Windows.Forms.Padding(5, 1, 2, 1);
            this.ChartAddLeft.Name = "ChartAddLeft";
            this.ChartAddLeft.Padding = new System.Windows.Forms.Padding(5);
            this.ChartAddLeft.Size = new System.Drawing.Size(118, 23);
            this.ChartAddLeft.TabIndex = 11;
            this.ChartAddLeft.Text = "Add Left";
            this.ChartAddLeft.Click += new System.EventHandler(this.ChartAddLeft_Click);
            // 
            // ChartAddRight
            // 
            this.ChartAddRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartAddRight.Location = new System.Drawing.Point(127, 365);
            this.ChartAddRight.Margin = new System.Windows.Forms.Padding(2, 1, 5, 1);
            this.ChartAddRight.Name = "ChartAddRight";
            this.ChartAddRight.Padding = new System.Windows.Forms.Padding(5);
            this.ChartAddRight.Size = new System.Drawing.Size(118, 23);
            this.ChartAddRight.TabIndex = 12;
            this.ChartAddRight.Text = "Add Right";
            this.ChartAddRight.Click += new System.EventHandler(this.ChartAddRight_Click);
            // 
            // Debugging
            // 
            this.Debugging.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Debugging.Controls.Add(this.DebugList);
            this.Debugging.Location = new System.Drawing.Point(4, 22);
            this.Debugging.Margin = new System.Windows.Forms.Padding(0);
            this.Debugging.Name = "Debugging";
            this.Debugging.Size = new System.Drawing.Size(1000, 664);
            this.Debugging.TabIndex = 1;
            this.Debugging.Text = "Debugging";
            // 
            // DebugList
            // 
            this.DebugList.ColumnCount = 1;
            this.DebugList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DebugList.Controls.Add(this.PacketGenBox, 0, 0);
            this.DebugList.Controls.Add(this.groupBox1, 0, 1);
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
            this.DebugList.Size = new System.Drawing.Size(1000, 664);
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
            this.PacketGenBox.Size = new System.Drawing.Size(990, 156);
            this.PacketGenBox.TabIndex = 1;
            this.PacketGenBox.TabStop = false;
            this.PacketGenBox.Text = "Packet Builder";
            // 
            // PacketGenLayout
            // 
            this.PacketGenLayout.ColumnCount = 4;
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.PacketGenLayout.Controls.Add(this.TimestampTitle, 0, 0);
            this.PacketGenLayout.Controls.Add(this.IDTitle, 1, 0);
            this.PacketGenLayout.Controls.Add(this.IDTextbox, 1, 1);
            this.PacketGenLayout.Controls.Add(this.DataTextbox, 2, 1);
            this.PacketGenLayout.Controls.Add(this.DataTitle, 2, 0);
            this.PacketGenLayout.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.PacketGenLayout.Controls.Add(this.InterpretationTimestamp, 0, 2);
            this.PacketGenLayout.Controls.Add(this.InterpretationID, 1, 2);
            this.PacketGenLayout.Controls.Add(this.InterpretationData, 2, 2);
            this.PacketGenLayout.Controls.Add(this.PacketConstructStatus, 3, 2);
            this.PacketGenLayout.Controls.Add(this.tableLayoutPanel3, 3, 1);
            this.PacketGenLayout.Controls.Add(this.SendAsUDP, 3, 0);
            this.PacketGenLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketGenLayout.Location = new System.Drawing.Point(3, 16);
            this.PacketGenLayout.Margin = new System.Windows.Forms.Padding(0);
            this.PacketGenLayout.Name = "PacketGenLayout";
            this.PacketGenLayout.RowCount = 3;
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PacketGenLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PacketGenLayout.Size = new System.Drawing.Size(984, 137);
            this.PacketGenLayout.TabIndex = 0;
            // 
            // TimestampTitle
            // 
            this.TimestampTitle.AutoSize = true;
            this.TimestampTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimestampTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TimestampTitle.Location = new System.Drawing.Point(3, 0);
            this.TimestampTitle.Name = "TimestampTitle";
            this.TimestampTitle.Size = new System.Drawing.Size(158, 17);
            this.TimestampTitle.TabIndex = 0;
            this.TimestampTitle.Text = "Timestamp";
            // 
            // IDTitle
            // 
            this.IDTitle.AutoSize = true;
            this.IDTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IDTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.IDTitle.Location = new System.Drawing.Point(167, 0);
            this.IDTitle.Name = "IDTitle";
            this.IDTitle.Size = new System.Drawing.Size(117, 17);
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
            this.IDTextbox.Location = new System.Drawing.Point(167, 20);
            this.IDTextbox.Name = "IDTextbox";
            this.IDTextbox.Size = new System.Drawing.Size(117, 20);
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
            this.DataTextbox.Location = new System.Drawing.Point(290, 20);
            this.DataTextbox.Name = "DataTextbox";
            this.DataTextbox.Size = new System.Drawing.Size(527, 20);
            this.DataTextbox.TabIndex = 4;
            this.DataTextbox.TextChanged += new System.EventHandler(this.DataTextbox_TextChanged);
            // 
            // DataTitle
            // 
            this.DataTitle.AutoSize = true;
            this.DataTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DataTitle.Location = new System.Drawing.Point(290, 0);
            this.DataTitle.Name = "DataTitle";
            this.DataTitle.Size = new System.Drawing.Size(527, 17);
            this.DataTitle.TabIndex = 5;
            this.DataTitle.Text = "Data";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.UseCurrentTime, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.TimestampTextbox, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 17);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(164, 107);
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
            this.TimestampTextbox.Size = new System.Drawing.Size(158, 20);
            this.TimestampTextbox.TabIndex = 2;
            this.TimestampTextbox.TextChanged += new System.EventHandler(this.TimestampTextbox_TextChanged);
            // 
            // InterpretationTimestamp
            // 
            this.InterpretationTimestamp.AutoSize = true;
            this.InterpretationTimestamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterpretationTimestamp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InterpretationTimestamp.Location = new System.Drawing.Point(3, 124);
            this.InterpretationTimestamp.Name = "InterpretationTimestamp";
            this.InterpretationTimestamp.Size = new System.Drawing.Size(158, 13);
            this.InterpretationTimestamp.TabIndex = 8;
            this.InterpretationTimestamp.Text = "Unknown";
            // 
            // InterpretationID
            // 
            this.InterpretationID.AutoSize = true;
            this.InterpretationID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterpretationID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InterpretationID.Location = new System.Drawing.Point(167, 124);
            this.InterpretationID.Name = "InterpretationID";
            this.InterpretationID.Size = new System.Drawing.Size(117, 13);
            this.InterpretationID.TabIndex = 9;
            this.InterpretationID.Text = "Unknown";
            // 
            // InterpretationData
            // 
            this.InterpretationData.AutoSize = true;
            this.InterpretationData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterpretationData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InterpretationData.Location = new System.Drawing.Point(290, 124);
            this.InterpretationData.Name = "InterpretationData";
            this.InterpretationData.Size = new System.Drawing.Size(527, 13);
            this.InterpretationData.TabIndex = 10;
            this.InterpretationData.Text = "Unknown";
            // 
            // PacketConstructStatus
            // 
            this.PacketConstructStatus.AutoSize = true;
            this.PacketConstructStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketConstructStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.PacketConstructStatus.Location = new System.Drawing.Point(823, 124);
            this.PacketConstructStatus.Name = "PacketConstructStatus";
            this.PacketConstructStatus.Size = new System.Drawing.Size(158, 13);
            this.PacketConstructStatus.TabIndex = 11;
            this.PacketConstructStatus.Text = "Unknown";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.SendPacketBtn, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ClientSelector, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(820, 17);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(164, 107);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // SendPacketBtn
            // 
            this.SendPacketBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendPacketBtn.Location = new System.Drawing.Point(3, 56);
            this.SendPacketBtn.Name = "SendPacketBtn";
            this.SendPacketBtn.Padding = new System.Windows.Forms.Padding(5);
            this.SendPacketBtn.Size = new System.Drawing.Size(158, 25);
            this.SendPacketBtn.TabIndex = 5;
            this.SendPacketBtn.Text = "Send";
            this.SendPacketBtn.Click += new System.EventHandler(this.SendPacketBtn_Click);
            // 
            // ClientSelector
            // 
            this.ClientSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientSelector.FormattingEnabled = true;
            this.ClientSelector.Location = new System.Drawing.Point(3, 3);
            this.ClientSelector.Name = "ClientSelector";
            this.ClientSelector.Size = new System.Drawing.Size(158, 21);
            this.ClientSelector.TabIndex = 6;
            // 
            // SendAsUDP
            // 
            this.SendAsUDP.AutoSize = true;
            this.SendAsUDP.Checked = true;
            this.SendAsUDP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SendAsUDP.Location = new System.Drawing.Point(823, 0);
            this.SendAsUDP.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.SendAsUDP.Name = "SendAsUDP";
            this.SendAsUDP.Size = new System.Drawing.Size(49, 17);
            this.SendAsUDP.TabIndex = 13;
            this.SendAsUDP.Text = "UDP";
            this.SendAsUDP.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Location = new System.Drawing.Point(5, 171);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(990, 156);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Base Status";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.StatsPacketQueueOut, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.StatsPacketQueueIn, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(984, 137);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // StatsPacketQueueOut
            // 
            this.StatsPacketQueueOut.AutoSize = true;
            this.StatsPacketQueueOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatsPacketQueueOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.StatsPacketQueueOut.Location = new System.Drawing.Point(3, 0);
            this.StatsPacketQueueOut.Name = "StatsPacketQueueOut";
            this.StatsPacketQueueOut.Size = new System.Drawing.Size(240, 45);
            this.StatsPacketQueueOut.TabIndex = 0;
            this.StatsPacketQueueOut.Text = "Packet Queue Out: N/A";
            this.StatsPacketQueueOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatsPacketQueueIn
            // 
            this.StatsPacketQueueIn.AutoSize = true;
            this.StatsPacketQueueIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatsPacketQueueIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.StatsPacketQueueIn.Location = new System.Drawing.Point(3, 45);
            this.StatsPacketQueueIn.Name = "StatsPacketQueueIn";
            this.StatsPacketQueueIn.Size = new System.Drawing.Size(240, 45);
            this.StatsPacketQueueIn.TabIndex = 1;
            this.StatsPacketQueueIn.Text = "Packet Queue In: N/A";
            this.StatsPacketQueueIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmergencyStopBtn
            // 
            this.EmergencyStopBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
            this.EmergencyStopBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmergencyStopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmergencyStopBtn.ForeColor = System.Drawing.Color.White;
            this.EmergencyStopBtn.Location = new System.Drawing.Point(432, 693);
            this.EmergencyStopBtn.Name = "EmergencyStopBtn";
            this.EmergencyStopBtn.Padding = new System.Windows.Forms.Padding(5);
            this.EmergencyStopBtn.Size = new System.Drawing.Size(144, 33);
            this.EmergencyStopBtn.TabIndex = 999;
            this.EmergencyStopBtn.Text = "EMERGENCY STOP";
            this.EmergencyStopBtn.UseVisualStyleBackColor = false;
            this.EmergencyStopBtn.Click += new System.EventHandler(this.EmergencyStopClick);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.StatusImgNetwork, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.StatusImgPower, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.StatusImgSystem, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(2, 692);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(425, 35);
            this.tableLayoutPanel6.TabIndex = 1000;
            // 
            // StatusImgNetwork
            // 
            this.StatusImgNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusImgNetwork.Image = global::Science_Base.Properties.Resources.Network;
            this.StatusImgNetwork.Location = new System.Drawing.Point(1, 1);
            this.StatusImgNetwork.Margin = new System.Windows.Forms.Padding(1);
            this.StatusImgNetwork.Name = "StatusImgNetwork";
            this.StatusImgNetwork.Size = new System.Drawing.Size(38, 33);
            this.StatusImgNetwork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StatusImgNetwork.TabIndex = 0;
            this.StatusImgNetwork.TabStop = false;
            // 
            // StatusImgPower
            // 
            this.StatusImgPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusImgPower.Image = global::Science_Base.Properties.Resources.Power;
            this.StatusImgPower.Location = new System.Drawing.Point(41, 1);
            this.StatusImgPower.Margin = new System.Windows.Forms.Padding(1);
            this.StatusImgPower.Name = "StatusImgPower";
            this.StatusImgPower.Size = new System.Drawing.Size(38, 33);
            this.StatusImgPower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StatusImgPower.TabIndex = 1;
            this.StatusImgPower.TabStop = false;
            // 
            // StatusImgSystem
            // 
            this.StatusImgSystem.Image = global::Science_Base.Properties.Resources.CPU;
            this.StatusImgSystem.Location = new System.Drawing.Point(81, 1);
            this.StatusImgSystem.Margin = new System.Windows.Forms.Padding(1);
            this.StatusImgSystem.Name = "StatusImgSystem";
            this.StatusImgSystem.Size = new System.Drawing.Size(38, 33);
            this.StatusImgSystem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StatusImgSystem.TabIndex = 2;
            this.StatusImgSystem.TabStop = false;
            // 
            // SecTimer
            // 
            this.SecTimer.Interval = 1000;
            this.SecTimer.Tick += new System.EventHandler(this.SecTimer_Tick);
            // 
            // UIUpdate
            // 
            this.UIUpdate.Interval = 33;
            this.UIUpdate.Tick += new System.EventHandler(this.UIUpdate_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(540, 358);
            this.Name = "MainWindow";
            this.Text = "Husky Robotics 2017-18 Science Station Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Control.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.GaugeTable.ResumeLayout(false);
            this.GaugeTable.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrillSpeed)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RailSpeed)).EndInit();
            this.Debugging.ResumeLayout(false);
            this.DebugList.ResumeLayout(false);
            this.PacketGenBox.ResumeLayout(false);
            this.PacketGenLayout.ResumeLayout(false);
            this.PacketGenLayout.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgNetwork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusImgSystem)).EndInit();
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
        private System.Windows.Forms.Timer UIUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private DarkUI.Controls.DarkLabel StatsPacketQueueOut;
        private DarkUI.Controls.DarkLabel StatsPacketQueueIn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox ClientSelector;
        private System.Windows.Forms.CheckBox SendAsUDP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel GaugeTable;
        private LiveCharts.WinForms.AngularGauge GaugeRailCurrent;
        private LiveCharts.WinForms.AngularGauge GaugeDrillCurrent;
        private LiveCharts.WinForms.AngularGauge GaugeSysCurrent;
        private LiveCharts.WinForms.AngularGauge GaugeSysVoltage;
        private DarkUI.Controls.DarkLabel darkLabel3;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel LabSysVoltage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.PictureBox StatusImgNetwork;
        private System.Windows.Forms.PictureBox StatusImgPower;
        private System.Windows.Forms.PictureBox StatusImgSystem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.GroupBox groupBox3;
        private LiveCharts.WinForms.CartesianChart ChartRight;
        private LiveCharts.WinForms.CartesianChart ChartLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TrackBar RailSpeed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TrackBar DrillSpeed;
        private DarkUI.Controls.DarkButton DrillToggle;
        private DarkUI.Controls.DarkCheckBox DrillReverse;
        private DarkUI.Controls.DarkButton ChartClearRight;
        private DarkUI.Controls.DarkButton ChartClearLeft;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView ChartDataChooser;
        private DarkUI.Controls.DarkButton ChartAddLeft;
        private DarkUI.Controls.DarkButton ChartAddRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private DarkUI.Controls.DarkLabel darkLabel4;
        private DarkUI.Controls.DarkLabel darkLabel5;
        private DarkUI.Controls.DarkLabel SampleTubeStatus;
        private DarkUI.Controls.DarkButton SampleTubeToggle;
        private DarkUI.Controls.DarkButton SampleCupToggle;
        private DarkUI.Controls.DarkLabel SampleCupStatus;
        private DarkUI.Controls.DarkLabel darkLabel6;
        private DarkUI.Controls.DarkLabel darkLabel7;
        private DarkUI.Controls.DarkLabel darkLabel8;
        private DarkUI.Controls.DarkLabel DistTopMeasurementLabel;
        private DarkUI.Controls.DarkLabel darkLabel10;
        private DarkUI.Controls.DarkButton RailGoTop;
        private DarkUI.Controls.DarkButton RailGoGround;
        private DarkUI.Controls.DarkLabel darkLabel11;
        private DarkUI.Controls.DarkLabel DistBottomMeasurementLabel;
        private DarkUI.Controls.DarkTextBox RailDistEntry;
        private DarkUI.Controls.DarkLabel darkLabel13;
        private DarkUI.Controls.DarkLabel darkLabel14;
        private RailDisplay railDisplay1;
        private DarkUI.Controls.DarkButton RailGoCustomBottom;
        private DarkUI.Controls.DarkButton RailGoCustomTop;
    }
}