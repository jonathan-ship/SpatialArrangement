namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    partial class frmMain
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkshopInfomationWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkshopMatrixInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBlockInfomationBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openPlateConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPlateDateInfomationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLFAlgorithmBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLFWithSlackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLFWithPriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reportRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportRToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dViewerVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dCumlatedOccupyingVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearResultsRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qRCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.learningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grdWorkshopInfo = new System.Windows.Forms.DataGridView();
            this.grdBlockInfo = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabChart = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chtWorkshop1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chtTotal = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.bLFWithFlotidsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWorkshopInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBlockInfo)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabChart.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtWorkshop1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1036, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(40, 17);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.runRToolStripMenuItem,
            this.reportRToolStripMenuItem,
            this.clearCToolStripMenuItem,
            this.cPSCToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1036, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorkshopInfomationWToolStripMenuItem,
            this.openWorkshopMatrixInformationToolStripMenuItem,
            this.openBlockInfomationBToolStripMenuItem,
            this.toolStripSeparator1,
            this.openPlateConfigToolStripMenuItem,
            this.openPlateDateInfomationToolStripMenuItem,
            this.endEToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.fileFToolStripMenuItem.Text = "File (&F)";
            this.fileFToolStripMenuItem.Click += new System.EventHandler(this.fileFToolStripMenuItem_Click);
            // 
            // openWorkshopInfomationWToolStripMenuItem
            // 
            this.openWorkshopInfomationWToolStripMenuItem.Name = "openWorkshopInfomationWToolStripMenuItem";
            this.openWorkshopInfomationWToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openWorkshopInfomationWToolStripMenuItem.Text = "Open Workshop Infomation (&W)";
            this.openWorkshopInfomationWToolStripMenuItem.Click += new System.EventHandler(this.openWorkshopInfomationWToolStripMenuItem_Click);
            // 
            // openWorkshopMatrixInformationToolStripMenuItem
            // 
            this.openWorkshopMatrixInformationToolStripMenuItem.Name = "openWorkshopMatrixInformationToolStripMenuItem";
            this.openWorkshopMatrixInformationToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openWorkshopMatrixInformationToolStripMenuItem.Text = "Open Workshop Matrix Information";
            this.openWorkshopMatrixInformationToolStripMenuItem.Click += new System.EventHandler(this.openWorkshopMatrixInformationToolStripMenuItem_Click);
            // 
            // openBlockInfomationBToolStripMenuItem
            // 
            this.openBlockInfomationBToolStripMenuItem.Name = "openBlockInfomationBToolStripMenuItem";
            this.openBlockInfomationBToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openBlockInfomationBToolStripMenuItem.Text = "Open Block Infomation (&B)";
            this.openBlockInfomationBToolStripMenuItem.Click += new System.EventHandler(this.openBlockInfomationBToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(263, 6);
            // 
            // openPlateConfigToolStripMenuItem
            // 
            this.openPlateConfigToolStripMenuItem.Name = "openPlateConfigToolStripMenuItem";
            this.openPlateConfigToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openPlateConfigToolStripMenuItem.Text = "Open Plate Config";
            this.openPlateConfigToolStripMenuItem.Click += new System.EventHandler(this.openPlateConfigToolStripMenuItem_Click);
            // 
            // openPlateDateInfomationToolStripMenuItem
            // 
            this.openPlateDateInfomationToolStripMenuItem.Name = "openPlateDateInfomationToolStripMenuItem";
            this.openPlateDateInfomationToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.openPlateDateInfomationToolStripMenuItem.Text = "Open Plate Date Infomation";
            this.openPlateDateInfomationToolStripMenuItem.Click += new System.EventHandler(this.openPlateDateInfomationToolStripMenuItem_Click);
            // 
            // endEToolStripMenuItem
            // 
            this.endEToolStripMenuItem.Name = "endEToolStripMenuItem";
            this.endEToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.endEToolStripMenuItem.Text = "End (&E)";
            this.endEToolStripMenuItem.Click += new System.EventHandler(this.endEToolStripMenuItem_Click);
            // 
            // runRToolStripMenuItem
            // 
            this.runRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bLFAlgorithmBToolStripMenuItem,
            this.bLFWithSlackToolStripMenuItem,
            this.bLFWithPriorityToolStripMenuItem,
            this.bLFWithFlotidsToolStripMenuItem,
            this.toolStripSeparator2});
            this.runRToolStripMenuItem.Name = "runRToolStripMenuItem";
            this.runRToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
            this.runRToolStripMenuItem.Text = "Arrangement (&A)";
            // 
            // bLFAlgorithmBToolStripMenuItem
            // 
            this.bLFAlgorithmBToolStripMenuItem.Name = "bLFAlgorithmBToolStripMenuItem";
            this.bLFAlgorithmBToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.bLFAlgorithmBToolStripMenuItem.Text = "BLF Algorithm (&B)";
            this.bLFAlgorithmBToolStripMenuItem.Click += new System.EventHandler(this.bLFAlgorithmBToolStripMenuItem_Click);
            // 
            // bLFWithSlackToolStripMenuItem
            // 
            this.bLFWithSlackToolStripMenuItem.Name = "bLFWithSlackToolStripMenuItem";
            this.bLFWithSlackToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.bLFWithSlackToolStripMenuItem.Text = "BLF with Slack";
            this.bLFWithSlackToolStripMenuItem.Click += new System.EventHandler(this.bLFWithSlackToolStripMenuItem_Click);
            // 
            // bLFWithPriorityToolStripMenuItem
            // 
            this.bLFWithPriorityToolStripMenuItem.Name = "bLFWithPriorityToolStripMenuItem";
            this.bLFWithPriorityToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.bLFWithPriorityToolStripMenuItem.Text = "BLF with Priority";
            this.bLFWithPriorityToolStripMenuItem.Click += new System.EventHandler(this.bLFWithPriorityToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
            // 
            // reportRToolStripMenuItem
            // 
            this.reportRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportRToolStripMenuItem1,
            this.dViewerVToolStripMenuItem,
            this.dCumlatedOccupyingVToolStripMenuItem});
            this.reportRToolStripMenuItem.Name = "reportRToolStripMenuItem";
            this.reportRToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.reportRToolStripMenuItem.Text = "Report (&R)";
            this.reportRToolStripMenuItem.Click += new System.EventHandler(this.reportRToolStripMenuItem_Click);
            // 
            // reportRToolStripMenuItem1
            // 
            this.reportRToolStripMenuItem1.Enabled = false;
            this.reportRToolStripMenuItem1.Name = "reportRToolStripMenuItem1";
            this.reportRToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.reportRToolStripMenuItem1.Text = "Report (&R)";
            this.reportRToolStripMenuItem1.Click += new System.EventHandler(this.reportRToolStripMenuItem1_Click);
            // 
            // dViewerVToolStripMenuItem
            // 
            this.dViewerVToolStripMenuItem.Enabled = false;
            this.dViewerVToolStripMenuItem.Name = "dViewerVToolStripMenuItem";
            this.dViewerVToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.dViewerVToolStripMenuItem.Text = "3D Viewer (&V)";
            this.dViewerVToolStripMenuItem.Click += new System.EventHandler(this.dViewerVToolStripMenuItem_Click);
            // 
            // dCumlatedOccupyingVToolStripMenuItem
            // 
            this.dCumlatedOccupyingVToolStripMenuItem.Name = "dCumlatedOccupyingVToolStripMenuItem";
            this.dCumlatedOccupyingVToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            // 
            // clearCToolStripMenuItem
            // 
            this.clearCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllAToolStripMenuItem,
            this.clearResultsRToolStripMenuItem});
            this.clearCToolStripMenuItem.Name = "clearCToolStripMenuItem";
            this.clearCToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.clearCToolStripMenuItem.Text = "Clear (&C)";
            // 
            // clearAllAToolStripMenuItem
            // 
            this.clearAllAToolStripMenuItem.Name = "clearAllAToolStripMenuItem";
            this.clearAllAToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.clearAllAToolStripMenuItem.Text = "Clear all (&A)";
            this.clearAllAToolStripMenuItem.Click += new System.EventHandler(this.clearAllAToolStripMenuItem_Click);
            // 
            // clearResultsRToolStripMenuItem
            // 
            this.clearResultsRToolStripMenuItem.Name = "clearResultsRToolStripMenuItem";
            this.clearResultsRToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.clearResultsRToolStripMenuItem.Text = "Clear results (&R)";
            this.clearResultsRToolStripMenuItem.Click += new System.EventHandler(this.clearResultsRToolStripMenuItem_Click);
            // 
            // cPSCToolStripMenuItem
            // 
            this.cPSCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qRCodeToolStripMenuItem,
            this.learningToolStripMenuItem});
            this.cPSCToolStripMenuItem.Enabled = false;
            this.cPSCToolStripMenuItem.Name = "cPSCToolStripMenuItem";
            this.cPSCToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.cPSCToolStripMenuItem.Text = "CPS (&C)";
            // 
            // qRCodeToolStripMenuItem
            // 
            this.qRCodeToolStripMenuItem.Name = "qRCodeToolStripMenuItem";
            this.qRCodeToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // learningToolStripMenuItem
            // 
            this.learningToolStripMenuItem.Name = "learningToolStripMenuItem";
            this.learningToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1036, 481);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.grdWorkshopInfo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.grdBlockInfo, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(510, 475);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Workshop Information";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(4, 241);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Block Information";
            // 
            // grdWorkshopInfo
            // 
            this.grdWorkshopInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdWorkshopInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdWorkshopInfo.Location = new System.Drawing.Point(4, 23);
            this.grdWorkshopInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grdWorkshopInfo.Name = "grdWorkshopInfo";
            this.grdWorkshopInfo.RowTemplate.Height = 23;
            this.grdWorkshopInfo.Size = new System.Drawing.Size(502, 211);
            this.grdWorkshopInfo.TabIndex = 2;
            this.grdWorkshopInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdWorkshopInfo_CellContentClick);
            // 
            // grdBlockInfo
            // 
            this.grdBlockInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBlockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBlockInfo.Location = new System.Drawing.Point(4, 260);
            this.grdBlockInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grdBlockInfo.Name = "grdBlockInfo";
            this.grdBlockInfo.RowTemplate.Height = 23;
            this.grdBlockInfo.Size = new System.Drawing.Size(502, 212);
            this.grdBlockInfo.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tabChart, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.chtTotal, 0, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(522, 3);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(510, 475);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(4, 241);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(294, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Block Arrangement Report (Total Workshop)";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Block Arrangement Report";
            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.tabPage1);
            this.tabChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChart.Location = new System.Drawing.Point(4, 23);
            this.tabChart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabChart.Name = "tabChart";
            this.tabChart.SelectedIndex = 0;
            this.tabChart.Size = new System.Drawing.Size(502, 211);
            this.tabChart.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chtWorkshop1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Size = new System.Drawing.Size(494, 185);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Workshop 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chtWorkshop1
            // 
            chartArea1.Name = "ChartArea1";
            this.chtWorkshop1.ChartAreas.Add(chartArea1);
            this.chtWorkshop1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chtWorkshop1.Legends.Add(legend1);
            this.chtWorkshop1.Location = new System.Drawing.Point(4, 3);
            this.chtWorkshop1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chtWorkshop1.Name = "chtWorkshop1";
            this.chtWorkshop1.Size = new System.Drawing.Size(486, 179);
            this.chtWorkshop1.TabIndex = 0;
            this.chtWorkshop1.Text = "chart1";
            // 
            // chtTotal
            // 
            chartArea2.Name = "ChartArea1";
            this.chtTotal.ChartAreas.Add(chartArea2);
            this.chtTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chtTotal.Legends.Add(legend2);
            this.chtTotal.Location = new System.Drawing.Point(4, 260);
            this.chtTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chtTotal.Name = "chtTotal";
            this.chtTotal.Size = new System.Drawing.Size(502, 212);
            this.chtTotal.TabIndex = 5;
            this.chtTotal.Text = "chart2";
            this.chtTotal.Click += new System.EventHandler(this.chtTotal_Click);
            // 
            // bLFWithFlotidsToolStripMenuItem
            // 
            this.bLFWithFlotidsToolStripMenuItem.Name = "bLFWithFlotidsToolStripMenuItem";
            this.bLFWithFlotidsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.bLFWithFlotidsToolStripMenuItem.Text = "BLF with Floating Crane";
            this.bLFWithFlotidsToolStripMenuItem.Click += new System.EventHandler(this.bLFWithFlotidsToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 527);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.Text = "EVA - Shipyard Arrangement Simulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWorkshopInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBlockInfo)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabChart.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtWorkshop1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtTotal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkshopInfomationWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBlockInfomationBToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem endEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bLFAlgorithmBToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem reportRToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dViewerVToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView grdWorkshopInfo;
        private System.Windows.Forms.DataGridView grdBlockInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabChart;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtWorkshop1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtTotal;
        private System.Windows.Forms.ToolStripMenuItem clearCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dCumlatedOccupyingVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qRCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem learningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearResultsRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPlateConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPlateDateInfomationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkshopMatrixInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bLFWithSlackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bLFWithPriorityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bLFWithFlotidsToolStripMenuItem;
    }
}