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
            this.grpBet.SuspendLayout();
            this.grpFunc.SuspendLayout();
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
            this.grpBet.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpBet.Name = "grpBet";
            this.grpBet.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpBet.Size = new System.Drawing.Size(1083, 140);
            this.grpBet.TabIndex = 0;
            this.grpBet.TabStop = false;
            this.grpBet.Text = "下注";
            // 
            // btnBet
            // 
            this.btnBet.Location = new System.Drawing.Point(867, 50);
            this.btnBet.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnBet.Name = "btnBet";
            this.btnBet.Size = new System.Drawing.Size(162, 50);
            this.btnBet.TabIndex = 4;
            this.btnBet.Text = "押注";
            this.btnBet.UseVisualStyleBackColor = true;
            this.btnBet.Click += new System.EventHandler(this.btnBet_Click);
            // 
            // txtBetAmount
            // 
            this.txtBetAmount.Location = new System.Drawing.Point(607, 54);
            this.txtBetAmount.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtBetAmount.Name = "txtBetAmount";
            this.txtBetAmount.Size = new System.Drawing.Size(212, 36);
            this.txtBetAmount.TabIndex = 3;
            this.txtBetAmount.Text = "500";
            // 
            // lblBet
            // 
            this.lblBet.AutoSize = true;
            this.lblBet.Location = new System.Drawing.Point(477, 60);
            this.lblBet.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBet.Name = "lblBet";
            this.lblBet.Size = new System.Drawing.Size(106, 24);
            this.lblBet.TabIndex = 2;
            this.lblBet.Text = "押注金額";
            // 
            // txtTotalFunds
            // 
            this.txtTotalFunds.Location = new System.Drawing.Point(173, 54);
            this.txtTotalFunds.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTotalFunds.Name = "txtTotalFunds";
            this.txtTotalFunds.ReadOnly = true;
            this.txtTotalFunds.Size = new System.Drawing.Size(212, 36);
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
            this.grpFunc.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpFunc.Name = "grpFunc";
            this.grpFunc.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpFunc.Size = new System.Drawing.Size(1083, 140);
            this.grpFunc.TabIndex = 1;
            this.grpFunc.TabStop = false;
            this.grpFunc.Text = "功能";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(650, 54);
            this.txtResult.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(374, 36);
            this.txtResult.TabIndex = 3;
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(433, 50);
            this.btnEvaluate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(162, 50);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "判斷牌型";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(238, 50);
            this.btnChange.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(162, 50);
            this.btnChange.TabIndex = 1;
            this.btnChange.Text = "換牌";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnDeal
            // 
            this.btnDeal.Location = new System.Drawing.Point(43, 50);
            this.btnDeal.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDeal.Name = "btnDeal";
            this.btnDeal.Size = new System.Drawing.Size(162, 50);
            this.btnDeal.TabIndex = 0;
            this.btnDeal.Text = "發牌";
            this.btnDeal.UseVisualStyleBackColor = true;
            this.btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
            // 
            // grpTable
            // 
            this.grpTable.Location = new System.Drawing.Point(87, 40);
            this.grpTable.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpTable.Name = "grpTable";
            this.grpTable.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grpTable.Size = new System.Drawing.Size(1083, 334);
            this.grpTable.TabIndex = 2;
            this.grpTable.TabStop = false;
            this.grpTable.Text = "牌桌";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 763);
            this.Controls.Add(this.grpTable);
            this.Controls.Add(this.grpFunc);
            this.Controls.Add(this.grpBet);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "五張撲克牌";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpBet.ResumeLayout(false);
            this.grpBet.PerformLayout();
            this.grpFunc.ResumeLayout(false);
            this.grpFunc.PerformLayout();
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
    }
}

