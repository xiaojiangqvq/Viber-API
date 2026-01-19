namespace ViberChannelBulkSender;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Label lblTokens;
    private System.Windows.Forms.TextBox txtTokens;
    private System.Windows.Forms.Label lblMessage;
    private System.Windows.Forms.TextBox txtMessage;
	    private System.Windows.Forms.Button btnSend;
	    private System.Windows.Forms.Label lblBatchSize;
	    private System.Windows.Forms.NumericUpDown numBatchSize;
	    private System.Windows.Forms.Label lblBatchDelay;
	    private System.Windows.Forms.NumericUpDown numBatchDelay;
	    private System.Windows.Forms.Label lblLog;
	    private System.Windows.Forms.TextBox txtLog;
	    private System.Windows.Forms.Label lblStatus;
		    private System.Windows.Forms.Label lblHttpUrl;
			    private System.Windows.Forms.Button btnToggleHttp;
			    private System.Windows.Forms.Button btnInitWebhook;
	    private System.Windows.Forms.Panel headerPanel;
	    private System.Windows.Forms.Label lblTitle;
	    private System.Windows.Forms.Label lblSubtitle;
	    private System.Windows.Forms.Panel panelBottom;

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
	        	this.lblTokens = new System.Windows.Forms.Label();
	        	this.txtTokens = new System.Windows.Forms.TextBox();
		        	this.lblMessage = new System.Windows.Forms.Label();
		        	this.txtMessage = new System.Windows.Forms.TextBox();
		        	this.btnSend = new System.Windows.Forms.Button();
		        	this.lblBatchSize = new System.Windows.Forms.Label();
		        	this.numBatchSize = new System.Windows.Forms.NumericUpDown();
		        	this.lblBatchDelay = new System.Windows.Forms.Label();
		        	this.numBatchDelay = new System.Windows.Forms.NumericUpDown();
		        	this.lblLog = new System.Windows.Forms.Label();
		        	this.txtLog = new System.Windows.Forms.TextBox();
		        	this.lblStatus = new System.Windows.Forms.Label();
		        	this.lblHttpUrl = new System.Windows.Forms.Label();
		        	this.btnToggleHttp = new System.Windows.Forms.Button();
		        	this.btnInitWebhook = new System.Windows.Forms.Button();
	        	this.headerPanel = new System.Windows.Forms.Panel();
	        	this.lblTitle = new System.Windows.Forms.Label();
	        	this.lblSubtitle = new System.Windows.Forms.Label();
	        	this.panelBottom = new System.Windows.Forms.Panel();
	        this.SuspendLayout();
	        // 
	        // headerPanel
	        // 
	        this.headerPanel.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
	        this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
	        this.headerPanel.Height = 52;
	        this.headerPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
	        this.headerPanel.Controls.Add(this.lblTitle);
	        this.headerPanel.Controls.Add(this.lblSubtitle);
	        // 
	        // lblTitle
	        // 
	        this.lblTitle.AutoSize = true;
	        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
	        this.lblTitle.Location = new System.Drawing.Point(12, 6);
	        this.lblTitle.Name = "lblTitle";
	        this.lblTitle.Text = "Viber 频道群发工具";
	        // 
	        // lblSubtitle
	        // 
	        this.lblSubtitle.AutoSize = true;
	        this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
	        this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
	        this.lblSubtitle.Location = new System.Drawing.Point(14, 28);
	        this.lblSubtitle.Name = "lblSubtitle";
	        this.lblSubtitle.Text = "批量配置频道 Token，编辑要群发的文本，一键发送";
        // 
	        // lblTokens
	        // 
	        this.lblTokens.AutoSize = true;
	        this.lblTokens.Location = new System.Drawing.Point(12, 64);
	        this.lblTokens.Name = "lblTokens";
	        this.lblTokens.Size = new System.Drawing.Size(171, 17);
	        this.lblTokens.TabIndex = 0;
	        this.lblTokens.Text = "频道 Token（每行一个）：";
        // 
        // txtTokens
        // 
        this.txtTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
	        this.txtTokens.Location = new System.Drawing.Point(12, 84);
        this.txtTokens.Multiline = true;
        this.txtTokens.Name = "txtTokens";
        this.txtTokens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtTokens.Size = new System.Drawing.Size(776, 120);
        this.txtTokens.TabIndex = 1;
        // 
	        // lblMessage
	        // 
	        this.lblMessage.AutoSize = true;
	        this.lblMessage.Location = new System.Drawing.Point(12, 217);
	        this.lblMessage.Name = "lblMessage";
	        this.lblMessage.Size = new System.Drawing.Size(106, 17);
	        this.lblMessage.TabIndex = 2;
	        this.lblMessage.Text = "要群发的文本：";
        // 
        // txtMessage
        // 
        this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
	        this.txtMessage.Location = new System.Drawing.Point(12, 237);
        this.txtMessage.Multiline = true;
        this.txtMessage.Name = "txtMessage";
        this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtMessage.Size = new System.Drawing.Size(776, 100);
        this.txtMessage.TabIndex = 3;
        // 
	        // btnSend
	        // 
	        this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
	        this.btnSend.Location = new System.Drawing.Point(665, 14);
	        this.btnSend.Name = "btnSend";
	        this.btnSend.Size = new System.Drawing.Size(90, 30);
	        this.btnSend.TabIndex = 4;
	        this.btnSend.Text = "发送";
	        this.btnSend.BackColor = System.Drawing.Color.FromArgb(76, 132, 255);
	        this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
	        this.btnSend.FlatAppearance.BorderSize = 0;
	        this.btnSend.ForeColor = System.Drawing.Color.White;
	        this.btnSend.UseVisualStyleBackColor = false;
	        this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
	        // 
	        // lblBatchSize
	        // 
	        this.lblBatchSize.AutoSize = true;
	        this.lblBatchSize.Location = new System.Drawing.Point(12, 18);
	        this.lblBatchSize.Name = "lblBatchSize";
	        this.lblBatchSize.Size = new System.Drawing.Size(116, 17);
	        this.lblBatchSize.TabIndex = 10;
	        this.lblBatchSize.Text = "每批发送频道数：";
	        // 
	        // numBatchSize
	        // 
	        this.numBatchSize.Location = new System.Drawing.Point(120, 16);
	        this.numBatchSize.Maximum = new decimal(new int[] {
	            1000,
	            0,
	            0,
	            0});
	        this.numBatchSize.Minimum = new decimal(new int[] {
	            1,
	            0,
	            0,
	            0});
	        this.numBatchSize.Name = "numBatchSize";
	        this.numBatchSize.Size = new System.Drawing.Size(60, 23);
	        this.numBatchSize.TabIndex = 11;
	        this.numBatchSize.Value = new decimal(new int[] {
	            8,
	            0,
	            0,
	            0});
	        // 
	        // lblBatchDelay
	        // 
	        this.lblBatchDelay.AutoSize = true;
	        this.lblBatchDelay.Location = new System.Drawing.Point(200, 18);
	        this.lblBatchDelay.Name = "lblBatchDelay";
	        this.lblBatchDelay.Size = new System.Drawing.Size(140, 17);
	        this.lblBatchDelay.TabIndex = 12;
	        this.lblBatchDelay.Text = "每批后暂停(分钟)：";
	        // 
	        // numBatchDelay
	        // 
	        this.numBatchDelay.Location = new System.Drawing.Point(340, 16);
	        this.numBatchDelay.Maximum = new decimal(new int[] {
	            1440,
	            0,
	            0,
	            0});
	        this.numBatchDelay.Name = "numBatchDelay";
	        this.numBatchDelay.Size = new System.Drawing.Size(60, 23);
	        this.numBatchDelay.TabIndex = 13;
        // 
	        // lblLog
	        // 
	        this.lblLog.AutoSize = true;
		        this.lblLog.Location = new System.Drawing.Point(12, 480);
	        this.lblLog.Name = "lblLog";
	        this.lblLog.Size = new System.Drawing.Size(32, 17);
	        this.lblLog.TabIndex = 5;
	        this.lblLog.Text = "日志：";
        // 
	        // txtLog
	        // 
	        this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
	            | System.Windows.Forms.AnchorStyles.Left) 
	            | System.Windows.Forms.AnchorStyles.Right)));
		        this.txtLog.Location = new System.Drawing.Point(12, 500);
	        this.txtLog.Multiline = true;
	        this.txtLog.Name = "txtLog";
	        this.txtLog.ReadOnly = true;
	        this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		        this.txtLog.Size = new System.Drawing.Size(776, 120);
	        this.txtLog.TabIndex = 6;
        // 
	        // lblStatus
	        // 
	        this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	            | System.Windows.Forms.AnchorStyles.Right)));
		        this.lblStatus.Location = new System.Drawing.Point(12, 437);
	        this.lblStatus.Name = "lblStatus";
	        this.lblStatus.Size = new System.Drawing.Size(695, 23);
	        this.lblStatus.TabIndex = 7;
	        this.lblStatus.Text = "就绪";
	        // 
	        // lblHttpUrl
	        // 
	        this.lblHttpUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	            | System.Windows.Forms.AnchorStyles.Right)));
		        this.lblHttpUrl.Location = new System.Drawing.Point(12, 457);
	        this.lblHttpUrl.Name = "lblHttpUrl";
	        this.lblHttpUrl.Size = new System.Drawing.Size(695, 23);
	        this.lblHttpUrl.TabIndex = 8;
	        this.lblHttpUrl.Text = "本地 HTTP 服务：已停止（配合 ngrok http 5000 使用）";
	        // 
	        // btnToggleHttp
	        // 
	        this.btnToggleHttp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
	        this.btnToggleHttp.Location = new System.Drawing.Point(451, 14);
	        this.btnToggleHttp.Name = "btnToggleHttp";
	        this.btnToggleHttp.Size = new System.Drawing.Size(100, 30);
	        this.btnToggleHttp.TabIndex = 9;
	        this.btnToggleHttp.Text = "HTTP 服务";
	        this.btnToggleHttp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
	        this.btnToggleHttp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(76, 132, 255);
	        this.btnToggleHttp.ForeColor = System.Drawing.Color.FromArgb(76, 132, 255);
	        this.btnToggleHttp.UseVisualStyleBackColor = true;
	        this.btnToggleHttp.Click += new System.EventHandler(this.btnToggleHttp_Click);
	        // 
	        // btnInitWebhook
	        // 
	        this.btnInitWebhook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
	        this.btnInitWebhook.Location = new System.Drawing.Point(559, 14);
	        this.btnInitWebhook.Name = "btnInitWebhook";
	        this.btnInitWebhook.Size = new System.Drawing.Size(96, 30);
	        this.btnInitWebhook.TabIndex = 14;
	        this.btnInitWebhook.Text = "ngrok + Webhook";
	        this.btnInitWebhook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
	        this.btnInitWebhook.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(76, 132, 255);
	        this.btnInitWebhook.ForeColor = System.Drawing.Color.FromArgb(76, 132, 255);
	        this.btnInitWebhook.UseVisualStyleBackColor = true;
	        this.btnInitWebhook.Click += new System.EventHandler(this.btnInitWebhook_Click);
	        // 
	        // panelBottom
	        // 
	        this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	            | System.Windows.Forms.AnchorStyles.Right)));
	        this.panelBottom.BackColor = System.Drawing.Color.FromArgb(248, 249, 252);
	        this.panelBottom.Location = new System.Drawing.Point(12, 345);
	        this.panelBottom.Name = "panelBottom";
	        this.panelBottom.Size = new System.Drawing.Size(776, 86);
	        this.panelBottom.TabIndex = 20;
	        this.panelBottom.Controls.Add(this.btnSend);
	        this.panelBottom.Controls.Add(this.btnInitWebhook);
	        this.panelBottom.Controls.Add(this.btnToggleHttp);
	        this.panelBottom.Controls.Add(this.lblBatchSize);
	        this.panelBottom.Controls.Add(this.numBatchSize);
	        this.panelBottom.Controls.Add(this.lblBatchDelay);
	        this.panelBottom.Controls.Add(this.numBatchDelay);
	        this.panelBottom.Controls.Add(this.lblStatus);
	        this.panelBottom.Controls.Add(this.lblHttpUrl);
	        // 
	        // Form1
	        // 
	        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
	        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	        this.ClientSize = new System.Drawing.Size(800, 640);
	        this.BackColor = System.Drawing.Color.White;
	        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
	        this.Controls.Add(this.txtLog);
	        this.Controls.Add(this.lblLog);
	        this.Controls.Add(this.txtMessage);
	        this.Controls.Add(this.lblMessage);
	        this.Controls.Add(this.txtTokens);
	        this.Controls.Add(this.lblTokens);
	        this.Controls.Add(this.panelBottom);
	        this.Controls.Add(this.headerPanel);
	        this.Name = "Form1";
	        this.Text = "ViberBot";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
