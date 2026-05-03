namespace Poker
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        private void InitializeComponent()
        {
            this.grpBet = new System.Windows.Forms.GroupBox();
            this.btnBet = new System.Windows.Forms.Button();
            this.txtBetAmount = new System.Windows.Forms.TextBox();
            this.lblBet = new System.Windows.Forms.Label();
            this.txtTotalFunds = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.grpFunc = new System.Windows.Forms.GroupBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnDeal = new System.Windows.Forms.Button();
            this.grpTable = new System.Windows.Forms.GroupBox();
            this.grpAiDashboard = new System.Windows.Forms.GroupBox();
            this.lstAiFindings = new System.Windows.Forms.ListView();
            this.colAiField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAiContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAiConfidence = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAiStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblAiAdvice = new System.Windows.Forms.Label();
            this.prgAiConfidence = new System.Windows.Forms.ProgressBar();
            this.lblWinRate = new System.Windows.Forms.Label();
            this.lblAiConfidence = new System.Windows.Forms.Label();
            this.lblAiMode = new System.Windows.Forms.Label();
            this.txtAiReport = new System.Windows.Forms.TextBox();
            this.pnlAiPreview = new System.Windows.Forms.Panel();
            this.btnAiAdvice = new System.Windows.Forms.Button();
            this.btnAiStats = new System.Windows.Forms.Button();
            this.btnAiVision = new System.Windows.Forms.Button();
            this.grpBet.SuspendLayout();
            this.grpFunc.SuspendLayout();
            this.grpAiDashboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBet
            // 
            this.grpBet.Controls.Add(this.btnBet);
            this.grpBet.Controls.Add(this.txtBetAmount);
            this.grpBet.Controls.Add(this.lblBet);
            this.grpBet.Controls.Add(this.txtTotalFunds);
            this.grpBet.Controls.Add(this.lblTotal);
            this.grpBet.Location = new System.Drawing.Point(87, 386);
            this.grpBet.Margin = new System.Windows.Forms.Padding(6);
            this.grpBet.Name = "grpBet";
            this.grpBet.Padding = new System.Windows.Forms.Padding(6);
            this.grpBet.Size = new System.Drawing.Size(1179, 140);
            this.grpBet.TabIndex = 0;
            this.grpBet.TabStop = false;
            this.grpBet.Text = "下注";
            // 
            // btnBet
            // 
            this.btnBet.Location = new System.Drawing.Point(991, 47);
            this.btnBet.Margin = new System.Windows.Forms.Padding(6);
            this.btnBet.Name = "btnBet";
            this.btnBet.Size = new System.Drawing.Size(162, 50);
            this.btnBet.TabIndex = 4;
            this.btnBet.Text = "押注";
            this.btnBet.UseVisualStyleBackColor = true;
            this.btnBet.Click += new System.EventHandler(this.btnBet_Click);
            // 
            // txtBetAmount
            // 
            this.txtBetAmount.Location = new System.Drawing.Point(732, 57);
            this.txtBetAmount.Margin = new System.Windows.Forms.Padding(6);
            this.txtBetAmount.Name = "txtBetAmount";
            this.txtBetAmount.Size = new System.Drawing.Size(247, 36);
            this.txtBetAmount.TabIndex = 3;
            this.txtBetAmount.Text = "500";
            // 
            // lblBet
            // 
            this.lblBet.AutoSize = true;
            this.lblBet.Location = new System.Drawing.Point(614, 60);
            this.lblBet.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBet.Name = "lblBet";
            this.lblBet.Size = new System.Drawing.Size(106, 24);
            this.lblBet.TabIndex = 2;
            this.lblBet.Text = "押注金額";
            // 
            // txtTotalFunds
            // 
            this.txtTotalFunds.Location = new System.Drawing.Point(137, 54);
            this.txtTotalFunds.Margin = new System.Windows.Forms.Padding(6);
            this.txtTotalFunds.Name = "txtTotalFunds";
            this.txtTotalFunds.ReadOnly = true;
            this.txtTotalFunds.Size = new System.Drawing.Size(458, 36);
            this.txtTotalFunds.TabIndex = 1;
            this.txtTotalFunds.Text = "1000000";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(43, 60);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(82, 24);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "總資金";
            // 
            // grpFunc
            // 
            this.grpFunc.Controls.Add(this.txtResult);
            this.grpFunc.Controls.Add(this.btnEvaluate);
            this.grpFunc.Controls.Add(this.btnChange);
            this.grpFunc.Controls.Add(this.btnDeal);
            this.grpFunc.Location = new System.Drawing.Point(87, 553);
            this.grpFunc.Margin = new System.Windows.Forms.Padding(6);
            this.grpFunc.Name = "grpFunc";
            this.grpFunc.Padding = new System.Windows.Forms.Padding(6);
            this.grpFunc.Size = new System.Drawing.Size(1179, 140);
            this.grpFunc.TabIndex = 1;
            this.grpFunc.TabStop = false;
            this.grpFunc.Text = "功能";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(650, 54);
            this.txtResult.Margin = new System.Windows.Forms.Padding(6);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(374, 36);
            this.txtResult.TabIndex = 3;
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(433, 50);
            this.btnEvaluate.Margin = new System.Windows.Forms.Padding(6);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(162, 64);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "判斷牌型";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(238, 50);
            this.btnChange.Margin = new System.Windows.Forms.Padding(6);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(162, 64);
            this.btnChange.TabIndex = 1;
            this.btnChange.Text = "換牌";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnDeal
            // 
            this.btnDeal.Location = new System.Drawing.Point(43, 50);
            this.btnDeal.Margin = new System.Windows.Forms.Padding(6);
            this.btnDeal.Name = "btnDeal";
            this.btnDeal.Size = new System.Drawing.Size(162, 64);
            this.btnDeal.TabIndex = 0;
            this.btnDeal.Text = "發牌";
            this.btnDeal.UseVisualStyleBackColor = true;
            this.btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
            // 
            // grpTable
            // 
            this.grpTable.Location = new System.Drawing.Point(87, 40);
            this.grpTable.Margin = new System.Windows.Forms.Padding(6);
            this.grpTable.Name = "grpTable";
            this.grpTable.Padding = new System.Windows.Forms.Padding(6);
            this.grpTable.Size = new System.Drawing.Size(1179, 334);
            this.grpTable.TabIndex = 2;
            this.grpTable.TabStop = false;
            this.grpTable.Text = "牌桌";
            // 
            // grpAiDashboard
            // 
            this.grpAiDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAiDashboard.Controls.Add(this.lstAiFindings);
            this.grpAiDashboard.Controls.Add(this.lblAiAdvice);
            this.grpAiDashboard.Controls.Add(this.prgAiConfidence);
            this.grpAiDashboard.Controls.Add(this.lblWinRate);
            this.grpAiDashboard.Controls.Add(this.lblAiConfidence);
            this.grpAiDashboard.Controls.Add(this.lblAiMode);
            this.grpAiDashboard.Controls.Add(this.txtAiReport);
            this.grpAiDashboard.Controls.Add(this.pnlAiPreview);
            this.grpAiDashboard.Controls.Add(this.btnAiAdvice);
            this.grpAiDashboard.Controls.Add(this.btnAiStats);
            this.grpAiDashboard.Controls.Add(this.btnAiVision);
            this.grpAiDashboard.Location = new System.Drawing.Point(87, 709);
            this.grpAiDashboard.Margin = new System.Windows.Forms.Padding(6);
            this.grpAiDashboard.Name = "grpAiDashboard";
            this.grpAiDashboard.Padding = new System.Windows.Forms.Padding(6);
            this.grpAiDashboard.Size = new System.Drawing.Size(1179, 597);
            this.grpAiDashboard.TabIndex = 3;
            this.grpAiDashboard.TabStop = false;
            this.grpAiDashboard.Text = "AI ANALYSIS DASHBOARD / 影像辨識 - 統計分析 - 最佳換牌建議";
            // 
            // lstAiFindings
            // 
            this.lstAiFindings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAiField,
            this.colAiContent,
            this.colAiConfidence,
            this.colAiStatus});
            this.lstAiFindings.FullRowSelect = true;
            this.lstAiFindings.GridLines = true;
            this.lstAiFindings.HideSelection = false;
            this.lstAiFindings.Location = new System.Drawing.Point(20, 372);
            this.lstAiFindings.Name = "lstAiFindings";
            this.lstAiFindings.Size = new System.Drawing.Size(1043, 216);
            this.lstAiFindings.TabIndex = 10;
            this.lstAiFindings.UseCompatibleStateImageBehavior = false;
            this.lstAiFindings.View = System.Windows.Forms.View.Details;
            // 
            // colAiField
            // 
            this.colAiField.Text = "欄位";
            this.colAiField.Width = 120;
            // 
            // colAiContent
            // 
            this.colAiContent.Text = "分析內容";
            this.colAiContent.Width = 260;
            // 
            // colAiConfidence
            // 
            this.colAiConfidence.Text = "信心度";
            this.colAiConfidence.Width = 90;
            // 
            // colAiStatus
            // 
            this.colAiStatus.Text = "狀態";
            this.colAiStatus.Width = 90;
            // 
            // lblAiAdvice
            // 
            this.lblAiAdvice.Location = new System.Drawing.Point(20, 324);
            this.lblAiAdvice.Name = "lblAiAdvice";
            this.lblAiAdvice.Size = new System.Drawing.Size(1043, 24);
            this.lblAiAdvice.TabIndex = 9;
            this.lblAiAdvice.Text = "建議：請先押注並發牌。";
            // 
            // prgAiConfidence
            // 
            this.prgAiConfidence.Location = new System.Drawing.Point(550, 324);
            this.prgAiConfidence.Name = "prgAiConfidence";
            this.prgAiConfidence.Size = new System.Drawing.Size(513, 18);
            this.prgAiConfidence.TabIndex = 8;
            // 
            // lblWinRate
            // 
            this.lblWinRate.Location = new System.Drawing.Point(808, 282);
            this.lblWinRate.Name = "lblWinRate";
            this.lblWinRate.Size = new System.Drawing.Size(255, 24);
            this.lblWinRate.TabIndex = 7;
            this.lblWinRate.Text = "DL 勝率：等待計算...";
            // 
            // lblAiConfidence
            // 
            this.lblAiConfidence.Location = new System.Drawing.Point(552, 282);
            this.lblAiConfidence.Name = "lblAiConfidence";
            this.lblAiConfidence.Size = new System.Drawing.Size(250, 24);
            this.lblAiConfidence.TabIndex = 6;
            this.lblAiConfidence.Text = "平均信心度：0%";
            // 
            // lblAiMode
            // 
            this.lblAiMode.Location = new System.Drawing.Point(20, 282);
            this.lblAiMode.Name = "lblAiMode";
            this.lblAiMode.Size = new System.Drawing.Size(510, 24);
            this.lblAiMode.TabIndex = 5;
            this.lblAiMode.Text = "目前模式：尚未分析";
            // 
            // txtAiReport
            // 
            this.txtAiReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAiReport.Location = new System.Drawing.Point(550, 112);
            this.txtAiReport.Multiline = true;
            this.txtAiReport.Name = "txtAiReport";
            this.txtAiReport.ReadOnly = true;
            this.txtAiReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAiReport.Size = new System.Drawing.Size(513, 150);
            this.txtAiReport.TabIndex = 4;
            // 
            // pnlAiPreview
            // 
            this.pnlAiPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAiPreview.Location = new System.Drawing.Point(20, 112);
            this.pnlAiPreview.Name = "pnlAiPreview";
            this.pnlAiPreview.Size = new System.Drawing.Size(510, 150);
            this.pnlAiPreview.TabIndex = 3;
            // 
            // btnAiAdvice
            // 
            this.btnAiAdvice.Location = new System.Drawing.Point(344, 38);
            this.btnAiAdvice.Name = "btnAiAdvice";
            this.btnAiAdvice.Size = new System.Drawing.Size(180, 52);
            this.btnAiAdvice.TabIndex = 2;
            this.btnAiAdvice.Text = "最佳換牌建議";
            this.btnAiAdvice.UseVisualStyleBackColor = true;
            // 
            // btnAiStats
            // 
            this.btnAiStats.Location = new System.Drawing.Point(182, 38);
            this.btnAiStats.Name = "btnAiStats";
            this.btnAiStats.Size = new System.Drawing.Size(150, 52);
            this.btnAiStats.TabIndex = 1;
            this.btnAiStats.Text = "統計分析";
            this.btnAiStats.UseVisualStyleBackColor = true;
            // 
            // btnAiVision
            // 
            this.btnAiVision.Location = new System.Drawing.Point(20, 38);
            this.btnAiVision.Name = "btnAiVision";
            this.btnAiVision.Size = new System.Drawing.Size(150, 52);
            this.btnAiVision.TabIndex = 0;
            this.btnAiVision.Text = "影像辨識";
            this.btnAiVision.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 1346);
            this.Controls.Add(this.grpAiDashboard);
            this.Controls.Add(this.grpTable);
            this.Controls.Add(this.grpFunc);
            this.Controls.Add(this.grpBet);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "五張撲克牌";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpBet.ResumeLayout(false);
            this.grpBet.PerformLayout();
            this.grpFunc.ResumeLayout(false);
            this.grpFunc.PerformLayout();
            this.grpAiDashboard.ResumeLayout(false);
            this.grpAiDashboard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBet;
        private System.Windows.Forms.Button btnBet;
        private System.Windows.Forms.TextBox txtBetAmount;
        private System.Windows.Forms.Label lblBet;
        private System.Windows.Forms.TextBox txtTotalFunds;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox grpFunc;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnDeal;
        private System.Windows.Forms.GroupBox grpTable;
        private System.Windows.Forms.GroupBox grpAiDashboard;
        private System.Windows.Forms.Button btnAiVision;
        private System.Windows.Forms.Button btnAiStats;
        private System.Windows.Forms.Button btnAiAdvice;
        private System.Windows.Forms.Panel pnlAiPreview;
        private System.Windows.Forms.TextBox txtAiReport;
        private System.Windows.Forms.Label lblAiMode;
        private System.Windows.Forms.Label lblAiConfidence;
        private System.Windows.Forms.Label lblWinRate;
        private System.Windows.Forms.ProgressBar prgAiConfidence;
        private System.Windows.Forms.Label lblAiAdvice;
        private System.Windows.Forms.ListView lstAiFindings;
        private System.Windows.Forms.ColumnHeader colAiField;
        private System.Windows.Forms.ColumnHeader colAiContent;
        private System.Windows.Forms.ColumnHeader colAiConfidence;
        private System.Windows.Forms.ColumnHeader colAiStatus;
    }
}
