namespace ocrFileVeriProc
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lblKeywords = new System.Windows.Forms.Label();
            this.txtBoxKeywords = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnOpenImg = new System.Windows.Forms.Button();
            this.chkBDebug = new System.Windows.Forms.CheckBox();
            this.lblFolderPathImg = new System.Windows.Forms.Label();
            this.txtBoxFolderPathImg = new System.Windows.Forms.TextBox();
            this.btnBrowseImg = new System.Windows.Forms.Button();
            this.lblLang = new System.Windows.Forms.Label();
            this.cmbBoxLang = new System.Windows.Forms.ComboBox();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.btnAbout = new System.Windows.Forms.Button();
            this.linkLabelDoc = new System.Windows.Forms.LinkLabel();
            this.btnOr = new System.Windows.Forms.Button();
            this.btnAnd = new System.Windows.Forms.Button();
            this.lblProcMode = new System.Windows.Forms.Label();
            this.cmbProcMode = new System.Windows.Forms.ComboBox();
            this.lblFolderPathDst = new System.Windows.Forms.Label();
            this.txtBoxFolderPathDst = new System.Windows.Forms.TextBox();
            this.btnBrowseDst = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.txtBoxResults = new System.Windows.Forms.TextBox();
            this.btnOpenDst = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblKeywords
            // 
            resources.ApplyResources(this.lblKeywords, "lblKeywords");
            this.lblKeywords.Name = "lblKeywords";
            // 
            // txtBoxKeywords
            // 
            resources.ApplyResources(this.txtBoxKeywords, "txtBoxKeywords");
            this.txtBoxKeywords.Name = "txtBoxKeywords";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // btnOpenImg
            // 
            resources.ApplyResources(this.btnOpenImg, "btnOpenImg");
            this.btnOpenImg.Name = "btnOpenImg";
            this.btnOpenImg.UseVisualStyleBackColor = true;
            this.btnOpenImg.Click += new System.EventHandler(this.BtnOpenImg_Click);
            // 
            // chkBDebug
            // 
            resources.ApplyResources(this.chkBDebug, "chkBDebug");
            this.chkBDebug.Name = "chkBDebug";
            this.chkBDebug.UseVisualStyleBackColor = true;
            // 
            // lblFolderPathImg
            // 
            resources.ApplyResources(this.lblFolderPathImg, "lblFolderPathImg");
            this.lblFolderPathImg.Name = "lblFolderPathImg";
            // 
            // txtBoxFolderPathImg
            // 
            resources.ApplyResources(this.txtBoxFolderPathImg, "txtBoxFolderPathImg");
            this.txtBoxFolderPathImg.Name = "txtBoxFolderPathImg";
            // 
            // btnBrowseImg
            // 
            resources.ApplyResources(this.btnBrowseImg, "btnBrowseImg");
            this.btnBrowseImg.Name = "btnBrowseImg";
            this.btnBrowseImg.UseVisualStyleBackColor = true;
            this.btnBrowseImg.Click += new System.EventHandler(this.BtnBrowseImg_Click);
            // 
            // lblLang
            // 
            resources.ApplyResources(this.lblLang, "lblLang");
            this.lblLang.Name = "lblLang";
            // 
            // cmbBoxLang
            // 
            resources.ApplyResources(this.cmbBoxLang, "cmbBoxLang");
            this.cmbBoxLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLang.FormattingEnabled = true;
            this.cmbBoxLang.Items.AddRange(new object[] {
            resources.GetString("cmbBoxLang.Items")});
            this.cmbBoxLang.Name = "cmbBoxLang";
            this.cmbBoxLang.SelectedIndexChanged += new System.EventHandler(this.CmbLang_SelectedIndexChanged);
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 1000;
            this.timerProgress.Tick += new System.EventHandler(this.TimerProgress_Tick);
            // 
            // btnAbout
            // 
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.BackColor = System.Drawing.Color.LightCyan;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // linkLabelDoc
            // 
            resources.ApplyResources(this.linkLabelDoc, "linkLabelDoc");
            this.linkLabelDoc.Name = "linkLabelDoc";
            this.linkLabelDoc.TabStop = true;
            this.linkLabelDoc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelDoc_LinkClicked);
            // 
            // btnOr
            // 
            resources.ApplyResources(this.btnOr, "btnOr");
            this.btnOr.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnOr.Name = "btnOr";
            this.btnOr.UseVisualStyleBackColor = false;
            // 
            // btnAnd
            // 
            resources.ApplyResources(this.btnAnd, "btnAnd");
            this.btnAnd.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.UseVisualStyleBackColor = false;
            // 
            // lblProcMode
            // 
            resources.ApplyResources(this.lblProcMode, "lblProcMode");
            this.lblProcMode.Name = "lblProcMode";
            // 
            // cmbProcMode
            // 
            resources.ApplyResources(this.cmbProcMode, "cmbProcMode");
            this.cmbProcMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcMode.FormattingEnabled = true;
            this.cmbProcMode.Items.AddRange(new object[] {
            resources.GetString("cmbProcMode.Items"),
            resources.GetString("cmbProcMode.Items1"),
            resources.GetString("cmbProcMode.Items2"),
            resources.GetString("cmbProcMode.Items3"),
            resources.GetString("cmbProcMode.Items4"),
            resources.GetString("cmbProcMode.Items5")});
            this.cmbProcMode.Name = "cmbProcMode";
            this.cmbProcMode.SelectedIndexChanged += new System.EventHandler(this.CmbProcMode_SelectedIndexChanged);
            // 
            // lblFolderPathDst
            // 
            resources.ApplyResources(this.lblFolderPathDst, "lblFolderPathDst");
            this.lblFolderPathDst.Name = "lblFolderPathDst";
            // 
            // txtBoxFolderPathDst
            // 
            resources.ApplyResources(this.txtBoxFolderPathDst, "txtBoxFolderPathDst");
            this.txtBoxFolderPathDst.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtBoxFolderPathDst.Name = "txtBoxFolderPathDst";
            // 
            // btnBrowseDst
            // 
            resources.ApplyResources(this.btnBrowseDst, "btnBrowseDst");
            this.btnBrowseDst.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBrowseDst.Name = "btnBrowseDst";
            this.btnBrowseDst.UseVisualStyleBackColor = false;
            this.btnBrowseDst.Click += new System.EventHandler(this.BtnBrowseDst_Click);
            // 
            // lblResults
            // 
            resources.ApplyResources(this.lblResults, "lblResults");
            this.lblResults.Name = "lblResults";
            // 
            // txtBoxResults
            // 
            resources.ApplyResources(this.txtBoxResults, "txtBoxResults");
            this.txtBoxResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxResults.Name = "txtBoxResults";
            this.txtBoxResults.ReadOnly = true;
            // 
            // btnOpenDst
            // 
            resources.ApplyResources(this.btnOpenDst, "btnOpenDst");
            this.btnOpenDst.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOpenDst.Name = "btnOpenDst";
            this.btnOpenDst.UseVisualStyleBackColor = false;
            this.btnOpenDst.Click += new System.EventHandler(this.BtnOpenDst_Click);
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOpenDst);
            this.Controls.Add(this.txtBoxResults);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.btnBrowseDst);
            this.Controls.Add(this.txtBoxFolderPathDst);
            this.Controls.Add(this.lblFolderPathDst);
            this.Controls.Add(this.cmbProcMode);
            this.Controls.Add(this.lblProcMode);
            this.Controls.Add(this.btnAnd);
            this.Controls.Add(this.btnOr);
            this.Controls.Add(this.linkLabelDoc);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.cmbBoxLang);
            this.Controls.Add(this.lblLang);
            this.Controls.Add(this.btnBrowseImg);
            this.Controls.Add(this.txtBoxFolderPathImg);
            this.Controls.Add(this.lblFolderPathImg);
            this.Controls.Add(this.chkBDebug);
            this.Controls.Add(this.btnOpenImg);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtBoxKeywords);
            this.Controls.Add(this.lblKeywords);
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKeywords;
        private System.Windows.Forms.TextBox txtBoxKeywords;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnOpenImg;
        private System.Windows.Forms.CheckBox chkBDebug;
        private System.Windows.Forms.Label lblFolderPathImg;
        private System.Windows.Forms.TextBox txtBoxFolderPathImg;
        private System.Windows.Forms.Button btnBrowseImg;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ComboBox cmbBoxLang;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.LinkLabel linkLabelDoc;
        private System.Windows.Forms.Button btnOr;
        private System.Windows.Forms.Button btnAnd;
        private System.Windows.Forms.Label lblProcMode;
        private System.Windows.Forms.ComboBox cmbProcMode;
        private System.Windows.Forms.Label lblFolderPathDst;
        private System.Windows.Forms.TextBox txtBoxFolderPathDst;
        private System.Windows.Forms.Button btnBrowseDst;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.TextBox txtBoxResults;
        private System.Windows.Forms.Button btnOpenDst;
    }
}

