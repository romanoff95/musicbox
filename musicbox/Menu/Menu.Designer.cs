namespace musicbox.Menu
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <paramFormat name="disposing">true if managed resources should be disposed; otherwise, false.</paramFormat>
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
            this.chkMoneyless = new System.Windows.Forms.CheckBox();
            this.trackBarOrderVolume = new System.Windows.Forms.TrackBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCommon = new System.Windows.Forms.TabPage();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPlayQueueClear = new System.Windows.Forms.Button();
            this.btnDemoSelect = new System.Windows.Forms.Button();
            this.btnCancelPlayingNow = new System.Windows.Forms.Button();
            this.tabPageCounters = new System.Windows.Forms.TabPage();
            this.lbStackerBillsCountCommon = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbStackerBillsCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbStackerSumCommon = new System.Windows.Forms.Label();
            this.lbStackerSum = new System.Windows.Forms.Label();
            this.lbBillAcceptorStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPageMoneylessMode = new System.Windows.Forms.TabPage();
            this.tabPageVolume = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarDemoMode = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.cbVolume = new System.Windows.Forms.ComboBox();
            this.tabPagePrice = new System.Windows.Forms.TabPage();
            this.cbPrice = new System.Windows.Forms.ComboBox();
            this.btnDecPrice = new System.Windows.Forms.Button();
            this.btnIncPrice = new System.Windows.Forms.Button();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.tabPageUsers = new System.Windows.Forms.TabPage();
            this.btnSetPermissions = new System.Windows.Forms.Button();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.tabTimer = new System.Windows.Forms.TabPage();
            this.btnMenuDelayDec = new System.Windows.Forms.Button();
            this.btnMenuDelayInc = new System.Windows.Forms.Button();
            this.numMenuDelaySec = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabModes = new System.Windows.Forms.TabPage();
            this.chkModesList = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOrderVolume)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageCommon.SuspendLayout();
            this.tabPageCounters.SuspendLayout();
            this.tabPageMoneylessMode.SuspendLayout();
            this.tabPageVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDemoMode)).BeginInit();
            this.tabPagePrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            this.tabPageUsers.SuspendLayout();
            this.tabTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMenuDelaySec)).BeginInit();
            this.tabModes.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkMoneyless
            // 
            this.chkMoneyless.AutoSize = true;
            this.chkMoneyless.Checked = global::musicbox.Properties.Settings.Default.MoneylessModeEnabled;
            this.chkMoneyless.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMoneyless.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::musicbox.Properties.Settings.Default, "MoneylessModeEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkMoneyless.Location = new System.Drawing.Point(34, 53);
            this.chkMoneyless.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.chkMoneyless.Name = "chkMoneyless";
            this.chkMoneyless.Size = new System.Drawing.Size(260, 33);
            this.chkMoneyless.TabIndex = 0;
            this.chkMoneyless.Text = "Бесплатный режим";
            this.chkMoneyless.UseVisualStyleBackColor = true;
            // 
            // trackBarOrderVolume
            // 
            this.trackBarOrderVolume.BackColor = System.Drawing.Color.Silver;
            this.trackBarOrderVolume.Location = new System.Drawing.Point(23, 111);
            this.trackBarOrderVolume.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.trackBarOrderVolume.Maximum = 100;
            this.trackBarOrderVolume.Name = "trackBarOrderVolume";
            this.trackBarOrderVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarOrderVolume.Size = new System.Drawing.Size(745, 42);
            this.trackBarOrderVolume.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(810, 590);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(222, 51);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCommon);
            this.tabControl1.Controls.Add(this.tabPageCounters);
            this.tabControl1.Controls.Add(this.tabPageMoneylessMode);
            this.tabControl1.Controls.Add(this.tabPageVolume);
            this.tabControl1.Controls.Add(this.tabPagePrice);
            this.tabControl1.Controls.Add(this.tabPageUsers);
            this.tabControl1.Controls.Add(this.tabTimer);
            this.tabControl1.Controls.Add(this.tabModes);
            this.tabControl1.Location = new System.Drawing.Point(2, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1029, 572);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageCommon
            // 
            this.tabPageCommon.Controls.Add(this.btnTest);
            this.tabPageCommon.Controls.Add(this.btnExit);
            this.tabPageCommon.Controls.Add(this.btnPlayQueueClear);
            this.tabPageCommon.Controls.Add(this.btnDemoSelect);
            this.tabPageCommon.Controls.Add(this.btnCancelPlayingNow);
            this.tabPageCommon.Location = new System.Drawing.Point(4, 38);
            this.tabPageCommon.Name = "tabPageCommon";
            this.tabPageCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommon.Size = new System.Drawing.Size(1021, 530);
            this.tabPageCommon.TabIndex = 6;
            this.tabPageCommon.Text = "Общее";
            this.tabPageCommon.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(45, 356);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(323, 51);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(45, 247);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(323, 51);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPlayQueueClear
            // 
            this.btnPlayQueueClear.Location = new System.Drawing.Point(45, 100);
            this.btnPlayQueueClear.Name = "btnPlayQueueClear";
            this.btnPlayQueueClear.Size = new System.Drawing.Size(323, 51);
            this.btnPlayQueueClear.TabIndex = 2;
            this.btnPlayQueueClear.Text = "Очистить очередь";
            this.btnPlayQueueClear.UseVisualStyleBackColor = true;
            this.btnPlayQueueClear.Click += new System.EventHandler(this.btnPlayQueueClear_Click);
            // 
            // btnDemoSelect
            // 
            this.btnDemoSelect.Location = new System.Drawing.Point(45, 172);
            this.btnDemoSelect.Name = "btnDemoSelect";
            this.btnDemoSelect.Size = new System.Drawing.Size(323, 51);
            this.btnDemoSelect.TabIndex = 1;
            this.btnDemoSelect.Text = "Выбор Демо";
            this.btnDemoSelect.UseVisualStyleBackColor = true;
            this.btnDemoSelect.Click += new System.EventHandler(this.btnDemoSelect_Click);
            // 
            // btnCancelPlayingNow
            // 
            this.btnCancelPlayingNow.Location = new System.Drawing.Point(45, 35);
            this.btnCancelPlayingNow.Name = "btnCancelPlayingNow";
            this.btnCancelPlayingNow.Size = new System.Drawing.Size(323, 51);
            this.btnCancelPlayingNow.TabIndex = 0;
            this.btnCancelPlayingNow.Text = "Отмена играемой песни";
            this.btnCancelPlayingNow.UseVisualStyleBackColor = true;
            this.btnCancelPlayingNow.Click += new System.EventHandler(this.btnCancelPlayingNow_Click);
            // 
            // tabPageCounters
            // 
            this.tabPageCounters.Controls.Add(this.lbStackerBillsCountCommon);
            this.tabPageCounters.Controls.Add(this.label13);
            this.tabPageCounters.Controls.Add(this.lbStackerBillsCount);
            this.tabPageCounters.Controls.Add(this.label10);
            this.tabPageCounters.Controls.Add(this.lbStackerSumCommon);
            this.tabPageCounters.Controls.Add(this.lbStackerSum);
            this.tabPageCounters.Controls.Add(this.lbBillAcceptorStatus);
            this.tabPageCounters.Controls.Add(this.label9);
            this.tabPageCounters.Controls.Add(this.label8);
            this.tabPageCounters.Controls.Add(this.label7);
            this.tabPageCounters.Location = new System.Drawing.Point(4, 38);
            this.tabPageCounters.Name = "tabPageCounters";
            this.tabPageCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCounters.Size = new System.Drawing.Size(1021, 530);
            this.tabPageCounters.TabIndex = 5;
            this.tabPageCounters.Text = "Счётчики";
            this.tabPageCounters.UseVisualStyleBackColor = true;
            // 
            // lbStackerBillsCountCommon
            // 
            this.lbStackerBillsCountCommon.AutoSize = true;
            this.lbStackerBillsCountCommon.Location = new System.Drawing.Point(434, 313);
            this.lbStackerBillsCountCommon.Name = "lbStackerBillsCountCommon";
            this.lbStackerBillsCountCommon.Size = new System.Drawing.Size(26, 29);
            this.lbStackerBillsCountCommon.TabIndex = 9;
            this.lbStackerBillsCountCommon.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(51, 313);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(179, 29);
            this.label13.TabIndex = 8;
            this.label13.Text = "Купюр(общее)";
            // 
            // lbStackerBillsCount
            // 
            this.lbStackerBillsCount.AutoSize = true;
            this.lbStackerBillsCount.Location = new System.Drawing.Point(434, 171);
            this.lbStackerBillsCount.Name = "lbStackerBillsCount";
            this.lbStackerBillsCount.Size = new System.Drawing.Size(26, 29);
            this.lbStackerBillsCount.TabIndex = 7;
            this.lbStackerBillsCount.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(51, 171);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 29);
            this.label10.TabIndex = 6;
            this.label10.Text = "Купюр в стакере";
            // 
            // lbStackerSumCommon
            // 
            this.lbStackerSumCommon.AutoSize = true;
            this.lbStackerSumCommon.Location = new System.Drawing.Point(434, 239);
            this.lbStackerSumCommon.Name = "lbStackerSumCommon";
            this.lbStackerSumCommon.Size = new System.Drawing.Size(26, 29);
            this.lbStackerSumCommon.TabIndex = 5;
            this.lbStackerSumCommon.Text = "0";
            // 
            // lbStackerSum
            // 
            this.lbStackerSum.AutoSize = true;
            this.lbStackerSum.Location = new System.Drawing.Point(434, 109);
            this.lbStackerSum.Name = "lbStackerSum";
            this.lbStackerSum.Size = new System.Drawing.Size(26, 29);
            this.lbStackerSum.TabIndex = 4;
            this.lbStackerSum.Text = "0";
            // 
            // lbBillAcceptorStatus
            // 
            this.lbBillAcceptorStatus.AutoSize = true;
            this.lbBillAcceptorStatus.Location = new System.Drawing.Point(434, 50);
            this.lbBillAcceptorStatus.Name = "lbBillAcceptorStatus";
            this.lbBillAcceptorStatus.Size = new System.Drawing.Size(46, 29);
            this.lbBillAcceptorStatus.TabIndex = 3;
            this.lbBillAcceptorStatus.Text = "xxx";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(51, 239);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(181, 29);
            this.label9.TabIndex = 2;
            this.label9.Text = "Сумма(общая)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(206, 29);
            this.label8.TabIndex = 1;
            this.label8.Text = "Сумма в стакере";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(311, 29);
            this.label7.TabIndex = 0;
            this.label7.Text = "Статус купюроприёмника";
            // 
            // tabPageMoneylessMode
            // 
            this.tabPageMoneylessMode.Controls.Add(this.chkMoneyless);
            this.tabPageMoneylessMode.Location = new System.Drawing.Point(4, 38);
            this.tabPageMoneylessMode.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageMoneylessMode.Name = "tabPageMoneylessMode";
            this.tabPageMoneylessMode.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageMoneylessMode.Size = new System.Drawing.Size(1021, 530);
            this.tabPageMoneylessMode.TabIndex = 0;
            this.tabPageMoneylessMode.Text = "Бесплатный режим";
            this.tabPageMoneylessMode.UseVisualStyleBackColor = true;
            // 
            // tabPageVolume
            // 
            this.tabPageVolume.Controls.Add(this.label3);
            this.tabPageVolume.Controls.Add(this.trackBarDemoMode);
            this.tabPageVolume.Controls.Add(this.label2);
            this.tabPageVolume.Controls.Add(this.cbVolume);
            this.tabPageVolume.Controls.Add(this.trackBarOrderVolume);
            this.tabPageVolume.Location = new System.Drawing.Point(4, 38);
            this.tabPageVolume.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageVolume.Name = "tabPageVolume";
            this.tabPageVolume.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageVolume.Size = new System.Drawing.Size(1021, 530);
            this.tabPageVolume.TabIndex = 1;
            this.tabPageVolume.Text = "Громкость";
            this.tabPageVolume.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Демо";
            // 
            // trackBarDemoMode
            // 
            this.trackBarDemoMode.BackColor = System.Drawing.Color.Silver;
            this.trackBarDemoMode.Location = new System.Drawing.Point(23, 248);
            this.trackBarDemoMode.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.trackBarDemoMode.Maximum = 100;
            this.trackBarDemoMode.Name = "trackBarDemoMode";
            this.trackBarDemoMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarDemoMode.Size = new System.Drawing.Size(745, 42);
            this.trackBarDemoMode.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Заказ";
            // 
            // cbVolume
            // 
            this.cbVolume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVolume.FormattingEnabled = true;
            this.cbVolume.Location = new System.Drawing.Point(23, 30);
            this.cbVolume.Name = "cbVolume";
            this.cbVolume.Size = new System.Drawing.Size(318, 37);
            this.cbVolume.TabIndex = 2;
            // 
            // tabPagePrice
            // 
            this.tabPagePrice.Controls.Add(this.cbPrice);
            this.tabPagePrice.Controls.Add(this.btnDecPrice);
            this.tabPagePrice.Controls.Add(this.btnIncPrice);
            this.tabPagePrice.Controls.Add(this.numericUpDownPrice);
            this.tabPagePrice.Location = new System.Drawing.Point(4, 38);
            this.tabPagePrice.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPagePrice.Name = "tabPagePrice";
            this.tabPagePrice.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPagePrice.Size = new System.Drawing.Size(1021, 530);
            this.tabPagePrice.TabIndex = 2;
            this.tabPagePrice.Text = "Стоимость";
            this.tabPagePrice.UseVisualStyleBackColor = true;
            // 
            // cbPrice
            // 
            this.cbPrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrice.FormattingEnabled = true;
            this.cbPrice.Location = new System.Drawing.Point(58, 23);
            this.cbPrice.Name = "cbPrice";
            this.cbPrice.Size = new System.Drawing.Size(318, 37);
            this.cbPrice.TabIndex = 8;
            // 
            // btnDecPrice
            // 
            this.btnDecPrice.Location = new System.Drawing.Point(316, 93);
            this.btnDecPrice.Name = "btnDecPrice";
            this.btnDecPrice.Size = new System.Drawing.Size(139, 51);
            this.btnDecPrice.TabIndex = 7;
            this.btnDecPrice.Text = "-";
            this.btnDecPrice.UseVisualStyleBackColor = true;
            // 
            // btnIncPrice
            // 
            this.btnIncPrice.Location = new System.Drawing.Point(462, 93);
            this.btnIncPrice.Name = "btnIncPrice";
            this.btnIncPrice.Size = new System.Drawing.Size(139, 51);
            this.btnIncPrice.TabIndex = 6;
            this.btnIncPrice.Text = "+";
            this.btnIncPrice.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPrice.Location = new System.Drawing.Point(58, 93);
            this.numericUpDownPrice.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.numericUpDownPrice.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.ReadOnly = true;
            this.numericUpDownPrice.Size = new System.Drawing.Size(199, 35);
            this.numericUpDownPrice.TabIndex = 0;
            // 
            // tabPageUsers
            // 
            this.tabPageUsers.Controls.Add(this.btnSetPermissions);
            this.tabPageUsers.Controls.Add(this.btnSetPassword);
            this.tabPageUsers.Controls.Add(this.cbUser);
            this.tabPageUsers.Location = new System.Drawing.Point(4, 38);
            this.tabPageUsers.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageUsers.Name = "tabPageUsers";
            this.tabPageUsers.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tabPageUsers.Size = new System.Drawing.Size(1021, 530);
            this.tabPageUsers.TabIndex = 3;
            this.tabPageUsers.Text = "Пользователи";
            this.tabPageUsers.UseVisualStyleBackColor = true;
            // 
            // btnSetPermissions
            // 
            this.btnSetPermissions.Location = new System.Drawing.Point(108, 175);
            this.btnSetPermissions.Name = "btnSetPermissions";
            this.btnSetPermissions.Size = new System.Drawing.Size(266, 52);
            this.btnSetPermissions.TabIndex = 2;
            this.btnSetPermissions.Text = "Задать права";
            this.btnSetPermissions.UseVisualStyleBackColor = true;
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Location = new System.Drawing.Point(108, 109);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(266, 52);
            this.btnSetPassword.TabIndex = 1;
            this.btnSetPassword.Text = "Задать пароль";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            // 
            // cbUser
            // 
            this.cbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Location = new System.Drawing.Point(112, 37);
            this.cbUser.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(396, 37);
            this.cbUser.TabIndex = 0;
            // 
            // tabTimer
            // 
            this.tabTimer.Controls.Add(this.btnMenuDelayDec);
            this.tabTimer.Controls.Add(this.btnMenuDelayInc);
            this.tabTimer.Controls.Add(this.numMenuDelaySec);
            this.tabTimer.Controls.Add(this.label1);
            this.tabTimer.Location = new System.Drawing.Point(4, 38);
            this.tabTimer.Name = "tabTimer";
            this.tabTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimer.Size = new System.Drawing.Size(1021, 530);
            this.tabTimer.TabIndex = 7;
            this.tabTimer.Text = "Время";
            this.tabTimer.UseVisualStyleBackColor = true;
            // 
            // btnMenuDelayDec
            // 
            this.btnMenuDelayDec.Location = new System.Drawing.Point(387, 49);
            this.btnMenuDelayDec.Name = "btnMenuDelayDec";
            this.btnMenuDelayDec.Size = new System.Drawing.Size(139, 51);
            this.btnMenuDelayDec.TabIndex = 9;
            this.btnMenuDelayDec.Text = "-";
            this.btnMenuDelayDec.UseVisualStyleBackColor = true;
            // 
            // btnMenuDelayInc
            // 
            this.btnMenuDelayInc.Location = new System.Drawing.Point(544, 49);
            this.btnMenuDelayInc.Name = "btnMenuDelayInc";
            this.btnMenuDelayInc.Size = new System.Drawing.Size(139, 51);
            this.btnMenuDelayInc.TabIndex = 8;
            this.btnMenuDelayInc.Text = "+";
            this.btnMenuDelayInc.UseVisualStyleBackColor = true;
            // 
            // numMenuDelaySec
            // 
            this.numMenuDelaySec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numMenuDelaySec.Location = new System.Drawing.Point(297, 57);
            this.numMenuDelaySec.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMenuDelaySec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMenuDelaySec.Name = "numMenuDelaySec";
            this.numMenuDelaySec.Size = new System.Drawing.Size(58, 35);
            this.numMenuDelaySec.TabIndex = 1;
            this.numMenuDelaySec.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 53);
            this.label1.TabIndex = 0;
            this.label1.Text = "Задержка меню(сек.)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabModes
            // 
            this.tabModes.Controls.Add(this.chkModesList);
            this.tabModes.Location = new System.Drawing.Point(4, 38);
            this.tabModes.Name = "tabModes";
            this.tabModes.Padding = new System.Windows.Forms.Padding(3);
            this.tabModes.Size = new System.Drawing.Size(1021, 530);
            this.tabModes.TabIndex = 8;
            this.tabModes.Text = "Режимы";
            this.tabModes.UseVisualStyleBackColor = true;
            // 
            // chkModesList
            // 
            this.chkModesList.CheckOnClick = true;
            this.chkModesList.FormattingEnabled = true;
            this.chkModesList.Location = new System.Drawing.Point(55, 70);
            this.chkModesList.Name = "chkModesList";
            this.chkModesList.Size = new System.Drawing.Size(257, 184);
            this.chkModesList.TabIndex = 0;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 655);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Menu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOrderVolume)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageCommon.ResumeLayout(false);
            this.tabPageCounters.ResumeLayout(false);
            this.tabPageCounters.PerformLayout();
            this.tabPageMoneylessMode.ResumeLayout(false);
            this.tabPageMoneylessMode.PerformLayout();
            this.tabPageVolume.ResumeLayout(false);
            this.tabPageVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDemoMode)).EndInit();
            this.tabPagePrice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            this.tabPageUsers.ResumeLayout(false);
            this.tabTimer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMenuDelaySec)).EndInit();
            this.tabModes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMoneylessMode;
        private System.Windows.Forms.TabPage tabPageVolume;
        private System.Windows.Forms.TabPage tabPagePrice;
        private System.Windows.Forms.TabPage tabPageUsers;
        public System.Windows.Forms.NumericUpDown numericUpDownPrice;
        public System.Windows.Forms.TrackBar trackBarOrderVolume;
        public System.Windows.Forms.ComboBox cbUser;
        private System.Windows.Forms.TabPage tabPageCounters;
        public System.Windows.Forms.CheckBox chkMoneyless;
        private System.Windows.Forms.TabPage tabPageCommon;
        private System.Windows.Forms.Button btnCancelPlayingNow;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label lbStackerSumCommon;
        public System.Windows.Forms.Label lbStackerSum;
        public System.Windows.Forms.Label lbBillAcceptorStatus;
        public System.Windows.Forms.Button btnDecPrice;
        public System.Windows.Forms.Button btnIncPrice;
        public System.Windows.Forms.Button btnDemoSelect;
        public System.Windows.Forms.Label lbStackerBillsCount;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label lbStackerBillsCountCommon;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnSetPermissions;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.ComboBox cbVolume;
        private System.Windows.Forms.ComboBox cbPrice;
        private System.Windows.Forms.Button btnPlayQueueClear;
        private System.Windows.Forms.TabPage tabTimer;
        public System.Windows.Forms.Button btnMenuDelayDec;
        public System.Windows.Forms.Button btnMenuDelayInc;
        private System.Windows.Forms.NumericUpDown numMenuDelaySec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabModes;
        private System.Windows.Forms.CheckedListBox chkModesList;
        public System.Windows.Forms.Button btnExit;
        public System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TrackBar trackBarDemoMode;
        private System.Windows.Forms.Label label2;
    }
}