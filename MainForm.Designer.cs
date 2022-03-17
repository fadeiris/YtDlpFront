namespace YtDlpFront
{
    partial class MainForm
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
            this.GBUrl = new System.Windows.Forms.GroupBox();
            this.TBUrl = new System.Windows.Forms.TextBox();
            this.GBLog = new System.Windows.Forms.GroupBox();
            this.RtbLogCtrlLog = new Serilog.Sinks.WinForms.Core.RichTextBoxLogControl();
            this.BtnClearLog = new System.Windows.Forms.Button();
            this.BtnExportLog = new System.Windows.Forms.Button();
            this.GBFunctionalOperation = new System.Windows.Forms.GroupBox();
            this.BtnBurnIn = new System.Windows.Forms.Button();
            this.BtnSplitLocalVideo = new System.Windows.Forms.Button();
            this.BtnUpdateYtDlp = new System.Windows.Forms.Button();
            this.BtnFetchVideoInfo = new System.Windows.Forms.Button();
            this.BtnDownloadVideo = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.GBStartTime = new System.Windows.Forms.GroupBox();
            this.MTBStartTime = new System.Windows.Forms.MaskedTextBox();
            this.TBStartSeconds = new System.Windows.Forms.TextBox();
            this.GBEndTime = new System.Windows.Forms.GroupBox();
            this.MTBEndtime = new System.Windows.Forms.MaskedTextBox();
            this.TBEndSeconds = new System.Windows.Forms.TextBox();
            this.GBClipName = new System.Windows.Forms.GroupBox();
            this.TBClipName = new System.Windows.Forms.TextBox();
            this.GBLoadCookiesFromBrowser = new System.Windows.Forms.GroupBox();
            this.CBEnableLoadCookiesFromBrowser = new System.Windows.Forms.CheckBox();
            this.TBBrowserProfile = new System.Windows.Forms.TextBox();
            this.CBBrowser = new System.Windows.Forms.ComboBox();
            this.GBFormat = new System.Windows.Forms.GroupBox();
            this.TBFormatSelection = new System.Windows.Forms.TextBox();
            this.GBOther = new System.Windows.Forms.GroupBox();
            this.CBEnableSplitChapters = new System.Windows.Forms.CheckBox();
            this.CBEnableWaitForVideo = new System.Windows.Forms.CheckBox();
            this.CBEnableLiveFromStart = new System.Windows.Forms.CheckBox();
            this.CBEnableEmbedMeta = new System.Windows.Forms.CheckBox();
            this.CBDownloadFirstSplitLater = new System.Windows.Forms.CheckBox();
            this.CBDeleteSourceFile = new System.Windows.Forms.CheckBox();
            this.CBHardwareAcceleratorType = new System.Windows.Forms.ComboBox();
            this.CBUseHardwareAcceleration = new System.Windows.Forms.CheckBox();
            this.BtnDlAria2 = new System.Windows.Forms.Button();
            this.BtnDlFFmpeg = new System.Windows.Forms.Button();
            this.BtnOpenDownloadFolder = new System.Windows.Forms.Button();
            this.BtnDlYtDlp = new System.Windows.Forms.Button();
            this.LYtDlpVer = new System.Windows.Forms.Label();
            this.LVersion = new System.Windows.Forms.Label();
            this.GBNewClip = new System.Windows.Forms.GroupBox();
            this.CBIsAudioOnly = new System.Windows.Forms.CheckBox();
            this.BtnAddNewClip = new System.Windows.Forms.Button();
            this.BtnResetNewClip = new System.Windows.Forms.Button();
            this.GBClip = new System.Windows.Forms.GroupBox();
            this.TBAutoEndSeconds = new System.Windows.Forms.TextBox();
            this.DgvClipList = new System.Windows.Forms.DataGridView();
            this.ClipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartSeconds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndSeconds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsAudioOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BtnClearClipList = new System.Windows.Forms.Button();
            this.BtnImportTimestamp = new System.Windows.Forms.Button();
            this.GBDownloadExternalProgram = new System.Windows.Forms.GroupBox();
            this.GBHyperlink = new System.Windows.Forms.GroupBox();
            this.LLTwitCasting = new System.Windows.Forms.LinkLabel();
            this.LLbilibili = new System.Windows.Forms.LinkLabel();
            this.LLTwitch = new System.Windows.Forms.LinkLabel();
            this.LLYouTube = new System.Windows.Forms.LinkLabel();
            this.LLAria2 = new System.Windows.Forms.LinkLabel();
            this.LLYtDlpFFmpeg = new System.Windows.Forms.LinkLabel();
            this.LLYtDlp = new System.Windows.Forms.LinkLabel();
            this.GBFFmpegUseHardwareAcceleration = new System.Windows.Forms.GroupBox();
            this.CBDeviceNo = new System.Windows.Forms.ComboBox();
            this.GBOpenFolder = new System.Windows.Forms.GroupBox();
            this.BtnOpenLogsFolder = new System.Windows.Forms.Button();
            this.BtnOpenConfigFolder = new System.Windows.Forms.Button();
            this.BtnOpenBinFolder = new System.Windows.Forms.Button();
            this.BtnOpenSubtitleCreator = new System.Windows.Forms.Button();
            this.BtnOpenYTSecConverter = new System.Windows.Forms.Button();
            this.GBSubtitleOption = new System.Windows.Forms.GroupBox();
            this.CBApplyFontSetting = new System.Windows.Forms.CheckBox();
            this.TBCustomEncode = new System.Windows.Forms.TextBox();
            this.CBEncode = new System.Windows.Forms.ComboBox();
            this.TBCustomFont = new System.Windows.Forms.TextBox();
            this.CBFont = new System.Windows.Forms.ComboBox();
            this.GBUrl.SuspendLayout();
            this.GBLog.SuspendLayout();
            this.GBFunctionalOperation.SuspendLayout();
            this.GBStartTime.SuspendLayout();
            this.GBEndTime.SuspendLayout();
            this.GBClipName.SuspendLayout();
            this.GBLoadCookiesFromBrowser.SuspendLayout();
            this.GBFormat.SuspendLayout();
            this.GBOther.SuspendLayout();
            this.GBNewClip.SuspendLayout();
            this.GBClip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvClipList)).BeginInit();
            this.GBDownloadExternalProgram.SuspendLayout();
            this.GBHyperlink.SuspendLayout();
            this.GBFFmpegUseHardwareAcceleration.SuspendLayout();
            this.GBOpenFolder.SuspendLayout();
            this.GBSubtitleOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBUrl
            // 
            this.GBUrl.Controls.Add(this.TBUrl);
            this.GBUrl.Location = new System.Drawing.Point(12, 12);
            this.GBUrl.Name = "GBUrl";
            this.GBUrl.Size = new System.Drawing.Size(285, 60);
            this.GBUrl.TabIndex = 0;
            this.GBUrl.TabStop = false;
            this.GBUrl.Text = "網址";
            // 
            // TBUrl
            // 
            this.TBUrl.Location = new System.Drawing.Point(6, 22);
            this.TBUrl.Name = "TBUrl";
            this.TBUrl.Size = new System.Drawing.Size(273, 23);
            this.TBUrl.TabIndex = 0;
            this.TBUrl.TextChanged += new System.EventHandler(this.TBUrl_TextChanged);
            // 
            // GBLog
            // 
            this.GBLog.Controls.Add(this.RtbLogCtrlLog);
            this.GBLog.Controls.Add(this.BtnClearLog);
            this.GBLog.Controls.Add(this.BtnExportLog);
            this.GBLog.Location = new System.Drawing.Point(597, 304);
            this.GBLog.Name = "GBLog";
            this.GBLog.Size = new System.Drawing.Size(502, 298);
            this.GBLog.TabIndex = 12;
            this.GBLog.TabStop = false;
            this.GBLog.Text = "日誌";
            // 
            // RtbLogCtrlLog
            // 
            this.RtbLogCtrlLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RtbLogCtrlLog.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RtbLogCtrlLog.ForContext = "";
            this.RtbLogCtrlLog.Location = new System.Drawing.Point(6, 51);
            this.RtbLogCtrlLog.Name = "RtbLogCtrlLog";
            this.RtbLogCtrlLog.ReadOnly = true;
            this.RtbLogCtrlLog.Size = new System.Drawing.Size(490, 241);
            this.RtbLogCtrlLog.TabIndex = 3;
            this.RtbLogCtrlLog.Text = "";
            this.RtbLogCtrlLog.WordWrap = false;
            // 
            // BtnClearLog
            // 
            this.BtnClearLog.Location = new System.Drawing.Point(102, 22);
            this.BtnClearLog.Name = "BtnClearLog";
            this.BtnClearLog.Size = new System.Drawing.Size(90, 23);
            this.BtnClearLog.TabIndex = 1;
            this.BtnClearLog.Text = "清除日誌";
            this.BtnClearLog.UseVisualStyleBackColor = true;
            this.BtnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // BtnExportLog
            // 
            this.BtnExportLog.Location = new System.Drawing.Point(6, 22);
            this.BtnExportLog.Name = "BtnExportLog";
            this.BtnExportLog.Size = new System.Drawing.Size(90, 23);
            this.BtnExportLog.TabIndex = 0;
            this.BtnExportLog.Text = "匯出日誌";
            this.BtnExportLog.UseVisualStyleBackColor = true;
            this.BtnExportLog.Click += new System.EventHandler(this.BtnExportLog_Click);
            // 
            // GBFunctionalOperation
            // 
            this.GBFunctionalOperation.Controls.Add(this.BtnBurnIn);
            this.GBFunctionalOperation.Controls.Add(this.BtnSplitLocalVideo);
            this.GBFunctionalOperation.Controls.Add(this.BtnUpdateYtDlp);
            this.GBFunctionalOperation.Controls.Add(this.BtnFetchVideoInfo);
            this.GBFunctionalOperation.Controls.Add(this.BtnDownloadVideo);
            this.GBFunctionalOperation.Controls.Add(this.BtnCancel);
            this.GBFunctionalOperation.Location = new System.Drawing.Point(597, 128);
            this.GBFunctionalOperation.Name = "GBFunctionalOperation";
            this.GBFunctionalOperation.Size = new System.Drawing.Size(232, 78);
            this.GBFunctionalOperation.TabIndex = 6;
            this.GBFunctionalOperation.TabStop = false;
            this.GBFunctionalOperation.Text = "功能操作";
            // 
            // BtnBurnIn
            // 
            this.BtnBurnIn.Location = new System.Drawing.Point(7, 49);
            this.BtnBurnIn.Margin = new System.Windows.Forms.Padding(2);
            this.BtnBurnIn.Name = "BtnBurnIn";
            this.BtnBurnIn.Size = new System.Drawing.Size(69, 23);
            this.BtnBurnIn.TabIndex = 3;
            this.BtnBurnIn.Text = "燒錄字幕";
            this.BtnBurnIn.UseVisualStyleBackColor = true;
            this.BtnBurnIn.Click += new System.EventHandler(this.BtnBurnIn_Click);
            // 
            // BtnSplitLocalVideo
            // 
            this.BtnSplitLocalVideo.Location = new System.Drawing.Point(156, 21);
            this.BtnSplitLocalVideo.Name = "BtnSplitLocalVideo";
            this.BtnSplitLocalVideo.Size = new System.Drawing.Size(69, 23);
            this.BtnSplitLocalVideo.TabIndex = 2;
            this.BtnSplitLocalVideo.Text = "分割影片";
            this.BtnSplitLocalVideo.UseVisualStyleBackColor = true;
            this.BtnSplitLocalVideo.Click += new System.EventHandler(this.BtnSplitLocalVideo_Click);
            // 
            // BtnUpdateYtDlp
            // 
            this.BtnUpdateYtDlp.Location = new System.Drawing.Point(81, 49);
            this.BtnUpdateYtDlp.Name = "BtnUpdateYtDlp";
            this.BtnUpdateYtDlp.Size = new System.Drawing.Size(69, 23);
            this.BtnUpdateYtDlp.TabIndex = 4;
            this.BtnUpdateYtDlp.Text = "更新";
            this.BtnUpdateYtDlp.UseVisualStyleBackColor = true;
            this.BtnUpdateYtDlp.Click += new System.EventHandler(this.BtnUpdateYtDlp_Click);
            // 
            // BtnFetchVideoInfo
            // 
            this.BtnFetchVideoInfo.Location = new System.Drawing.Point(6, 21);
            this.BtnFetchVideoInfo.Name = "BtnFetchVideoInfo";
            this.BtnFetchVideoInfo.Size = new System.Drawing.Size(69, 23);
            this.BtnFetchVideoInfo.TabIndex = 0;
            this.BtnFetchVideoInfo.Text = "獲取資訊";
            this.BtnFetchVideoInfo.UseVisualStyleBackColor = true;
            this.BtnFetchVideoInfo.Click += new System.EventHandler(this.BtnFetchVideoInfo_Click);
            // 
            // BtnDownloadVideo
            // 
            this.BtnDownloadVideo.Location = new System.Drawing.Point(81, 21);
            this.BtnDownloadVideo.Name = "BtnDownloadVideo";
            this.BtnDownloadVideo.Size = new System.Drawing.Size(69, 23);
            this.BtnDownloadVideo.TabIndex = 1;
            this.BtnDownloadVideo.Text = "下載影片";
            this.BtnDownloadVideo.UseVisualStyleBackColor = true;
            this.BtnDownloadVideo.Click += new System.EventHandler(this.BtnDownloadVideo_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(156, 49);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(69, 23);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "取消作業";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // GBStartTime
            // 
            this.GBStartTime.Controls.Add(this.MTBStartTime);
            this.GBStartTime.Controls.Add(this.TBStartSeconds);
            this.GBStartTime.Location = new System.Drawing.Point(363, 22);
            this.GBStartTime.Name = "GBStartTime";
            this.GBStartTime.Size = new System.Drawing.Size(100, 97);
            this.GBStartTime.TabIndex = 1;
            this.GBStartTime.TabStop = false;
            this.GBStartTime.Text = "開始時間";
            // 
            // MTBStartTime
            // 
            this.MTBStartTime.Location = new System.Drawing.Point(6, 56);
            this.MTBStartTime.Mask = "00:00:00.000";
            this.MTBStartTime.Name = "MTBStartTime";
            this.MTBStartTime.Size = new System.Drawing.Size(88, 23);
            this.MTBStartTime.TabIndex = 1;
            this.MTBStartTime.Text = "000000000";
            // 
            // TBStartSeconds
            // 
            this.TBStartSeconds.Location = new System.Drawing.Point(6, 27);
            this.TBStartSeconds.Name = "TBStartSeconds";
            this.TBStartSeconds.Size = new System.Drawing.Size(88, 23);
            this.TBStartSeconds.TabIndex = 0;
            this.TBStartSeconds.Text = "0.0";
            this.TBStartSeconds.TextChanged += new System.EventHandler(this.TBStartSeconds_TextChanged);
            // 
            // GBEndTime
            // 
            this.GBEndTime.Controls.Add(this.MTBEndtime);
            this.GBEndTime.Controls.Add(this.TBEndSeconds);
            this.GBEndTime.Location = new System.Drawing.Point(469, 22);
            this.GBEndTime.Name = "GBEndTime";
            this.GBEndTime.Size = new System.Drawing.Size(100, 96);
            this.GBEndTime.TabIndex = 2;
            this.GBEndTime.TabStop = false;
            this.GBEndTime.Text = "結束時間";
            // 
            // MTBEndtime
            // 
            this.MTBEndtime.Location = new System.Drawing.Point(6, 56);
            this.MTBEndtime.Mask = "00:00:00.000";
            this.MTBEndtime.Name = "MTBEndtime";
            this.MTBEndtime.Size = new System.Drawing.Size(88, 23);
            this.MTBEndtime.TabIndex = 1;
            this.MTBEndtime.Text = "000000000";
            // 
            // TBEndSeconds
            // 
            this.TBEndSeconds.Location = new System.Drawing.Point(6, 27);
            this.TBEndSeconds.Name = "TBEndSeconds";
            this.TBEndSeconds.Size = new System.Drawing.Size(88, 23);
            this.TBEndSeconds.TabIndex = 0;
            this.TBEndSeconds.Text = "0.0";
            this.TBEndSeconds.TextChanged += new System.EventHandler(this.TBEndSeconds_TextChanged);
            // 
            // GBClipName
            // 
            this.GBClipName.Controls.Add(this.TBClipName);
            this.GBClipName.Location = new System.Drawing.Point(6, 22);
            this.GBClipName.Name = "GBClipName";
            this.GBClipName.Size = new System.Drawing.Size(351, 60);
            this.GBClipName.TabIndex = 0;
            this.GBClipName.TabStop = false;
            this.GBClipName.Text = "短片名稱";
            // 
            // TBClipName
            // 
            this.TBClipName.Location = new System.Drawing.Point(6, 22);
            this.TBClipName.Name = "TBClipName";
            this.TBClipName.Size = new System.Drawing.Size(339, 23);
            this.TBClipName.TabIndex = 0;
            // 
            // GBLoadCookiesFromBrowser
            // 
            this.GBLoadCookiesFromBrowser.Controls.Add(this.CBEnableLoadCookiesFromBrowser);
            this.GBLoadCookiesFromBrowser.Controls.Add(this.TBBrowserProfile);
            this.GBLoadCookiesFromBrowser.Controls.Add(this.CBBrowser);
            this.GBLoadCookiesFromBrowser.Location = new System.Drawing.Point(597, 11);
            this.GBLoadCookiesFromBrowser.Name = "GBLoadCookiesFromBrowser";
            this.GBLoadCookiesFromBrowser.Size = new System.Drawing.Size(232, 110);
            this.GBLoadCookiesFromBrowser.TabIndex = 2;
            this.GBLoadCookiesFromBrowser.TabStop = false;
            this.GBLoadCookiesFromBrowser.Text = "Cookies";
            // 
            // CBEnableLoadCookiesFromBrowser
            // 
            this.CBEnableLoadCookiesFromBrowser.AutoSize = true;
            this.CBEnableLoadCookiesFromBrowser.Location = new System.Drawing.Point(6, 80);
            this.CBEnableLoadCookiesFromBrowser.Name = "CBEnableLoadCookiesFromBrowser";
            this.CBEnableLoadCookiesFromBrowser.Size = new System.Drawing.Size(98, 19);
            this.CBEnableLoadCookiesFromBrowser.TabIndex = 2;
            this.CBEnableLoadCookiesFromBrowser.Text = "載入 Cookies";
            this.CBEnableLoadCookiesFromBrowser.UseVisualStyleBackColor = true;
            this.CBEnableLoadCookiesFromBrowser.CheckedChanged += new System.EventHandler(this.CBEnableLoadCookiesFromBrowser_CheckedChanged);
            // 
            // TBBrowserProfile
            // 
            this.TBBrowserProfile.Location = new System.Drawing.Point(6, 51);
            this.TBBrowserProfile.Name = "TBBrowserProfile";
            this.TBBrowserProfile.Size = new System.Drawing.Size(188, 23);
            this.TBBrowserProfile.TabIndex = 1;
            this.TBBrowserProfile.TextChanged += new System.EventHandler(this.TBBrowserProfile_TextChanged);
            // 
            // CBBrowser
            // 
            this.CBBrowser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBBrowser.FormattingEnabled = true;
            this.CBBrowser.Items.AddRange(new object[] {
            "brave",
            "chrome",
            "chromium",
            "edge",
            "firefox",
            "opera",
            "safari",
            "vivaldi"});
            this.CBBrowser.Location = new System.Drawing.Point(6, 22);
            this.CBBrowser.Name = "CBBrowser";
            this.CBBrowser.Size = new System.Drawing.Size(188, 23);
            this.CBBrowser.TabIndex = 0;
            this.CBBrowser.SelectedIndexChanged += new System.EventHandler(this.CBBrowser_SelectedIndexChanged);
            // 
            // GBFormat
            // 
            this.GBFormat.Controls.Add(this.TBFormatSelection);
            this.GBFormat.Location = new System.Drawing.Point(306, 12);
            this.GBFormat.Name = "GBFormat";
            this.GBFormat.Size = new System.Drawing.Size(285, 60);
            this.GBFormat.TabIndex = 1;
            this.GBFormat.TabStop = false;
            this.GBFormat.Text = "格式";
            // 
            // TBFormatSelection
            // 
            this.TBFormatSelection.Location = new System.Drawing.Point(6, 22);
            this.TBFormatSelection.Name = "TBFormatSelection";
            this.TBFormatSelection.Size = new System.Drawing.Size(273, 23);
            this.TBFormatSelection.TabIndex = 0;
            this.TBFormatSelection.TextChanged += new System.EventHandler(this.TBFormatSelection_TextChanged);
            // 
            // GBOther
            // 
            this.GBOther.Controls.Add(this.CBEnableSplitChapters);
            this.GBOther.Controls.Add(this.CBEnableWaitForVideo);
            this.GBOther.Controls.Add(this.CBEnableLiveFromStart);
            this.GBOther.Controls.Add(this.CBEnableEmbedMeta);
            this.GBOther.Controls.Add(this.CBDownloadFirstSplitLater);
            this.GBOther.Controls.Add(this.CBDeleteSourceFile);
            this.GBOther.Location = new System.Drawing.Point(835, 12);
            this.GBOther.Name = "GBOther";
            this.GBOther.Size = new System.Drawing.Size(130, 194);
            this.GBOther.TabIndex = 3;
            this.GBOther.TabStop = false;
            this.GBOther.Text = "其他";
            // 
            // CBEnableSplitChapters
            // 
            this.CBEnableSplitChapters.AutoSize = true;
            this.CBEnableSplitChapters.Location = new System.Drawing.Point(6, 147);
            this.CBEnableSplitChapters.Name = "CBEnableSplitChapters";
            this.CBEnableSplitChapters.Size = new System.Drawing.Size(110, 19);
            this.CBEnableSplitChapters.TabIndex = 5;
            this.CBEnableSplitChapters.Text = "依影片章節分割";
            this.CBEnableSplitChapters.UseVisualStyleBackColor = true;
            this.CBEnableSplitChapters.CheckedChanged += new System.EventHandler(this.CBEnableSplitChapters_CheckedChanged);
            // 
            // CBEnableWaitForVideo
            // 
            this.CBEnableWaitForVideo.AutoSize = true;
            this.CBEnableWaitForVideo.Location = new System.Drawing.Point(6, 122);
            this.CBEnableWaitForVideo.Name = "CBEnableWaitForVideo";
            this.CBEnableWaitForVideo.Size = new System.Drawing.Size(98, 19);
            this.CBEnableWaitForVideo.TabIndex = 4;
            this.CBEnableWaitForVideo.Text = "等待影片開始";
            this.CBEnableWaitForVideo.UseVisualStyleBackColor = true;
            this.CBEnableWaitForVideo.CheckedChanged += new System.EventHandler(this.CBEnableWaitForVideo_CheckedChanged);
            // 
            // CBEnableLiveFromStart
            // 
            this.CBEnableLiveFromStart.AutoSize = true;
            this.CBEnableLiveFromStart.Location = new System.Drawing.Point(6, 97);
            this.CBEnableLiveFromStart.Name = "CBEnableLiveFromStart";
            this.CBEnableLiveFromStart.Size = new System.Drawing.Size(122, 19);
            this.CBEnableLiveFromStart.TabIndex = 3;
            this.CBEnableLiveFromStart.Text = "從頭開始下載串流";
            this.CBEnableLiveFromStart.UseVisualStyleBackColor = true;
            this.CBEnableLiveFromStart.CheckedChanged += new System.EventHandler(this.CBEnableLiveFromStart_CheckedChanged);
            // 
            // CBEnableEmbedMeta
            // 
            this.CBEnableEmbedMeta.AutoSize = true;
            this.CBEnableEmbedMeta.Location = new System.Drawing.Point(6, 72);
            this.CBEnableEmbedMeta.Name = "CBEnableEmbedMeta";
            this.CBEnableEmbedMeta.Size = new System.Drawing.Size(122, 19);
            this.CBEnableEmbedMeta.TabIndex = 2;
            this.CBEnableEmbedMeta.Text = "影片嵌入後設資料";
            this.CBEnableEmbedMeta.UseVisualStyleBackColor = true;
            this.CBEnableEmbedMeta.CheckedChanged += new System.EventHandler(this.CBEnableEmbedMeta_CheckedChanged);
            // 
            // CBDownloadFirstSplitLater
            // 
            this.CBDownloadFirstSplitLater.AutoSize = true;
            this.CBDownloadFirstSplitLater.Location = new System.Drawing.Point(6, 22);
            this.CBDownloadFirstSplitLater.Name = "CBDownloadFirstSplitLater";
            this.CBDownloadFirstSplitLater.Size = new System.Drawing.Size(122, 19);
            this.CBDownloadFirstSplitLater.TabIndex = 0;
            this.CBDownloadFirstSplitLater.Text = "先下載再分割影片";
            this.CBDownloadFirstSplitLater.UseVisualStyleBackColor = true;
            this.CBDownloadFirstSplitLater.CheckedChanged += new System.EventHandler(this.CBDownloadFirstSplitLater_CheckedChanged);
            // 
            // CBDeleteSourceFile
            // 
            this.CBDeleteSourceFile.AutoSize = true;
            this.CBDeleteSourceFile.Location = new System.Drawing.Point(6, 47);
            this.CBDeleteSourceFile.Name = "CBDeleteSourceFile";
            this.CBDeleteSourceFile.Size = new System.Drawing.Size(122, 19);
            this.CBDeleteSourceFile.TabIndex = 1;
            this.CBDeleteSourceFile.Text = "刪除原始視訊檔案";
            this.CBDeleteSourceFile.UseVisualStyleBackColor = true;
            this.CBDeleteSourceFile.CheckedChanged += new System.EventHandler(this.CBDeleteSourceFile_CheckedChanged);
            // 
            // CBHardwareAcceleratorType
            // 
            this.CBHardwareAcceleratorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBHardwareAcceleratorType.FormattingEnabled = true;
            this.CBHardwareAcceleratorType.Items.AddRange(new object[] {
            "Intel",
            "NVIDIA",
            "AMD"});
            this.CBHardwareAcceleratorType.Location = new System.Drawing.Point(6, 47);
            this.CBHardwareAcceleratorType.Name = "CBHardwareAcceleratorType";
            this.CBHardwareAcceleratorType.Size = new System.Drawing.Size(118, 23);
            this.CBHardwareAcceleratorType.TabIndex = 1;
            this.CBHardwareAcceleratorType.SelectedIndexChanged += new System.EventHandler(this.CBHardwareAcceleratorType_SelectedIndexChanged);
            // 
            // CBUseHardwareAcceleration
            // 
            this.CBUseHardwareAcceleration.AutoSize = true;
            this.CBUseHardwareAcceleration.Location = new System.Drawing.Point(6, 22);
            this.CBUseHardwareAcceleration.Name = "CBUseHardwareAcceleration";
            this.CBUseHardwareAcceleration.Size = new System.Drawing.Size(98, 19);
            this.CBUseHardwareAcceleration.TabIndex = 0;
            this.CBUseHardwareAcceleration.Text = "使用硬體加速";
            this.CBUseHardwareAcceleration.UseVisualStyleBackColor = true;
            this.CBUseHardwareAcceleration.CheckedChanged += new System.EventHandler(this.CBUseHardwareAcceleratation_CheckedChanged);
            // 
            // BtnDlAria2
            // 
            this.BtnDlAria2.Location = new System.Drawing.Point(230, 22);
            this.BtnDlAria2.Name = "BtnDlAria2";
            this.BtnDlAria2.Size = new System.Drawing.Size(60, 23);
            this.BtnDlAria2.TabIndex = 2;
            this.BtnDlAria2.Text = "aria2";
            this.BtnDlAria2.UseVisualStyleBackColor = true;
            this.BtnDlAria2.Click += new System.EventHandler(this.BtnDlAria2_Click);
            // 
            // BtnDlFFmpeg
            // 
            this.BtnDlFFmpeg.Location = new System.Drawing.Point(72, 22);
            this.BtnDlFFmpeg.Name = "BtnDlFFmpeg";
            this.BtnDlFFmpeg.Size = new System.Drawing.Size(152, 23);
            this.BtnDlFFmpeg.TabIndex = 1;
            this.BtnDlFFmpeg.Text = "yt-dlp/FFmpeg-Builds";
            this.BtnDlFFmpeg.UseVisualStyleBackColor = true;
            this.BtnDlFFmpeg.Click += new System.EventHandler(this.BtnDlFFmpeg_Click);
            // 
            // BtnOpenDownloadFolder
            // 
            this.BtnOpenDownloadFolder.Location = new System.Drawing.Point(90, 22);
            this.BtnOpenDownloadFolder.Name = "BtnOpenDownloadFolder";
            this.BtnOpenDownloadFolder.Size = new System.Drawing.Size(78, 23);
            this.BtnOpenDownloadFolder.TabIndex = 1;
            this.BtnOpenDownloadFolder.Text = "下載資料夾";
            this.BtnOpenDownloadFolder.UseVisualStyleBackColor = true;
            this.BtnOpenDownloadFolder.Click += new System.EventHandler(this.BtnOpenDownloadFolder_Click);
            // 
            // BtnDlYtDlp
            // 
            this.BtnDlYtDlp.Location = new System.Drawing.Point(6, 22);
            this.BtnDlYtDlp.Name = "BtnDlYtDlp";
            this.BtnDlYtDlp.Size = new System.Drawing.Size(60, 23);
            this.BtnDlYtDlp.TabIndex = 0;
            this.BtnDlYtDlp.Text = "yt-dlp";
            this.BtnDlYtDlp.UseVisualStyleBackColor = true;
            this.BtnDlYtDlp.Click += new System.EventHandler(this.BtnDlYtDlp_Click);
            // 
            // LYtDlpVer
            // 
            this.LYtDlpVer.AutoSize = true;
            this.LYtDlpVer.Location = new System.Drawing.Point(98, 612);
            this.LYtDlpVer.Name = "LYtDlpVer";
            this.LYtDlpVer.Size = new System.Drawing.Size(80, 15);
            this.LYtDlpVer.TabIndex = 14;
            this.LYtDlpVer.Text = "yt-dlp 版本：";
            // 
            // LVersion
            // 
            this.LVersion.AutoSize = true;
            this.LVersion.Location = new System.Drawing.Point(12, 612);
            this.LVersion.Name = "LVersion";
            this.LVersion.Size = new System.Drawing.Size(80, 15);
            this.LVersion.TabIndex = 13;
            this.LVersion.Text = "版本：0.0.0.0";
            // 
            // GBNewClip
            // 
            this.GBNewClip.Controls.Add(this.CBIsAudioOnly);
            this.GBNewClip.Controls.Add(this.BtnAddNewClip);
            this.GBNewClip.Controls.Add(this.BtnResetNewClip);
            this.GBNewClip.Controls.Add(this.GBClipName);
            this.GBNewClip.Controls.Add(this.GBStartTime);
            this.GBNewClip.Controls.Add(this.GBEndTime);
            this.GBNewClip.Location = new System.Drawing.Point(12, 78);
            this.GBNewClip.Name = "GBNewClip";
            this.GBNewClip.Size = new System.Drawing.Size(579, 124);
            this.GBNewClip.TabIndex = 5;
            this.GBNewClip.TabStop = false;
            this.GBNewClip.Text = "新短片";
            // 
            // CBIsAudioOnly
            // 
            this.CBIsAudioOnly.AutoSize = true;
            this.CBIsAudioOnly.Location = new System.Drawing.Point(295, 98);
            this.CBIsAudioOnly.Name = "CBIsAudioOnly";
            this.CBIsAudioOnly.Size = new System.Drawing.Size(62, 19);
            this.CBIsAudioOnly.TabIndex = 3;
            this.CBIsAudioOnly.Text = "僅音訊";
            this.CBIsAudioOnly.UseVisualStyleBackColor = true;
            // 
            // BtnAddNewClip
            // 
            this.BtnAddNewClip.Location = new System.Drawing.Point(220, 94);
            this.BtnAddNewClip.Name = "BtnAddNewClip";
            this.BtnAddNewClip.Size = new System.Drawing.Size(69, 23);
            this.BtnAddNewClip.TabIndex = 4;
            this.BtnAddNewClip.Text = "新增";
            this.BtnAddNewClip.UseVisualStyleBackColor = true;
            this.BtnAddNewClip.Click += new System.EventHandler(this.BtnAddNewClip_Click);
            // 
            // BtnResetNewClip
            // 
            this.BtnResetNewClip.Location = new System.Drawing.Point(6, 95);
            this.BtnResetNewClip.Name = "BtnResetNewClip";
            this.BtnResetNewClip.Size = new System.Drawing.Size(69, 23);
            this.BtnResetNewClip.TabIndex = 5;
            this.BtnResetNewClip.Text = "重置";
            this.BtnResetNewClip.UseVisualStyleBackColor = true;
            this.BtnResetNewClip.Click += new System.EventHandler(this.BtnResetNewClip_Click);
            // 
            // GBClip
            // 
            this.GBClip.Controls.Add(this.TBAutoEndSeconds);
            this.GBClip.Controls.Add(this.DgvClipList);
            this.GBClip.Controls.Add(this.BtnClearClipList);
            this.GBClip.Controls.Add(this.BtnImportTimestamp);
            this.GBClip.Location = new System.Drawing.Point(12, 304);
            this.GBClip.Name = "GBClip";
            this.GBClip.Size = new System.Drawing.Size(579, 298);
            this.GBClip.TabIndex = 11;
            this.GBClip.TabStop = false;
            this.GBClip.Text = "短片清單";
            // 
            // TBAutoEndSeconds
            // 
            this.TBAutoEndSeconds.Location = new System.Drawing.Point(383, 22);
            this.TBAutoEndSeconds.Name = "TBAutoEndSeconds";
            this.TBAutoEndSeconds.Size = new System.Drawing.Size(190, 23);
            this.TBAutoEndSeconds.TabIndex = 3;
            this.TBAutoEndSeconds.TextChanged += new System.EventHandler(this.TBAutoEndSeconds_TextChanged);
            // 
            // DgvClipList
            // 
            this.DgvClipList.AllowDrop = true;
            this.DgvClipList.AllowUserToAddRows = false;
            this.DgvClipList.AllowUserToOrderColumns = true;
            this.DgvClipList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvClipList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClipNo,
            this.ClipName,
            this.StartSeconds,
            this.EndSeconds,
            this.IsAudioOnly});
            this.DgvClipList.Location = new System.Drawing.Point(6, 51);
            this.DgvClipList.Name = "DgvClipList";
            this.DgvClipList.RowHeadersWidth = 51;
            this.DgvClipList.RowTemplate.Height = 25;
            this.DgvClipList.Size = new System.Drawing.Size(567, 241);
            this.DgvClipList.TabIndex = 2;
            this.DgvClipList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DgvClipList_CellValidating);
            this.DgvClipList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DgvClipList_DataError);
            this.DgvClipList.DragDrop += new System.Windows.Forms.DragEventHandler(this.DgvClipList_DragDrop);
            this.DgvClipList.DragEnter += new System.Windows.Forms.DragEventHandler(this.DgvClipList_DragEnter);
            // 
            // ClipNo
            // 
            this.ClipNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ClipNo.DataPropertyName = "ClipNo";
            this.ClipNo.HeaderText = "短片編號";
            this.ClipNo.MinimumWidth = 6;
            this.ClipNo.Name = "ClipNo";
            this.ClipNo.ToolTipText = "短片編號";
            this.ClipNo.Width = 80;
            // 
            // ClipName
            // 
            this.ClipName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClipName.DataPropertyName = "ClipName";
            this.ClipName.HeaderText = "短片名稱";
            this.ClipName.MinimumWidth = 6;
            this.ClipName.Name = "ClipName";
            this.ClipName.ToolTipText = "短片名稱";
            // 
            // StartSeconds
            // 
            this.StartSeconds.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.StartSeconds.DataPropertyName = "StartSeconds";
            this.StartSeconds.HeaderText = "開始秒數";
            this.StartSeconds.MinimumWidth = 6;
            this.StartSeconds.Name = "StartSeconds";
            this.StartSeconds.ToolTipText = "開始秒數";
            this.StartSeconds.Width = 80;
            // 
            // EndSeconds
            // 
            this.EndSeconds.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.EndSeconds.DataPropertyName = "EndSeconds";
            this.EndSeconds.HeaderText = "結束秒數";
            this.EndSeconds.MinimumWidth = 6;
            this.EndSeconds.Name = "EndSeconds";
            this.EndSeconds.ToolTipText = "結束秒數";
            this.EndSeconds.Width = 80;
            // 
            // IsAudioOnly
            // 
            this.IsAudioOnly.DataPropertyName = "IsAudioOnly";
            this.IsAudioOnly.HeaderText = "僅音訊";
            this.IsAudioOnly.Name = "IsAudioOnly";
            this.IsAudioOnly.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsAudioOnly.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsAudioOnly.ToolTipText = "僅音訊";
            this.IsAudioOnly.Width = 70;
            // 
            // BtnClearClipList
            // 
            this.BtnClearClipList.Location = new System.Drawing.Point(110, 22);
            this.BtnClearClipList.Name = "BtnClearClipList";
            this.BtnClearClipList.Size = new System.Drawing.Size(98, 23);
            this.BtnClearClipList.TabIndex = 1;
            this.BtnClearClipList.Text = "清除短片清單";
            this.BtnClearClipList.UseVisualStyleBackColor = true;
            this.BtnClearClipList.Click += new System.EventHandler(this.BtnClearClipList_Click);
            // 
            // BtnImportTimestamp
            // 
            this.BtnImportTimestamp.Location = new System.Drawing.Point(6, 22);
            this.BtnImportTimestamp.Name = "BtnImportTimestamp";
            this.BtnImportTimestamp.Size = new System.Drawing.Size(98, 23);
            this.BtnImportTimestamp.TabIndex = 0;
            this.BtnImportTimestamp.Text = "匯入時間標記";
            this.BtnImportTimestamp.UseVisualStyleBackColor = true;
            this.BtnImportTimestamp.Click += new System.EventHandler(this.BtnImportTimestamp_Click);
            // 
            // GBDownloadExternalProgram
            // 
            this.GBDownloadExternalProgram.Controls.Add(this.BtnDlAria2);
            this.GBDownloadExternalProgram.Controls.Add(this.BtnDlYtDlp);
            this.GBDownloadExternalProgram.Controls.Add(this.BtnDlFFmpeg);
            this.GBDownloadExternalProgram.Location = new System.Drawing.Point(12, 208);
            this.GBDownloadExternalProgram.Name = "GBDownloadExternalProgram";
            this.GBDownloadExternalProgram.Size = new System.Drawing.Size(296, 90);
            this.GBDownloadExternalProgram.TabIndex = 8;
            this.GBDownloadExternalProgram.TabStop = false;
            this.GBDownloadExternalProgram.Text = "下載外部程式";
            // 
            // GBHyperlink
            // 
            this.GBHyperlink.Controls.Add(this.LLTwitCasting);
            this.GBHyperlink.Controls.Add(this.LLbilibili);
            this.GBHyperlink.Controls.Add(this.LLTwitch);
            this.GBHyperlink.Controls.Add(this.LLYouTube);
            this.GBHyperlink.Controls.Add(this.LLAria2);
            this.GBHyperlink.Controls.Add(this.LLYtDlpFFmpeg);
            this.GBHyperlink.Controls.Add(this.LLYtDlp);
            this.GBHyperlink.Location = new System.Drawing.Point(597, 212);
            this.GBHyperlink.Name = "GBHyperlink";
            this.GBHyperlink.Size = new System.Drawing.Size(368, 86);
            this.GBHyperlink.TabIndex = 10;
            this.GBHyperlink.TabStop = false;
            this.GBHyperlink.Text = "相關連結";
            // 
            // LLTwitCasting
            // 
            this.LLTwitCasting.AutoSize = true;
            this.LLTwitCasting.Location = new System.Drawing.Point(166, 38);
            this.LLTwitCasting.Name = "LLTwitCasting";
            this.LLTwitCasting.Size = new System.Drawing.Size(72, 15);
            this.LLTwitCasting.TabIndex = 6;
            this.LLTwitCasting.TabStop = true;
            this.LLTwitCasting.Text = "TwitCasting";
            this.LLTwitCasting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLTwitCasting_LinkClicked);
            // 
            // LLbilibili
            // 
            this.LLbilibili.AutoSize = true;
            this.LLbilibili.Location = new System.Drawing.Point(119, 38);
            this.LLbilibili.Name = "LLbilibili";
            this.LLbilibili.Size = new System.Drawing.Size(41, 15);
            this.LLbilibili.TabIndex = 5;
            this.LLbilibili.TabStop = true;
            this.LLbilibili.Text = "bilibili";
            this.LLbilibili.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLbilibili_LinkClicked);
            // 
            // LLTwitch
            // 
            this.LLTwitch.AutoSize = true;
            this.LLTwitch.Location = new System.Drawing.Point(70, 38);
            this.LLTwitch.Name = "LLTwitch";
            this.LLTwitch.Size = new System.Drawing.Size(43, 15);
            this.LLTwitch.TabIndex = 4;
            this.LLTwitch.TabStop = true;
            this.LLTwitch.Text = "Twitch";
            this.LLTwitch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLTwitch_LinkClicked);
            // 
            // LLYouTube
            // 
            this.LLYouTube.AutoSize = true;
            this.LLYouTube.Location = new System.Drawing.Point(7, 38);
            this.LLYouTube.Name = "LLYouTube";
            this.LLYouTube.Size = new System.Drawing.Size(58, 15);
            this.LLYouTube.TabIndex = 3;
            this.LLYouTube.TabStop = true;
            this.LLYouTube.Text = "YouTube";
            this.LLYouTube.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYouTube_LinkClicked);
            // 
            // LLAria2
            // 
            this.LLAria2.AutoSize = true;
            this.LLAria2.Location = new System.Drawing.Point(189, 18);
            this.LLAria2.Name = "LLAria2";
            this.LLAria2.Size = new System.Drawing.Size(35, 15);
            this.LLAria2.TabIndex = 2;
            this.LLAria2.TabStop = true;
            this.LLAria2.Text = "aria2";
            this.LLAria2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLAria2_LinkClicked);
            // 
            // LLYtDlpFFmpeg
            // 
            this.LLYtDlpFFmpeg.AutoSize = true;
            this.LLYtDlpFFmpeg.Location = new System.Drawing.Point(53, 18);
            this.LLYtDlpFFmpeg.Name = "LLYtDlpFFmpeg";
            this.LLYtDlpFFmpeg.Size = new System.Drawing.Size(130, 15);
            this.LLYtDlpFFmpeg.TabIndex = 1;
            this.LLYtDlpFFmpeg.TabStop = true;
            this.LLYtDlpFFmpeg.Text = "yt-dlp/FFmpeg-Builds";
            this.LLYtDlpFFmpeg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYtDlpFFmpeg_LinkClicked);
            // 
            // LLYtDlp
            // 
            this.LLYtDlp.AutoSize = true;
            this.LLYtDlp.Location = new System.Drawing.Point(6, 18);
            this.LLYtDlp.Name = "LLYtDlp";
            this.LLYtDlp.Size = new System.Drawing.Size(41, 15);
            this.LLYtDlp.TabIndex = 0;
            this.LLYtDlp.TabStop = true;
            this.LLYtDlp.Text = "yt-dlp";
            this.LLYtDlp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYtDlp_LinkClicked);
            // 
            // GBFFmpegUseHardwareAcceleration
            // 
            this.GBFFmpegUseHardwareAcceleration.Controls.Add(this.CBDeviceNo);
            this.GBFFmpegUseHardwareAcceleration.Controls.Add(this.CBUseHardwareAcceleration);
            this.GBFFmpegUseHardwareAcceleration.Controls.Add(this.CBHardwareAcceleratorType);
            this.GBFFmpegUseHardwareAcceleration.Location = new System.Drawing.Point(970, 176);
            this.GBFFmpegUseHardwareAcceleration.Name = "GBFFmpegUseHardwareAcceleration";
            this.GBFFmpegUseHardwareAcceleration.Size = new System.Drawing.Size(128, 122);
            this.GBFFmpegUseHardwareAcceleration.TabIndex = 7;
            this.GBFFmpegUseHardwareAcceleration.TabStop = false;
            this.GBFFmpegUseHardwareAcceleration.Text = "FFmpeg 硬體加速";
            // 
            // CBDeviceNo
            // 
            this.CBDeviceNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBDeviceNo.FormattingEnabled = true;
            this.CBDeviceNo.Location = new System.Drawing.Point(6, 76);
            this.CBDeviceNo.Name = "CBDeviceNo";
            this.CBDeviceNo.Size = new System.Drawing.Size(118, 23);
            this.CBDeviceNo.TabIndex = 2;
            this.CBDeviceNo.SelectedIndexChanged += new System.EventHandler(this.CBDeviceNo_SelectedIndexChanged);
            // 
            // GBOpenFolder
            // 
            this.GBOpenFolder.Controls.Add(this.BtnOpenLogsFolder);
            this.GBOpenFolder.Controls.Add(this.BtnOpenConfigFolder);
            this.GBOpenFolder.Controls.Add(this.BtnOpenBinFolder);
            this.GBOpenFolder.Controls.Add(this.BtnOpenDownloadFolder);
            this.GBOpenFolder.Location = new System.Drawing.Point(312, 208);
            this.GBOpenFolder.Name = "GBOpenFolder";
            this.GBOpenFolder.Size = new System.Drawing.Size(279, 90);
            this.GBOpenFolder.TabIndex = 9;
            this.GBOpenFolder.TabStop = false;
            this.GBOpenFolder.Text = "開啟資料夾";
            // 
            // BtnOpenLogsFolder
            // 
            this.BtnOpenLogsFolder.Location = new System.Drawing.Point(6, 51);
            this.BtnOpenLogsFolder.Name = "BtnOpenLogsFolder";
            this.BtnOpenLogsFolder.Size = new System.Drawing.Size(78, 23);
            this.BtnOpenLogsFolder.TabIndex = 3;
            this.BtnOpenLogsFolder.Text = "Logs 資料夾";
            this.BtnOpenLogsFolder.UseVisualStyleBackColor = true;
            this.BtnOpenLogsFolder.Click += new System.EventHandler(this.BtnOpenLogsFolder_Click);
            // 
            // BtnOpenConfigFolder
            // 
            this.BtnOpenConfigFolder.Location = new System.Drawing.Point(174, 22);
            this.BtnOpenConfigFolder.Name = "BtnOpenConfigFolder";
            this.BtnOpenConfigFolder.Size = new System.Drawing.Size(90, 23);
            this.BtnOpenConfigFolder.TabIndex = 2;
            this.BtnOpenConfigFolder.Text = "設定檔資料夾";
            this.BtnOpenConfigFolder.UseVisualStyleBackColor = true;
            this.BtnOpenConfigFolder.Click += new System.EventHandler(this.BtnOpenConfigFolder_Click);
            // 
            // BtnOpenBinFolder
            // 
            this.BtnOpenBinFolder.Location = new System.Drawing.Point(6, 22);
            this.BtnOpenBinFolder.Name = "BtnOpenBinFolder";
            this.BtnOpenBinFolder.Size = new System.Drawing.Size(78, 23);
            this.BtnOpenBinFolder.TabIndex = 0;
            this.BtnOpenBinFolder.Text = "Bin 資料夾";
            this.BtnOpenBinFolder.UseVisualStyleBackColor = true;
            this.BtnOpenBinFolder.Click += new System.EventHandler(this.BtnOpenBinFolder_Click);
            // 
            // BtnOpenSubtitleCreator
            // 
            this.BtnOpenSubtitleCreator.Location = new System.Drawing.Point(787, 608);
            this.BtnOpenSubtitleCreator.Name = "BtnOpenSubtitleCreator";
            this.BtnOpenSubtitleCreator.Size = new System.Drawing.Size(124, 23);
            this.BtnOpenSubtitleCreator.TabIndex = 15;
            this.BtnOpenSubtitleCreator.Text = "開啟陽春字幕產生器";
            this.BtnOpenSubtitleCreator.UseVisualStyleBackColor = true;
            this.BtnOpenSubtitleCreator.Click += new System.EventHandler(this.BtnOpenSubtitleCreator_Click);
            // 
            // BtnOpenYTSecConverter
            // 
            this.BtnOpenYTSecConverter.Location = new System.Drawing.Point(917, 608);
            this.BtnOpenYTSecConverter.Name = "BtnOpenYTSecConverter";
            this.BtnOpenYTSecConverter.Size = new System.Drawing.Size(182, 23);
            this.BtnOpenYTSecConverter.TabIndex = 16;
            this.BtnOpenYTSecConverter.Text = "開啟 YouTube 影片秒數轉換器";
            this.BtnOpenYTSecConverter.UseVisualStyleBackColor = true;
            this.BtnOpenYTSecConverter.Click += new System.EventHandler(this.BtnOpenYTSecConverter_Click);
            // 
            // GBSubtitleOption
            // 
            this.GBSubtitleOption.Controls.Add(this.CBApplyFontSetting);
            this.GBSubtitleOption.Controls.Add(this.TBCustomEncode);
            this.GBSubtitleOption.Controls.Add(this.CBEncode);
            this.GBSubtitleOption.Controls.Add(this.TBCustomFont);
            this.GBSubtitleOption.Controls.Add(this.CBFont);
            this.GBSubtitleOption.Location = new System.Drawing.Point(970, 11);
            this.GBSubtitleOption.Margin = new System.Windows.Forms.Padding(2);
            this.GBSubtitleOption.Name = "GBSubtitleOption";
            this.GBSubtitleOption.Padding = new System.Windows.Forms.Padding(2);
            this.GBSubtitleOption.Size = new System.Drawing.Size(129, 160);
            this.GBSubtitleOption.TabIndex = 4;
            this.GBSubtitleOption.TabStop = false;
            this.GBSubtitleOption.Text = "字幕檔設定";
            // 
            // CBApplyFontSetting
            // 
            this.CBApplyFontSetting.AutoSize = true;
            this.CBApplyFontSetting.Location = new System.Drawing.Point(4, 20);
            this.CBApplyFontSetting.Name = "CBApplyFontSetting";
            this.CBApplyFontSetting.Size = new System.Drawing.Size(98, 19);
            this.CBApplyFontSetting.TabIndex = 0;
            this.CBApplyFontSetting.Text = "套用字型設定";
            this.CBApplyFontSetting.UseVisualStyleBackColor = true;
            // 
            // TBCustomEncode
            // 
            this.TBCustomEncode.Location = new System.Drawing.Point(5, 126);
            this.TBCustomEncode.Margin = new System.Windows.Forms.Padding(2);
            this.TBCustomEncode.Name = "TBCustomEncode";
            this.TBCustomEncode.PlaceholderText = "（自定義文字編碼）";
            this.TBCustomEncode.Size = new System.Drawing.Size(118, 23);
            this.TBCustomEncode.TabIndex = 4;
            // 
            // CBEncode
            // 
            this.CBEncode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBEncode.FormattingEnabled = true;
            this.CBEncode.Items.AddRange(new object[] {
            "850",
            "862",
            "866",
            "ANSI_X3.4-1968",
            "ANSI_X3.4-1986",
            "ARABIC",
            "ARMSCII-8",
            "ASCII",
            "ASMO-708",
            "BIG-5",
            "BIG-FIVE",
            "BIG5",
            "BIG5-HKSCS",
            "BIG5-HKSCS:1999",
            "BIG5-HKSCS:2001",
            "BIG5-HKSCS:2004",
            "BIG5-HKSCS:2008",
            "BIG5HKSCS",
            "BIGFIVE",
            "C99",
            "CHAR",
            "CHINESE",
            "CN",
            "CN-BIG5",
            "CN-GB",
            "CN-GB-ISOIR165",
            "CP1131",
            "CP1133",
            "CP1250",
            "CP1251",
            "CP1252",
            "CP1253",
            "CP1254",
            "CP1255",
            "CP1256",
            "CP1257",
            "CP1258",
            "CP1361",
            "CP154",
            "CP367",
            "CP819",
            "CP850",
            "CP862",
            "CP866",
            "CP874",
            "CP932",
            "CP936",
            "CP949",
            "CP950",
            "CSASCII",
            "CSBIG5",
            "CSEUCKR",
            "CSEUCPKDFMTJAPANESE",
            "CSEUCTW",
            "CSGB2312",
            "CSHALFWIDTHKATAKANA",
            "CSHPROMAN8",
            "CSIBM866",
            "CSISO14JISC6220RO",
            "CSISO159JISX02121990",
            "CSISO2022CN",
            "CSISO2022JP",
            "CSISO2022JP2",
            "CSISO2022KR",
            "CSISO57GB1988",
            "CSISO58GB231280",
            "CSISO87JISX0208",
            "CSISOLATIN1",
            "CSISOLATIN2",
            "CSISOLATIN3",
            "CSISOLATIN4",
            "CSISOLATIN5",
            "CSISOLATIN6",
            "CSISOLATINARABIC",
            "CSISOLATINCYRILLIC",
            "CSISOLATINGREEK",
            "CSISOLATINHEBREW",
            "CSKOI8R",
            "CSKSC56011987",
            "CSKZ1048",
            "CSMACINTOSH",
            "CSPC850MULTILINGUAL",
            "CSPC862LATINHEBREW",
            "CSPTCP154",
            "CSSHIFTJIS",
            "CSUCS4",
            "CSUNICODE",
            "CSUNICODE11",
            "CSUNICODE11UTF7",
            "CSVISCII",
            "CYRILLIC",
            "CYRILLIC-ASIAN",
            "ECMA-114",
            "ECMA-118",
            "ELOT_928",
            "EUC-CN",
            "EUC-JP",
            "EUC-KR",
            "EUC-TW",
            "EUCCN",
            "EUCJP",
            "EUCKR",
            "EUCTW",
            "EXTENDED_UNIX_CODE_PACKED_FORMAT_FOR_JAPANESE",
            "GB_1988-80",
            "GB_2312-80",
            "GB18030",
            "GB2312",
            "GBK",
            "GEORGIAN-ACADEMY",
            "GEORGIAN-PS",
            "GREEK",
            "GREEK8",
            "HEBREW",
            "HP-ROMAN8",
            "HZ",
            "HZ-GB-2312",
            "IBM-CP1133",
            "IBM367",
            "IBM819",
            "IBM850",
            "IBM862",
            "IBM866",
            "ISO-10646-UCS-2",
            "ISO-10646-UCS-4",
            "ISO-2022-CN",
            "ISO-2022-CN-EXT",
            "ISO-2022-JP",
            "ISO-2022-JP-1",
            "ISO-2022-JP-2",
            "ISO-2022-KR",
            "ISO-8859-1",
            "ISO-8859-10",
            "ISO-8859-11",
            "ISO-8859-13",
            "ISO-8859-14",
            "ISO-8859-15",
            "ISO-8859-16",
            "ISO-8859-2",
            "ISO-8859-3",
            "ISO-8859-4",
            "ISO-8859-5",
            "ISO-8859-6",
            "ISO-8859-7",
            "ISO-8859-8",
            "ISO-8859-9",
            "ISO-CELTIC",
            "ISO-IR-100",
            "ISO-IR-101",
            "ISO-IR-109",
            "ISO-IR-110",
            "ISO-IR-126",
            "ISO-IR-127",
            "ISO-IR-138",
            "ISO-IR-14",
            "ISO-IR-144",
            "ISO-IR-148",
            "ISO-IR-149",
            "ISO-IR-157",
            "ISO-IR-159",
            "ISO-IR-165",
            "ISO-IR-166",
            "ISO-IR-179",
            "ISO-IR-199",
            "ISO-IR-203",
            "ISO-IR-226",
            "ISO-IR-57",
            "ISO-IR-58",
            "ISO-IR-6",
            "ISO-IR-87",
            "ISO_646.IRV:1991",
            "ISO_8859-1",
            "ISO_8859-1:1987",
            "ISO_8859-10",
            "ISO_8859-10:1992",
            "ISO_8859-11",
            "ISO_8859-13",
            "ISO_8859-14",
            "ISO_8859-14:1998",
            "ISO_8859-15",
            "ISO_8859-15:1998",
            "ISO_8859-16",
            "ISO_8859-16:2001",
            "ISO_8859-2",
            "ISO_8859-2:1987",
            "ISO_8859-3",
            "ISO_8859-3:1988",
            "ISO_8859-4",
            "ISO_8859-4:1988",
            "ISO_8859-5",
            "ISO_8859-5:1988",
            "ISO_8859-6",
            "ISO_8859-6:1987",
            "ISO_8859-7",
            "ISO_8859-7:1987",
            "ISO_8859-7:2003",
            "ISO_8859-8",
            "ISO_8859-8:1988",
            "ISO_8859-9",
            "ISO_8859-9:1989",
            "ISO646-CN",
            "ISO646-JP",
            "ISO646-US",
            "ISO8859-1",
            "ISO8859-10",
            "ISO8859-11",
            "ISO8859-13",
            "ISO8859-14",
            "ISO8859-15",
            "ISO8859-16",
            "ISO8859-2",
            "ISO8859-3",
            "ISO8859-4",
            "ISO8859-5",
            "ISO8859-6",
            "ISO8859-7",
            "ISO8859-8",
            "ISO8859-9",
            "JAVA",
            "JIS_C6220-1969-RO",
            "JIS_C6226-1983",
            "JIS_X0201",
            "JIS_X0208",
            "JIS_X0208-1983",
            "JIS_X0208-1990",
            "JIS_X0212",
            "JIS_X0212-1990",
            "JIS_X0212.1990-0",
            "JIS0208",
            "JISX0201-1976",
            "JOHAB",
            "JP",
            "KOI8-R",
            "KOI8-RU",
            "KOI8-T",
            "KOI8-U",
            "KOREAN",
            "KS_C_5601-1987",
            "KS_C_5601-1989",
            "KSC_5601",
            "KZ-1048",
            "L1",
            "L10",
            "L2",
            "L3",
            "L4",
            "L5",
            "L6",
            "L7",
            "L8",
            "LATIN-9",
            "LATIN1",
            "LATIN10",
            "LATIN2",
            "LATIN3",
            "LATIN4",
            "LATIN5",
            "LATIN6",
            "LATIN7",
            "LATIN8",
            "MAC",
            "MACARABIC",
            "MACCENTRALEUROPE",
            "MACCROATIAN",
            "MACCYRILLIC",
            "MACGREEK",
            "MACHEBREW",
            "MACICELAND",
            "MACINTOSH",
            "MACROMAN",
            "MACROMANIA",
            "MACTHAI",
            "MACTURKISH",
            "MACUKRAINE",
            "MS-ANSI",
            "MS-ARAB",
            "MS-CYRL",
            "MS-EE",
            "MS-GREEK",
            "MS-HEBR",
            "MS-TURK",
            "MS_KANJI",
            "MS936",
            "MULELAO-1",
            "NEXTSTEP",
            "PT154",
            "PTCP154",
            "R8",
            "RK1048",
            "ROMAN8",
            "SHIFT-JIS",
            "SHIFT_JIS",
            "SJIS",
            "STRK1048-2002",
            "TCVN",
            "TCVN-5712",
            "TCVN5712-1",
            "TCVN5712-1:1993",
            "TIS-620",
            "TIS620",
            "TIS620-0",
            "TIS620.2529-1",
            "TIS620.2533-0",
            "TIS620.2533-1",
            "UCS-2",
            "UCS-2-INTERNAL",
            "UCS-2-SWAPPED",
            "UCS-2BE",
            "UCS-2LE",
            "UCS-4",
            "UCS-4-INTERNAL",
            "UCS-4-SWAPPED",
            "UCS-4BE",
            "UCS-4LE",
            "UHC",
            "UNICODE-1-1",
            "UNICODE-1-1-UTF-7",
            "UNICODEBIG",
            "UNICODELITTLE",
            "US",
            "US-ASCII",
            "UTF-16",
            "UTF-16BE",
            "UTF-16LE",
            "UTF-32",
            "UTF-32BE",
            "UTF-32LE",
            "UTF-7",
            "UTF-8",
            "VISCII",
            "VISCII1.1-1",
            "WCHAR_T",
            "WINBALTRIM",
            "WINDOWS-1250",
            "WINDOWS-1251",
            "WINDOWS-1252",
            "WINDOWS-1253",
            "WINDOWS-1254",
            "WINDOWS-1255",
            "WINDOWS-1256",
            "WINDOWS-1257",
            "WINDOWS-1258",
            "WINDOWS-874",
            "WINDOWS-936",
            "X0201",
            "X0208",
            "X0212"});
            this.CBEncode.Location = new System.Drawing.Point(5, 99);
            this.CBEncode.Margin = new System.Windows.Forms.Padding(2);
            this.CBEncode.Name = "CBEncode";
            this.CBEncode.Size = new System.Drawing.Size(118, 23);
            this.CBEncode.TabIndex = 3;
            // 
            // TBCustomFont
            // 
            this.TBCustomFont.Location = new System.Drawing.Point(5, 72);
            this.TBCustomFont.Margin = new System.Windows.Forms.Padding(2);
            this.TBCustomFont.Name = "TBCustomFont";
            this.TBCustomFont.PlaceholderText = "（自定義字型名稱）";
            this.TBCustomFont.Size = new System.Drawing.Size(118, 23);
            this.TBCustomFont.TabIndex = 2;
            // 
            // CBFont
            // 
            this.CBFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBFont.FormattingEnabled = true;
            this.CBFont.Location = new System.Drawing.Point(5, 45);
            this.CBFont.Margin = new System.Windows.Forms.Padding(2);
            this.CBFont.Name = "CBFont";
            this.CBFont.Size = new System.Drawing.Size(118, 23);
            this.CBFont.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1114, 641);
            this.Controls.Add(this.GBSubtitleOption);
            this.Controls.Add(this.BtnOpenYTSecConverter);
            this.Controls.Add(this.BtnOpenSubtitleCreator);
            this.Controls.Add(this.GBOpenFolder);
            this.Controls.Add(this.GBFFmpegUseHardwareAcceleration);
            this.Controls.Add(this.GBHyperlink);
            this.Controls.Add(this.GBDownloadExternalProgram);
            this.Controls.Add(this.GBClip);
            this.Controls.Add(this.GBNewClip);
            this.Controls.Add(this.LVersion);
            this.Controls.Add(this.GBFormat);
            this.Controls.Add(this.LYtDlpVer);
            this.Controls.Add(this.GBUrl);
            this.Controls.Add(this.GBOther);
            this.Controls.Add(this.GBLoadCookiesFromBrowser);
            this.Controls.Add(this.GBFunctionalOperation);
            this.Controls.Add(this.GBLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "YtDlpFront";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.GBUrl.ResumeLayout(false);
            this.GBUrl.PerformLayout();
            this.GBLog.ResumeLayout(false);
            this.GBFunctionalOperation.ResumeLayout(false);
            this.GBStartTime.ResumeLayout(false);
            this.GBStartTime.PerformLayout();
            this.GBEndTime.ResumeLayout(false);
            this.GBEndTime.PerformLayout();
            this.GBClipName.ResumeLayout(false);
            this.GBClipName.PerformLayout();
            this.GBLoadCookiesFromBrowser.ResumeLayout(false);
            this.GBLoadCookiesFromBrowser.PerformLayout();
            this.GBFormat.ResumeLayout(false);
            this.GBFormat.PerformLayout();
            this.GBOther.ResumeLayout(false);
            this.GBOther.PerformLayout();
            this.GBNewClip.ResumeLayout(false);
            this.GBNewClip.PerformLayout();
            this.GBClip.ResumeLayout(false);
            this.GBClip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvClipList)).EndInit();
            this.GBDownloadExternalProgram.ResumeLayout(false);
            this.GBHyperlink.ResumeLayout(false);
            this.GBHyperlink.PerformLayout();
            this.GBFFmpegUseHardwareAcceleration.ResumeLayout(false);
            this.GBFFmpegUseHardwareAcceleration.PerformLayout();
            this.GBOpenFolder.ResumeLayout(false);
            this.GBSubtitleOption.ResumeLayout(false);
            this.GBSubtitleOption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox GBUrl;
        private TextBox TBUrl;
        private GroupBox GBLog;
        private GroupBox GBFunctionalOperation;
        private Button BtnCancel;
        private Button BtnDownloadVideo;
        private GroupBox GBStartTime;
        private TextBox TBStartSeconds;
        private GroupBox GBEndTime;
        private TextBox TBEndSeconds;
        private GroupBox GBClipName;
        private TextBox TBClipName;
        private GroupBox GBLoadCookiesFromBrowser;
        private MaskedTextBox MTBStartTime;
        private MaskedTextBox MTBEndtime;
        private ComboBox CBBrowser;
        private TextBox TBBrowserProfile;
        private CheckBox CBEnableLoadCookiesFromBrowser;
        private GroupBox GBFormat;
        private TextBox TBFormatSelection;
        private GroupBox GBOther;
        private Button BtnDlYtDlp;
        private Button BtnClearLog;
        private Button BtnOpenDownloadFolder;
        private Button BtnDlFFmpeg;
        private Label LYtDlpVer;
        private CheckBox CBDeleteSourceFile;
        private Button BtnExportLog;
        private Button BtnDlAria2;
        private Label LVersion;
        private Button BtnFetchVideoInfo;
        private GroupBox GBNewClip;
        private Button BtnAddNewClip;
        private Button BtnResetNewClip;
        private GroupBox GBClip;
        private Button BtnClearClipList;
        private Button BtnImportTimestamp;
        private DataGridView DgvClipList;
        private Button BtnUpdateYtDlp;
        private TextBox TBAutoEndSeconds;
        private CheckBox CBDownloadFirstSplitLater;
        private Button BtnSplitLocalVideo;
        private GroupBox GBDownloadExternalProgram;
        private CheckBox CBUseHardwareAcceleration;
        private ComboBox CBHardwareAcceleratorType;
        private GroupBox GBHyperlink;
        private LinkLabel LLYouTube;
        private LinkLabel LLAria2;
        private LinkLabel LLYtDlpFFmpeg;
        private LinkLabel LLYtDlp;
        private CheckBox CBEnableEmbedMeta;
        private GroupBox GBFFmpegUseHardwareAcceleration;
        private GroupBox GBOpenFolder;
        private Button BtnOpenBinFolder;
        private CheckBox CBEnableLiveFromStart;
        private ComboBox CBDeviceNo;
        private Button BtnOpenConfigFolder;
        private LinkLabel LLTwitch;
        private CheckBox CBEnableWaitForVideo;
        private Button BtnOpenSubtitleCreator;
        private Button BtnOpenYTSecConverter;
        private LinkLabel LLbilibili;
        private Button BtnBurnIn;
        private GroupBox GBSubtitleOption;
        private CheckBox CBApplyFontSetting;
        private TextBox TBCustomEncode;
        private ComboBox CBEncode;
        private TextBox TBCustomFont;
        private ComboBox CBFont;
        private LinkLabel LLTwitCasting;
        private Serilog.Sinks.WinForms.Core.RichTextBoxLogControl RtbLogCtrlLog;
        private CheckBox CBEnableSplitChapters;
        private Button BtnOpenLogsFolder;
        private DataGridViewTextBoxColumn ClipNo;
        private DataGridViewTextBoxColumn ClipName;
        private DataGridViewTextBoxColumn StartSeconds;
        private DataGridViewTextBoxColumn EndSeconds;
        private DataGridViewCheckBoxColumn IsAudioOnly;
        private CheckBox CBIsAudioOnly;
    }
}