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
            System.Windows.Forms.AGaugeLabel aGaugeLabel1 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange1 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange2 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange3 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange4 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange5 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeLabel aGaugeLabel2 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange6 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange7 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange8 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeLabel aGaugeLabel3 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange9 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange10 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange11 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeLabel aGaugeLabel4 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange12 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange13 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange14 = new System.Windows.Forms.AGaugeRange();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Control = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.GaugeTable = new System.Windows.Forms.TableLayoutPanel();
            this.GaugeSysVoltage = new System.Windows.Forms.AGauge();
            this.GaugeRailCurrent = new System.Windows.Forms.AGauge();
            this.GaugeDrillCurrent = new System.Windows.Forms.AGauge();
            this.GaugeSysCurrent = new System.Windows.Forms.AGauge();
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
            this.SecTimer = new System.Windows.Forms.Timer(this.components);
            this.UIUpdate = new System.Windows.Forms.Timer(this.components);
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Control.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.GaugeTable.SuspendLayout();
            this.Debugging.SuspendLayout();
            this.DebugList.SuspendLayout();
            this.PacketGenBox.SuspendLayout();
            this.PacketGenLayout.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(784, 522);
            this.tabControl1.TabIndex = 0;
            // 
            // Control
            // 
            this.Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Control.Controls.Add(this.tableLayoutPanel5);
            this.Control.Location = new System.Drawing.Point(4, 22);
            this.Control.Margin = new System.Windows.Forms.Padding(0);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(776, 496);
            this.Control.TabIndex = 0;
            this.Control.Text = "Control";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.GaugeTable, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.cartesianChart1, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(776, 496);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // GaugeTable
            // 
            this.GaugeTable.ColumnCount = 2;
            this.GaugeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.Controls.Add(this.GaugeSysVoltage, 0, 0);
            this.GaugeTable.Controls.Add(this.GaugeRailCurrent, 1, 1);
            this.GaugeTable.Controls.Add(this.GaugeDrillCurrent, 0, 1);
            this.GaugeTable.Controls.Add(this.GaugeSysCurrent, 1, 0);
            this.GaugeTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GaugeTable.Location = new System.Drawing.Point(388, 0);
            this.GaugeTable.Margin = new System.Windows.Forms.Padding(0);
            this.GaugeTable.Name = "GaugeTable";
            this.GaugeTable.RowCount = 2;
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GaugeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.GaugeTable.Size = new System.Drawing.Size(388, 310);
            this.GaugeTable.TabIndex = 0;
            // 
            // GaugeSysVoltage
            // 
            this.GaugeSysVoltage.BaseArcColor = System.Drawing.Color.White;
            this.GaugeSysVoltage.BaseArcRadius = 80;
            this.GaugeSysVoltage.BaseArcStart = 160;
            this.GaugeSysVoltage.BaseArcSweep = 220;
            this.GaugeSysVoltage.BaseArcWidth = 3;
            this.GaugeSysVoltage.Center = new System.Drawing.Point(93, 100);
            this.GaugeSysVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeLabel1.Color = System.Drawing.Color.White;
            aGaugeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aGaugeLabel1.Name = "GaugeLabel1";
            aGaugeLabel1.Position = new System.Drawing.Point(63, 120);
            aGaugeLabel1.Text = "Supply (V)";
            this.GaugeSysVoltage.GaugeLabels.Add(aGaugeLabel1);
            aGaugeRange1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            aGaugeRange1.EndValue = 22.5F;
            aGaugeRange1.InnerRadius = 80;
            aGaugeRange1.InRange = true;
            aGaugeRange1.Name = "GaugeRange3";
            aGaugeRange1.OuterRadius = 85;
            aGaugeRange1.StartValue = 22F;
            aGaugeRange2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            aGaugeRange2.EndValue = 23.5F;
            aGaugeRange2.InnerRadius = 80;
            aGaugeRange2.InRange = true;
            aGaugeRange2.Name = "GaugeRange2";
            aGaugeRange2.OuterRadius = 85;
            aGaugeRange2.StartValue = 22.5F;
            aGaugeRange3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            aGaugeRange3.EndValue = 28.5F;
            aGaugeRange3.InnerRadius = 80;
            aGaugeRange3.InRange = false;
            aGaugeRange3.Name = "GaugeRange1";
            aGaugeRange3.OuterRadius = 85;
            aGaugeRange3.StartValue = 23.5F;
            aGaugeRange4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            aGaugeRange4.EndValue = 29.25F;
            aGaugeRange4.InnerRadius = 80;
            aGaugeRange4.InRange = false;
            aGaugeRange4.Name = "GaugeRange4";
            aGaugeRange4.OuterRadius = 85;
            aGaugeRange4.StartValue = 28.5F;
            aGaugeRange5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            aGaugeRange5.EndValue = 30F;
            aGaugeRange5.InnerRadius = 80;
            aGaugeRange5.InRange = false;
            aGaugeRange5.Name = "GaugeRange5";
            aGaugeRange5.OuterRadius = 85;
            aGaugeRange5.StartValue = 29.25F;
            this.GaugeSysVoltage.GaugeRanges.Add(aGaugeRange1);
            this.GaugeSysVoltage.GaugeRanges.Add(aGaugeRange2);
            this.GaugeSysVoltage.GaugeRanges.Add(aGaugeRange3);
            this.GaugeSysVoltage.GaugeRanges.Add(aGaugeRange4);
            this.GaugeSysVoltage.GaugeRanges.Add(aGaugeRange5);
            this.GaugeSysVoltage.Location = new System.Drawing.Point(3, 3);
            this.GaugeSysVoltage.MaxValue = 30F;
            this.GaugeSysVoltage.MinValue = 22F;
            this.GaugeSysVoltage.Name = "GaugeSysVoltage";
            this.GaugeSysVoltage.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.GaugeSysVoltage.NeedleColor2 = System.Drawing.Color.LightGray;
            this.GaugeSysVoltage.NeedleRadius = 80;
            this.GaugeSysVoltage.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.GaugeSysVoltage.NeedleWidth = 2;
            this.GaugeSysVoltage.ScaleLinesInterColor = System.Drawing.Color.White;
            this.GaugeSysVoltage.ScaleLinesInterInnerRadius = 73;
            this.GaugeSysVoltage.ScaleLinesInterOuterRadius = 80;
            this.GaugeSysVoltage.ScaleLinesInterWidth = 1;
            this.GaugeSysVoltage.ScaleLinesMajorColor = System.Drawing.Color.White;
            this.GaugeSysVoltage.ScaleLinesMajorInnerRadius = 70;
            this.GaugeSysVoltage.ScaleLinesMajorOuterRadius = 80;
            this.GaugeSysVoltage.ScaleLinesMajorStepValue = 1F;
            this.GaugeSysVoltage.ScaleLinesMajorWidth = 2;
            this.GaugeSysVoltage.ScaleLinesMinorColor = System.Drawing.Color.LightGray;
            this.GaugeSysVoltage.ScaleLinesMinorInnerRadius = 75;
            this.GaugeSysVoltage.ScaleLinesMinorOuterRadius = 80;
            this.GaugeSysVoltage.ScaleLinesMinorTicks = 3;
            this.GaugeSysVoltage.ScaleLinesMinorWidth = 1;
            this.GaugeSysVoltage.ScaleNumbersColor = System.Drawing.Color.White;
            this.GaugeSysVoltage.ScaleNumbersFormat = null;
            this.GaugeSysVoltage.ScaleNumbersRadius = 90;
            this.GaugeSysVoltage.ScaleNumbersRotation = 90;
            this.GaugeSysVoltage.ScaleNumbersStartScaleLine = 1;
            this.GaugeSysVoltage.ScaleNumbersStepScaleLines = 1;
            this.GaugeSysVoltage.Size = new System.Drawing.Size(188, 149);
            this.GaugeSysVoltage.TabIndex = 4;
            this.GaugeSysVoltage.Text = "Supply Voltage";
            this.GaugeSysVoltage.Value = 22.5F;
            // 
            // GaugeRailCurrent
            // 
            this.GaugeRailCurrent.BaseArcColor = System.Drawing.Color.White;
            this.GaugeRailCurrent.BaseArcRadius = 80;
            this.GaugeRailCurrent.BaseArcStart = 160;
            this.GaugeRailCurrent.BaseArcSweep = 220;
            this.GaugeRailCurrent.BaseArcWidth = 3;
            this.GaugeRailCurrent.Center = new System.Drawing.Point(93, 100);
            this.GaugeRailCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeLabel2.Color = System.Drawing.Color.White;
            aGaugeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aGaugeLabel2.Name = "GaugeLabel1";
            aGaugeLabel2.Position = new System.Drawing.Point(74, 120);
            aGaugeLabel2.Text = "Rail (A)";
            this.GaugeRailCurrent.GaugeLabels.Add(aGaugeLabel2);
            aGaugeRange6.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            aGaugeRange6.EndValue = 35F;
            aGaugeRange6.InnerRadius = 80;
            aGaugeRange6.InRange = false;
            aGaugeRange6.Name = "GaugeRange1";
            aGaugeRange6.OuterRadius = 85;
            aGaugeRange6.StartValue = 0F;
            aGaugeRange7.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            aGaugeRange7.EndValue = 45F;
            aGaugeRange7.InnerRadius = 80;
            aGaugeRange7.InRange = false;
            aGaugeRange7.Name = "GaugeRange2";
            aGaugeRange7.OuterRadius = 85;
            aGaugeRange7.StartValue = 35F;
            aGaugeRange8.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            aGaugeRange8.EndValue = 60F;
            aGaugeRange8.InnerRadius = 80;
            aGaugeRange8.InRange = false;
            aGaugeRange8.Name = "GaugeRange3";
            aGaugeRange8.OuterRadius = 85;
            aGaugeRange8.StartValue = 45F;
            this.GaugeRailCurrent.GaugeRanges.Add(aGaugeRange6);
            this.GaugeRailCurrent.GaugeRanges.Add(aGaugeRange7);
            this.GaugeRailCurrent.GaugeRanges.Add(aGaugeRange8);
            this.GaugeRailCurrent.Location = new System.Drawing.Point(197, 158);
            this.GaugeRailCurrent.MaxValue = 60F;
            this.GaugeRailCurrent.MinValue = 0F;
            this.GaugeRailCurrent.Name = "GaugeRailCurrent";
            this.GaugeRailCurrent.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.GaugeRailCurrent.NeedleColor2 = System.Drawing.Color.LightGray;
            this.GaugeRailCurrent.NeedleRadius = 80;
            this.GaugeRailCurrent.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.GaugeRailCurrent.NeedleWidth = 2;
            this.GaugeRailCurrent.ScaleLinesInterColor = System.Drawing.Color.White;
            this.GaugeRailCurrent.ScaleLinesInterInnerRadius = 73;
            this.GaugeRailCurrent.ScaleLinesInterOuterRadius = 80;
            this.GaugeRailCurrent.ScaleLinesInterWidth = 1;
            this.GaugeRailCurrent.ScaleLinesMajorColor = System.Drawing.Color.White;
            this.GaugeRailCurrent.ScaleLinesMajorInnerRadius = 70;
            this.GaugeRailCurrent.ScaleLinesMajorOuterRadius = 80;
            this.GaugeRailCurrent.ScaleLinesMajorStepValue = 10F;
            this.GaugeRailCurrent.ScaleLinesMajorWidth = 2;
            this.GaugeRailCurrent.ScaleLinesMinorColor = System.Drawing.Color.LightGray;
            this.GaugeRailCurrent.ScaleLinesMinorInnerRadius = 75;
            this.GaugeRailCurrent.ScaleLinesMinorOuterRadius = 80;
            this.GaugeRailCurrent.ScaleLinesMinorTicks = 9;
            this.GaugeRailCurrent.ScaleLinesMinorWidth = 1;
            this.GaugeRailCurrent.ScaleNumbersColor = System.Drawing.Color.White;
            this.GaugeRailCurrent.ScaleNumbersFormat = null;
            this.GaugeRailCurrent.ScaleNumbersRadius = 90;
            this.GaugeRailCurrent.ScaleNumbersRotation = 90;
            this.GaugeRailCurrent.ScaleNumbersStartScaleLine = 1;
            this.GaugeRailCurrent.ScaleNumbersStepScaleLines = 1;
            this.GaugeRailCurrent.Size = new System.Drawing.Size(188, 149);
            this.GaugeRailCurrent.TabIndex = 3;
            this.GaugeRailCurrent.Text = "aGauge3";
            this.GaugeRailCurrent.Value = 0F;
            // 
            // GaugeDrillCurrent
            // 
            this.GaugeDrillCurrent.BaseArcColor = System.Drawing.Color.White;
            this.GaugeDrillCurrent.BaseArcRadius = 80;
            this.GaugeDrillCurrent.BaseArcStart = 160;
            this.GaugeDrillCurrent.BaseArcSweep = 220;
            this.GaugeDrillCurrent.BaseArcWidth = 3;
            this.GaugeDrillCurrent.Center = new System.Drawing.Point(93, 100);
            this.GaugeDrillCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeLabel3.Color = System.Drawing.Color.White;
            aGaugeLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aGaugeLabel3.Name = "GaugeLabel1";
            aGaugeLabel3.Position = new System.Drawing.Point(73, 120);
            aGaugeLabel3.Text = "Drill (A)";
            this.GaugeDrillCurrent.GaugeLabels.Add(aGaugeLabel3);
            aGaugeRange9.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            aGaugeRange9.EndValue = 9F;
            aGaugeRange9.InnerRadius = 80;
            aGaugeRange9.InRange = false;
            aGaugeRange9.Name = "GaugeRange1";
            aGaugeRange9.OuterRadius = 85;
            aGaugeRange9.StartValue = 0F;
            aGaugeRange10.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            aGaugeRange10.EndValue = 12F;
            aGaugeRange10.InnerRadius = 80;
            aGaugeRange10.InRange = false;
            aGaugeRange10.Name = "GaugeRange2";
            aGaugeRange10.OuterRadius = 85;
            aGaugeRange10.StartValue = 9F;
            aGaugeRange11.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            aGaugeRange11.EndValue = 15F;
            aGaugeRange11.InnerRadius = 80;
            aGaugeRange11.InRange = false;
            aGaugeRange11.Name = "GaugeRange3";
            aGaugeRange11.OuterRadius = 85;
            aGaugeRange11.StartValue = 12F;
            this.GaugeDrillCurrent.GaugeRanges.Add(aGaugeRange9);
            this.GaugeDrillCurrent.GaugeRanges.Add(aGaugeRange10);
            this.GaugeDrillCurrent.GaugeRanges.Add(aGaugeRange11);
            this.GaugeDrillCurrent.Location = new System.Drawing.Point(3, 158);
            this.GaugeDrillCurrent.MaxValue = 15F;
            this.GaugeDrillCurrent.MinValue = 0F;
            this.GaugeDrillCurrent.Name = "GaugeDrillCurrent";
            this.GaugeDrillCurrent.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.GaugeDrillCurrent.NeedleColor2 = System.Drawing.Color.LightGray;
            this.GaugeDrillCurrent.NeedleRadius = 80;
            this.GaugeDrillCurrent.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.GaugeDrillCurrent.NeedleWidth = 2;
            this.GaugeDrillCurrent.ScaleLinesInterColor = System.Drawing.Color.White;
            this.GaugeDrillCurrent.ScaleLinesInterInnerRadius = 73;
            this.GaugeDrillCurrent.ScaleLinesInterOuterRadius = 80;
            this.GaugeDrillCurrent.ScaleLinesInterWidth = 1;
            this.GaugeDrillCurrent.ScaleLinesMajorColor = System.Drawing.Color.White;
            this.GaugeDrillCurrent.ScaleLinesMajorInnerRadius = 70;
            this.GaugeDrillCurrent.ScaleLinesMajorOuterRadius = 80;
            this.GaugeDrillCurrent.ScaleLinesMajorStepValue = 3F;
            this.GaugeDrillCurrent.ScaleLinesMajorWidth = 2;
            this.GaugeDrillCurrent.ScaleLinesMinorColor = System.Drawing.Color.LightGray;
            this.GaugeDrillCurrent.ScaleLinesMinorInnerRadius = 75;
            this.GaugeDrillCurrent.ScaleLinesMinorOuterRadius = 80;
            this.GaugeDrillCurrent.ScaleLinesMinorTicks = 5;
            this.GaugeDrillCurrent.ScaleLinesMinorWidth = 1;
            this.GaugeDrillCurrent.ScaleNumbersColor = System.Drawing.Color.White;
            this.GaugeDrillCurrent.ScaleNumbersFormat = null;
            this.GaugeDrillCurrent.ScaleNumbersRadius = 90;
            this.GaugeDrillCurrent.ScaleNumbersRotation = 90;
            this.GaugeDrillCurrent.ScaleNumbersStartScaleLine = 1;
            this.GaugeDrillCurrent.ScaleNumbersStepScaleLines = 1;
            this.GaugeDrillCurrent.Size = new System.Drawing.Size(188, 149);
            this.GaugeDrillCurrent.TabIndex = 2;
            this.GaugeDrillCurrent.Text = "aGauge3";
            this.GaugeDrillCurrent.Value = 0F;
            // 
            // GaugeSysCurrent
            // 
            this.GaugeSysCurrent.BaseArcColor = System.Drawing.Color.White;
            this.GaugeSysCurrent.BaseArcRadius = 80;
            this.GaugeSysCurrent.BaseArcStart = 160;
            this.GaugeSysCurrent.BaseArcSweep = 220;
            this.GaugeSysCurrent.BaseArcWidth = 3;
            this.GaugeSysCurrent.Center = new System.Drawing.Point(93, 100);
            this.GaugeSysCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeLabel4.Color = System.Drawing.Color.White;
            aGaugeLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aGaugeLabel4.Name = "GaugeLabel1";
            aGaugeLabel4.Position = new System.Drawing.Point(64, 120);
            aGaugeLabel4.Text = "System (A)";
            this.GaugeSysCurrent.GaugeLabels.Add(aGaugeLabel4);
            aGaugeRange12.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            aGaugeRange12.EndValue = 5F;
            aGaugeRange12.InnerRadius = 80;
            aGaugeRange12.InRange = false;
            aGaugeRange12.Name = "GaugeRange1";
            aGaugeRange12.OuterRadius = 85;
            aGaugeRange12.StartValue = 0F;
            aGaugeRange13.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            aGaugeRange13.EndValue = 8F;
            aGaugeRange13.InnerRadius = 80;
            aGaugeRange13.InRange = false;
            aGaugeRange13.Name = "GaugeRange2";
            aGaugeRange13.OuterRadius = 85;
            aGaugeRange13.StartValue = 5F;
            aGaugeRange14.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            aGaugeRange14.EndValue = 10F;
            aGaugeRange14.InnerRadius = 80;
            aGaugeRange14.InRange = false;
            aGaugeRange14.Name = "GaugeRange3";
            aGaugeRange14.OuterRadius = 85;
            aGaugeRange14.StartValue = 8F;
            this.GaugeSysCurrent.GaugeRanges.Add(aGaugeRange12);
            this.GaugeSysCurrent.GaugeRanges.Add(aGaugeRange13);
            this.GaugeSysCurrent.GaugeRanges.Add(aGaugeRange14);
            this.GaugeSysCurrent.Location = new System.Drawing.Point(197, 3);
            this.GaugeSysCurrent.MaxValue = 10F;
            this.GaugeSysCurrent.MinValue = 0F;
            this.GaugeSysCurrent.Name = "GaugeSysCurrent";
            this.GaugeSysCurrent.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.GaugeSysCurrent.NeedleColor2 = System.Drawing.Color.LightGray;
            this.GaugeSysCurrent.NeedleRadius = 80;
            this.GaugeSysCurrent.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.GaugeSysCurrent.NeedleWidth = 2;
            this.GaugeSysCurrent.ScaleLinesInterColor = System.Drawing.Color.White;
            this.GaugeSysCurrent.ScaleLinesInterInnerRadius = 73;
            this.GaugeSysCurrent.ScaleLinesInterOuterRadius = 80;
            this.GaugeSysCurrent.ScaleLinesInterWidth = 1;
            this.GaugeSysCurrent.ScaleLinesMajorColor = System.Drawing.Color.White;
            this.GaugeSysCurrent.ScaleLinesMajorInnerRadius = 70;
            this.GaugeSysCurrent.ScaleLinesMajorOuterRadius = 80;
            this.GaugeSysCurrent.ScaleLinesMajorStepValue = 2F;
            this.GaugeSysCurrent.ScaleLinesMajorWidth = 2;
            this.GaugeSysCurrent.ScaleLinesMinorColor = System.Drawing.Color.LightGray;
            this.GaugeSysCurrent.ScaleLinesMinorInnerRadius = 75;
            this.GaugeSysCurrent.ScaleLinesMinorOuterRadius = 80;
            this.GaugeSysCurrent.ScaleLinesMinorTicks = 9;
            this.GaugeSysCurrent.ScaleLinesMinorWidth = 1;
            this.GaugeSysCurrent.ScaleNumbersColor = System.Drawing.Color.White;
            this.GaugeSysCurrent.ScaleNumbersFormat = null;
            this.GaugeSysCurrent.ScaleNumbersRadius = 90;
            this.GaugeSysCurrent.ScaleNumbersRotation = 90;
            this.GaugeSysCurrent.ScaleNumbersStartScaleLine = 1;
            this.GaugeSysCurrent.ScaleNumbersStepScaleLines = 1;
            this.GaugeSysCurrent.Size = new System.Drawing.Size(188, 149);
            this.GaugeSysCurrent.TabIndex = 0;
            this.GaugeSysCurrent.Text = "Total System Current";
            this.GaugeSysCurrent.Value = 0F;
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
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.PacketGenLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
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
            this.TimestampTitle.Size = new System.Drawing.Size(114, 17);
            this.TimestampTitle.TabIndex = 0;
            this.TimestampTitle.Text = "Timestamp";
            // 
            // IDTitle
            // 
            this.IDTitle.AutoSize = true;
            this.IDTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IDTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.IDTitle.Location = new System.Drawing.Point(123, 0);
            this.IDTitle.Name = "IDTitle";
            this.IDTitle.Size = new System.Drawing.Size(84, 17);
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
            this.IDTextbox.Location = new System.Drawing.Point(123, 20);
            this.IDTextbox.Name = "IDTextbox";
            this.IDTextbox.Size = new System.Drawing.Size(84, 20);
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
            this.DataTextbox.Location = new System.Drawing.Point(213, 20);
            this.DataTextbox.Name = "DataTextbox";
            this.DataTextbox.Size = new System.Drawing.Size(385, 20);
            this.DataTextbox.TabIndex = 4;
            this.DataTextbox.TextChanged += new System.EventHandler(this.DataTextbox_TextChanged);
            // 
            // DataTitle
            // 
            this.DataTitle.AutoSize = true;
            this.DataTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DataTitle.Location = new System.Drawing.Point(213, 0);
            this.DataTitle.Name = "DataTitle";
            this.DataTitle.Size = new System.Drawing.Size(385, 17);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(120, 65);
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
            this.TimestampTextbox.Size = new System.Drawing.Size(114, 20);
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
            this.InterpretationTimestamp.Size = new System.Drawing.Size(114, 13);
            this.InterpretationTimestamp.TabIndex = 8;
            this.InterpretationTimestamp.Text = "Unknown";
            // 
            // InterpretationID
            // 
            this.InterpretationID.AutoSize = true;
            this.InterpretationID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterpretationID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InterpretationID.Location = new System.Drawing.Point(123, 82);
            this.InterpretationID.Name = "InterpretationID";
            this.InterpretationID.Size = new System.Drawing.Size(84, 13);
            this.InterpretationID.TabIndex = 9;
            this.InterpretationID.Text = "Unknown";
            // 
            // InterpretationData
            // 
            this.InterpretationData.AutoSize = true;
            this.InterpretationData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InterpretationData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InterpretationData.Location = new System.Drawing.Point(213, 82);
            this.InterpretationData.Name = "InterpretationData";
            this.InterpretationData.Size = new System.Drawing.Size(385, 13);
            this.InterpretationData.TabIndex = 10;
            this.InterpretationData.Text = "Unknown";
            // 
            // PacketConstructStatus
            // 
            this.PacketConstructStatus.AutoSize = true;
            this.PacketConstructStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketConstructStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.PacketConstructStatus.Location = new System.Drawing.Point(604, 82);
            this.PacketConstructStatus.Name = "PacketConstructStatus";
            this.PacketConstructStatus.Size = new System.Drawing.Size(153, 13);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(601, 17);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(159, 65);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // SendPacketBtn
            // 
            this.SendPacketBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendPacketBtn.Location = new System.Drawing.Point(3, 35);
            this.SendPacketBtn.Name = "SendPacketBtn";
            this.SendPacketBtn.Padding = new System.Windows.Forms.Padding(5);
            this.SendPacketBtn.Size = new System.Drawing.Size(153, 25);
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
            this.ClientSelector.Size = new System.Drawing.Size(153, 21);
            this.ClientSelector.TabIndex = 6;
            // 
            // SendAsUDP
            // 
            this.SendAsUDP.AutoSize = true;
            this.SendAsUDP.Checked = true;
            this.SendAsUDP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SendAsUDP.Location = new System.Drawing.Point(604, 0);
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
            this.groupBox1.Location = new System.Drawing.Point(5, 129);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(766, 114);
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
            this.tableLayoutPanel4.Size = new System.Drawing.Size(760, 95);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // StatsPacketQueueOut
            // 
            this.StatsPacketQueueOut.AutoSize = true;
            this.StatsPacketQueueOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatsPacketQueueOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.StatsPacketQueueOut.Location = new System.Drawing.Point(3, 0);
            this.StatsPacketQueueOut.Name = "StatsPacketQueueOut";
            this.StatsPacketQueueOut.Size = new System.Drawing.Size(184, 31);
            this.StatsPacketQueueOut.TabIndex = 0;
            this.StatsPacketQueueOut.Text = "Packet Queue Out: N/A";
            this.StatsPacketQueueOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatsPacketQueueIn
            // 
            this.StatsPacketQueueIn.AutoSize = true;
            this.StatsPacketQueueIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatsPacketQueueIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.StatsPacketQueueIn.Location = new System.Drawing.Point(3, 31);
            this.StatsPacketQueueIn.Name = "StatsPacketQueueIn";
            this.StatsPacketQueueIn.Size = new System.Drawing.Size(184, 31);
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
            this.EmergencyStopBtn.Location = new System.Drawing.Point(320, 525);
            this.EmergencyStopBtn.Name = "EmergencyStopBtn";
            this.EmergencyStopBtn.Padding = new System.Windows.Forms.Padding(5);
            this.EmergencyStopBtn.Size = new System.Drawing.Size(144, 33);
            this.EmergencyStopBtn.TabIndex = 999;
            this.EmergencyStopBtn.Text = "EMERGENCY STOP";
            this.EmergencyStopBtn.UseVisualStyleBackColor = false;
            this.EmergencyStopBtn.Click += new System.EventHandler(this.EmergencyStopClick);
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
            // cartesianChart1
            // 
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart1.Location = new System.Drawing.Point(3, 313);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(382, 180);
            this.cartesianChart1.TabIndex = 1;
            this.cartesianChart1.Text = "cartesianChart1";
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
            this.Control.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.GaugeTable.ResumeLayout(false);
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
        private System.Windows.Forms.AGauge GaugeSysCurrent;
        private System.Windows.Forms.AGauge GaugeDrillCurrent;
        private System.Windows.Forms.AGauge GaugeRailCurrent;
        private System.Windows.Forms.AGauge GaugeSysVoltage;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
    }
}