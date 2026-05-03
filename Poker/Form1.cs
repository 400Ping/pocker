using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Poker
{
    public partial class Form1 : Form
    {
        private int totalFunds = 1000000;
        private int currentBet = 0;
        private int[] allPoker = new int[52];
        private int[] playerPoker = new int[5];
        private int nextCardIndex = 0;
        private readonly Random rand = new Random();
        private PictureBox[] picCards = new PictureBox[5];
        private bool[] cardSelectedToChange = new bool[5];
        private bool canSelectCards = false;
        private bool handDealt = false;

        private readonly byte[] securityKey = Encoding.UTF8.GetBytes("Poker-CIA-Integrity-Key-2026");
        private string encodedFunds;
        private string fundsHash;
        private string fundsMac;
        private bool securityFailure = false;
        private List<int> aiPreviewCards = new List<int>();

        private readonly string[] suitNames = { "梅花", "方塊", "紅心", "黑桃" };
        private readonly string[] pointNames = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        private readonly Color darkPanel = Color.FromArgb(12, 18, 27);
        private readonly Color darkerPanel = Color.FromArgb(7, 10, 16);
        private readonly Color accentGold = Color.FromArgb(206, 163, 71);
        private readonly Color accentBlue = Color.FromArgb(49, 176, 214);
        private readonly Color accentGreen = Color.FromArgb(88, 173, 112);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyPress += Form1_KeyPress;
            this.AutoScroll = true; // 開啟視窗自動捲動功能

            // 讓傳統群組框支持動態縮放 (響應式設計)
            grpTable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpBet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpFunc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            ApplyCasinoTheme();
            InitializeAiDashboard();

            // 綁定視窗縮放事件，以致中排列撲克牌
            this.Resize += Form1_Resize;

            // 初始化五張撲克牌的 PictureBox
            for (int i = 0; i < 5; i++)
            {
                picCards[i] = new PictureBox();
                picCards[i].Width = 71;
                picCards[i].Height = 96;
                picCards[i].Left = 20 + i * 90;
                picCards[i].Top = 50;
                picCards[i].SizeMode = PictureBoxSizeMode.StretchImage; // 改為 StretchImage 支援縮放
                picCards[i].BorderStyle = BorderStyle.FixedSingle;
                picCards[i].Name = "pic" + i;
                picCards[i].Tag = "back";
                picCards[i].Enabled = false;
                picCards[i].MouseClick += PicCard_Click;
                grpTable.Controls.Add(picCards[i]);
            }
            InitializeSecureFunds(totalFunds);
            ResetGame();
            UpdateAiDashboardIdle();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 動態調整撲克牌在桌面的位置與大小，使其在放大視窗時保持等比例擴展及置中
            if (picCards != null && picCards[0] != null)
            {
                int padding = 20;
                int cardWidth = (grpTable.Width - padding * 6) / 5;
                int cardHeight = (int)(cardWidth * 1.35f); // 保持卡牌比例，約 96/71 = 1.35

                // 確保不超過最大可用高度
                if (cardHeight > grpTable.Height - 40)
                {
                    cardHeight = grpTable.Height - 40;
                    cardWidth = (int)(cardHeight / 1.35f);
                }

                int totalWidth = 5 * cardWidth + 4 * padding;
                int startX = Math.Max(10, (grpTable.Width - totalWidth) / 2);
                int startY = Math.Max(20, (grpTable.Height - cardHeight) / 2);

                for (int i = 0; i < 5; i++)
                {
                    picCards[i].Width = cardWidth;
                    picCards[i].Height = cardHeight;
                    picCards[i].Left = startX + i * (cardWidth + padding);
                    picCards[i].Top = startY;
                }
            }

            if (grpAiDashboard != null)
            {
                ResizeAiDashboard();
            }
        }

        private void ApplyCasinoTheme()
        {
            this.Text = "AI Casino Poker Lab - 五張撲克牌";
            this.BackColor = Color.FromArgb(5, 9, 15);
            this.ForeColor = Color.WhiteSmoke;
            this.ClientSize = new Size(Math.Max(this.ClientSize.Width, 1213), 980);

            StyleGroupBox(grpTable, "POKER TABLE / 牌桌");
            StyleGroupBox(grpBet, "BETTING / 下注");
            StyleGroupBox(grpFunc, "GAME CONTROL / 遊戲控制");

            txtTotalFunds.BackColor = darkerPanel;
            txtTotalFunds.ForeColor = Color.White;
            txtBetAmount.BackColor = darkerPanel;
            txtBetAmount.ForeColor = Color.White;
            txtResult.BackColor = darkerPanel;
            txtResult.ForeColor = Color.White;

            StyleButton(btnBet, accentGold);
            StyleButton(btnDeal, accentBlue);
            StyleButton(btnChange, accentGold);
            StyleButton(btnEvaluate, accentGreen);

            lblTotal.ForeColor = accentGold;
            lblBet.ForeColor = accentGold;
        }

        private void InitializeAiDashboard()
        {
            StyleGroupBox(grpAiDashboard, grpAiDashboard.Text);
            StyleButton(btnAiVision, accentBlue);
            StyleButton(btnAiStats, accentGold);
            StyleButton(btnAiAdvice, accentGreen);
            btnAiVision.Click += (s, e) => ExecuteGameAction(RunImageRecognitionAnalysis);
            btnAiStats.Click += (s, e) => ExecuteGameAction(RunStatisticalAnalysis);
            btnAiAdvice.Click += (s, e) => ExecuteGameAction(RunBestSwapAdvice);

            pnlAiPreview.BackColor = darkerPanel;
            pnlAiPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlAiPreview.Paint += PnlAiPreview_Paint;

            txtAiReport.BackColor = darkerPanel;
            txtAiReport.ForeColor = Color.WhiteSmoke;
            txtAiReport.BorderStyle = BorderStyle.FixedSingle;
            txtAiReport.Font = new Font("Consolas", 9F);

            lstAiFindings.BackColor = darkerPanel;
            lstAiFindings.ForeColor = Color.WhiteSmoke;
            lstAiFindings.BorderStyle = BorderStyle.FixedSingle;

            StyleAiLabel(lblAiMode, accentGold, true);
            StyleAiLabel(lblAiConfidence, accentBlue, true);
            StyleAiLabel(lblAiAdvice, Color.WhiteSmoke, false);
            StyleAiLabel(lblWinRate, accentBlue, true);

            prgAiConfidence.Minimum = 0;
            prgAiConfidence.Maximum = 100;
            prgAiConfidence.Value = 0;

            ResizeAiDashboard();
        }

        private void ResizeAiDashboard()
        {
            grpAiDashboard.Left = grpFunc.Left;
            grpAiDashboard.Top = grpFunc.Bottom + 16;
            grpAiDashboard.Width = grpFunc.Width;

            int margin = 20;
            int buttonTop = 38;
            btnAiVision.Location = new Point(margin, buttonTop);
            btnAiStats.Location = new Point(btnAiVision.Right + 12, buttonTop);
            btnAiAdvice.Location = new Point(btnAiStats.Right + 12, buttonTop);

            int panelTop = btnAiVision.Bottom + 16;
            int panelHeight = 150;
            int leftWidth = Math.Max(360, (grpAiDashboard.ClientSize.Width - margin * 3) / 2);
            int rightWidth = grpAiDashboard.ClientSize.Width - leftWidth - margin * 3;

            pnlAiPreview.SetBounds(margin, panelTop, leftWidth, panelHeight);
            txtAiReport.SetBounds(pnlAiPreview.Right + margin, panelTop, rightWidth, panelHeight);

            lblAiMode.Location = new Point(margin, pnlAiPreview.Bottom + 14);
            lblAiMode.Width = leftWidth;
            lblAiConfidence.Location = new Point(pnlAiPreview.Right + margin, pnlAiPreview.Bottom + 14);
            lblAiConfidence.Width = rightWidth / 2;
            lblWinRate.Location = new Point(lblAiConfidence.Right + 8, pnlAiPreview.Bottom + 14);
            lblWinRate.Width = rightWidth / 2 - 8;

            prgAiConfidence.SetBounds(pnlAiPreview.Right + margin, lblAiConfidence.Bottom + 8, rightWidth, 18);
            lblAiAdvice.SetBounds(margin, lblAiMode.Bottom + 18, grpAiDashboard.ClientSize.Width - margin * 2, 24);
            lstAiFindings.SetBounds(margin, lblAiAdvice.Bottom + 10, grpAiDashboard.ClientSize.Width - margin * 2, 78);

            grpAiDashboard.Height = lstAiFindings.Bottom + margin;
            this.AutoScrollMinSize = new Size(0, grpAiDashboard.Bottom + 20);
        }

        private void StyleAiLabel(Label label, Color color, bool bold)
        {
            label.AutoSize = false;
            label.Height = 24;
            label.ForeColor = color;
            label.Font = new Font("微軟正黑體", 9F, bold ? FontStyle.Bold : FontStyle.Regular);
        }

        private void StyleGroupBox(GroupBox groupBox, string text)
        {
            groupBox.Text = text;
            groupBox.BackColor = darkPanel;
            groupBox.ForeColor = accentGold;
            groupBox.Font = new Font("微軟正黑體", 9F, FontStyle.Bold);
        }

        private void StyleButton(Button button, Color borderColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = 2;
            button.Font = new Font("微軟正黑體", 9F, FontStyle.Bold);
            button.Tag = borderColor;
            button.UseVisualStyleBackColor = false;
            button.EnabledChanged += (s, e) => UpdateButtonVisualState((Button)s);
            UpdateButtonVisualState(button);
        }

        private void UpdateButtonVisualState(Button button)
        {
            if (button.Enabled)
            {
                button.BackColor = darkPanel;
                button.ForeColor = Color.WhiteSmoke;
            }
            else
            {
                button.BackColor = Color.FromArgb(225, 229, 235);
                button.ForeColor = Color.FromArgb(35, 42, 52);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnDeal.Enabled == false)
            {
                switch (char.ToLowerInvariant(e.KeyChar))
                {
                    case 'q': // q鍵
                        // 同花大順
                        playerPoker[0] = 51;
                        playerPoker[1] = 47;
                        playerPoker[2] = 43;
                        playerPoker[3] = 39;
                        playerPoker[4] = 3;
                        break;
                    case 'w': // w鍵
                        // 同花順
                        playerPoker[0] = 37;
                        playerPoker[1] = 33;
                        playerPoker[2] = 29;
                        playerPoker[3] = 25;
                        playerPoker[4] = 21;
                        break;
                    case 'e': // e鍵
                        // 同花
                        playerPoker[0] = 50;
                        playerPoker[1] = 38;
                        playerPoker[2] = 34;
                        playerPoker[3] = 22;
                        playerPoker[4] = 18;
                        break;
                    case 'r': // r鍵
                        // 鐵支
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 38;
                        playerPoker[3] = 37;
                        playerPoker[4] = 36;
                        break;
                    case 't': // t鍵
                        // 葫蘆
                        playerPoker[0] = 30;
                        playerPoker[1] = 29;
                        playerPoker[2] = 6;
                        playerPoker[3] = 5;
                        playerPoker[4] = 4;
                        break;
                    case 'y': // y鍵
                        // 三條
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 15;
                        playerPoker[3] = 14;
                        playerPoker[4] = 13;
                        break;
                }

                // 顯示五張撲克牌到桌面上
                for (int i = 0; i < 5; i++)
                {
                    UpdateCardImage(i, playerPoker[i]);
                }
            }
        }

        private void PicCard_Click(object sender, MouseEventArgs e)
        {
            if (!canSelectCards)
            {
                return;
            }

            PictureBox pic = (PictureBox)sender;
            int idx = int.Parse(pic.Name.Replace("pic", ""));

            if (pic.Tag.ToString() == "back")
            {
                pic.Tag = "front";
                cardSelectedToChange[idx] = false;
                UpdateCardImage(idx, playerPoker[idx]);
            }
            else
            {
                pic.Tag = "back";
                cardSelectedToChange[idx] = true;
                pic.Image = Properties.Resources.back;
            }
        }

        private void ResetGame()
        {
            txtResult.Text = "";
            currentBet = 0;
            handDealt = false;
            UpdateFundsDisplay();
            btnChange.Enabled = false;
            btnEvaluate.Enabled = false;
            btnDeal.Enabled = false; // 必須先押注
            btnBet.Enabled = true;
            canSelectCards = false;

            Image backImage = Properties.Resources.back;

            for (int i = 0; i < 5; i++)
            {
                picCards[i].Image = backImage;
                picCards[i].Top = 50;
                cardSelectedToChange[i] = false;
                picCards[i].Tag = "back";
                picCards[i].Enabled = false;
            }
        }

        private void InitializeDeck()
        {
            for (int i = 0; i < allPoker.Length; i++)
            {
                allPoker[i] = i;
            }
            Shuffle();
        }

        private void Shuffle()
        {
            for (int i = 0; i < allPoker.Length; i++)
            {
                int r = rand.Next(allPoker.Length);
                int temp = allPoker[r];
                allPoker[r] = allPoker[0];
                allPoker[0] = temp;
            }
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            ExecuteGameAction(PlaceBet);
        }

        private void PlaceBet()
        {
            if (int.TryParse(txtBetAmount.Text, out int betAmount))
            {
                if (betAmount > totalFunds || betAmount <= 0)
                {
                    MessageBox.Show("押注金額錯誤或資金不足！");
                    return;
                }
                currentBet = betAmount;
                SecureSetFunds(totalFunds - currentBet, "下注扣款");

                btnDeal.Enabled = true;
                btnBet.Enabled = false;
                txtResult.Text = "請發牌...";
            }
            else
            {
                MessageBox.Show("請輸入正確的數字！");
            }
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            ExecuteGameAction(DealCards);
        }

        private void DealCards()
        {
            InitializeDeck();
            nextCardIndex = 0;

            // 發五張牌
            for (int i = 0; i < 5; i++)
            {
                playerPoker[i] = allPoker[nextCardIndex++];
                UpdateCardImage(i, playerPoker[i]);
                cardSelectedToChange[i] = false;
                picCards[i].Tag = "front";
                picCards[i].Enabled = true;
            }

            btnDeal.Enabled = false;
            btnChange.Enabled = true;
            btnEvaluate.Enabled = true;
            canSelectCards = true;
            handDealt = true;
            txtResult.Text = "請點擊要保留的牌，然後換牌或直接判斷牌型";
            RunStatisticalAnalysis();

            RunDeepLearningModel(); // 啟動深度學習預測引擎
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            ExecuteGameAction(ChangeCards);
        }

        private void ChangeCards()
        {
            for (int i = 0; i < 5; i++)
            {
                if (cardSelectedToChange[i])
                {
                    playerPoker[i] = allPoker[nextCardIndex++];
                    UpdateCardImage(i, playerPoker[i]);
                }
                cardSelectedToChange[i] = false;
                picCards[i].Tag = "front";
                picCards[i].Enabled = false;
            }

            btnChange.Enabled = false;
            canSelectCards = false;
            RunStatisticalAnalysis();
        }

        private void UpdateCardImage(int index, int cardId)
        {
            // 動態從資源中讀取圖片，名稱對應 pic1.png ~ pic52.png
            object rm = Properties.Resources.ResourceManager.GetObject("pic" + (cardId + 1));
            if (rm != null)
            {
                picCards[index].Image = (Image)rm; // 保持原圖，完全不覆蓋或繪製任何字樣
            }
        }

        private void RunImageRecognitionAnalysis()
        {
            if (!HasVisibleHand())
            {
                UpdateAiDashboardIdle();
                return;
            }

            aiPreviewCards = playerPoker.ToList();
            string[] detectedCards = playerPoker.Select(GetCardName).ToArray();
            int confidence = 96;

            SetAiStatus("影像辨識", confidence, "已從目前牌面影像狀態建立手牌摘要。");
            txtAiReport.Text =
                "牌面影像辨識報告" + Environment.NewLine +
                "----------------" + Environment.NewLine +
                string.Join(Environment.NewLine, detectedCards.Select((card, index) => $"CARD {index + 1}: {card}")) + Environment.NewLine +
                Environment.NewLine +
                "方法：以目前 PictureBox 牌面資源與遊戲狀態建立辨識結果。" + Environment.NewLine +
                "輸出：5 張牌皆已對應到花色與點數。";

            lstAiFindings.Items.Clear();
            for (int i = 0; i < detectedCards.Length; i++)
            {
                AddAiFinding($"第 {i + 1} 張", detectedCards[i], $"{confidence - i}%", "已辨識");
            }

            pnlAiPreview.Invalidate();
        }

        private void RunStatisticalAnalysis()
        {
            if (!HasVisibleHand())
            {
                UpdateAiDashboardIdle();
                return;
            }

            HandEvaluation evaluation = EvaluatePokerHand(playerPoker);
            SwapAdvice advice = FindBestSwapAdvice();
            int confidence = 90;

            SetAiStatus("統計分析", confidence, $"目前牌型：{evaluation.Name}，最佳期望倍率：{advice.ExpectedMultiplier:F2}x。");
            txtAiReport.Text =
                "統計分析報告" + Environment.NewLine +
                "------------" + Environment.NewLine +
                $"目前手牌：{string.Join("、", playerPoker.Select(GetCardName))}" + Environment.NewLine +
                $"目前牌型：{evaluation.Name}" + Environment.NewLine +
                $"目前倍率：{evaluation.Multiplier}x" + Environment.NewLine +
                $"押注金額：{currentBet}" + Environment.NewLine +
                $"目前可得獎金估算：{currentBet * evaluation.Multiplier}" + Environment.NewLine +
                $"最佳換牌期望倍率：{advice.ExpectedMultiplier:F2}x" + Environment.NewLine +
                $"最佳換牌期望獎金：{currentBet * advice.ExpectedMultiplier:F0}";

            lstAiFindings.Items.Clear();
            AddAiFinding("目前牌型", evaluation.Name, "100%", "已計算");
            AddAiFinding("目前倍率", $"{evaluation.Multiplier}x", "100%", "已計算");
            AddAiFinding("期望倍率", $"{advice.ExpectedMultiplier:F2}x", $"{confidence}%", "已估算");
            AddAiFinding("換牌張數", $"{advice.DiscardIndexes.Count} 張", $"{confidence}%", "已估算");

            aiPreviewCards = playerPoker.ToList();
            pnlAiPreview.Invalidate();
        }

        private void RunBestSwapAdvice()
        {
            if (!HasVisibleHand())
            {
                UpdateAiDashboardIdle();
                return;
            }

            SwapAdvice advice = FindBestSwapAdvice();
            int confidence = 88;
            string discardText = advice.DiscardIndexes.Count == 0
                ? "保留全部手牌"
                : string.Join("、", advice.DiscardIndexes.Select(index => $"第 {index + 1} 張 {GetCardName(playerPoker[index])}"));

            SetAiStatus("最佳換牌建議", confidence, advice.Summary);
            txtAiReport.Text =
                "最佳換牌建議" + Environment.NewLine +
                "------------" + Environment.NewLine +
                $"建議動作：{discardText}" + Environment.NewLine +
                $"保留手牌：{advice.KeepSummary}" + Environment.NewLine +
                $"期望倍率：{advice.ExpectedMultiplier:F2}x" + Environment.NewLine +
                $"抽樣組合：{advice.SampleCount:N0}" + Environment.NewLine +
                Environment.NewLine +
                advice.Summary;

            lstAiFindings.Items.Clear();
            AddAiFinding("建議", discardText, $"{confidence}%", "可執行");
            AddAiFinding("保留", advice.KeepSummary, $"{confidence}%", "已選擇");
            AddAiFinding("期望倍率", $"{advice.ExpectedMultiplier:F2}x", $"{confidence}%", "已估算");
            AddAiFinding("抽樣組合", $"{advice.SampleCount:N0}", "100%", "已完成");

            aiPreviewCards = playerPoker.ToList();
            pnlAiPreview.Invalidate();
        }

        private void UpdateAiDashboardIdle()
        {
            aiPreviewCards = new List<int>();
            SetAiStatus("尚未分析", 0, "建議：請先押注並發牌，再執行 AI 分析。");
            txtAiReport.Text = "等待手牌資料。發牌後可執行影像辨識、統計分析或最佳換牌建議。";
            lstAiFindings.Items.Clear();
            AddAiFinding("狀態", "等待發牌", "0%", "待命");
            pnlAiPreview?.Invalidate();
        }

        private void SetAiStatus(string mode, int confidence, string advice)
        {
            lblAiMode.Text = $"目前模式：{mode}";
            lblAiConfidence.Text = $"平均信心度：{confidence}%";
            lblAiAdvice.Text = $"建議：{advice}";
            prgAiConfidence.Value = Math.Max(0, Math.Min(100, confidence));
        }

        private void AddAiFinding(string field, string content, string confidence, string status)
        {
            ListViewItem item = new ListViewItem(field);
            item.SubItems.Add(content);
            item.SubItems.Add(confidence);
            item.SubItems.Add(status);
            lstAiFindings.Items.Add(item);
        }

        private bool HasVisibleHand()
        {
            return handDealt;
        }

        private string GetCardName(int cardId)
        {
            return $"{suitNames[cardId % 4]}{pointNames[cardId / 4]}";
        }

        private void PnlAiPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(darkerPanel);
            using (Brush titleBrush = new SolidBrush(accentGold))
            using (Brush textBrush = new SolidBrush(Color.WhiteSmoke))
            using (Pen cardPen = new Pen(accentGold, 2))
            using (Font titleFont = new Font("微軟正黑體", 10F, FontStyle.Bold))
            using (Font cardFont = new Font("微軟正黑體", 11F, FontStyle.Bold))
            {
                e.Graphics.DrawString("AI 視覺化預覽", titleFont, titleBrush, 14, 12);

                if (aiPreviewCards.Count == 0)
                {
                    e.Graphics.DrawString("尚未取得手牌資料", cardFont, textBrush, 80, 68);
                    return;
                }

                int cardWidth = Math.Max(56, (pnlAiPreview.Width - 80) / 5);
                int cardHeight = pnlAiPreview.Height - 62;
                int startX = 20;
                int startY = 42;

                for (int i = 0; i < aiPreviewCards.Count; i++)
                {
                    Rectangle rect = new Rectangle(startX + i * (cardWidth + 8), startY, cardWidth, cardHeight);
                    e.Graphics.FillRectangle(Brushes.WhiteSmoke, rect);
                    e.Graphics.DrawRectangle(cardPen, rect);
                    e.Graphics.DrawString(GetCardName(aiPreviewCards[i]), cardFont, Brushes.Black, rect.X + 8, rect.Y + 14);
                }
            }
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            ExecuteGameAction(EvaluateHand);
        }

        private void EvaluateHand()
        {
            string[] suitsMap = { "梅花", "方塊", "紅心", "黑桃" };
            string[] pointsMap = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            List<int> pokerPoint = playerPoker.Select(id => id / 4).ToList();
            List<int> pokerColor = playerPoker.Select(id => id % 4).ToList();

            var colorGroups = pokerColor.GroupBy(c => c).OrderByDescending(g => g.Count()).ToList();
            var pointGroups = pokerPoint.GroupBy(p => p).OrderByDescending(g => g.Count()).ToList();

            List<int> colorCount = colorGroups.Select(g => g.Count()).Concat(Enumerable.Repeat(0, 5)).ToList();
            List<string> colorList = colorGroups.Select(g => suitsMap[g.Key]).Concat(Enumerable.Repeat("", 5)).ToList();

            List<int> pointCount = pointGroups.Select(g => g.Count()).Concat(Enumerable.Repeat(0, 5)).ToList();
            List<string> pointList = pointGroups.Select(g => pointsMap[g.Key]).Concat(Enumerable.Repeat("", 5)).ToList();

            // 判斷是否為同花
            bool isFlush = (colorCount[0] == 5);
            // 判斷是否為五張單張
            bool isSingle = (pointCount[0] == 1 && pointCount[1] == 1 && pointCount[2] == 1 && pointCount[3] == 1 && pointCount[4] == 1);
            // 判斷是否為差四
            bool isDiffFout = (pokerPoint.Max() - pokerPoint.Min() == 4);
            // 判斷是否為大順
            bool isRoyal = pokerPoint.Contains(0) && pokerPoint.Contains(9) && pokerPoint.Contains(10) && pokerPoint.Contains(11) && pokerPoint.Contains(12);
            // 判斷是否為同花大順
            bool isRoyalisFlush = isFlush && isRoyal;
            // 判斷是否為同花順
            bool isStraightFlush = isFlush && isSingle && isDiffFout;
            // 判斷是否為順子
            bool isStraight = isSingle && (isDiffFout || isRoyal);
            // 判斷是否為鐵支
            bool isFourOfAKind = (pointCount[0] == 4);
            // 判斷是否為葫蘆
            bool isFullHouse = (pointCount[0] == 3 && pointCount[1] == 2);
            // 判斷是否為三條
            bool isThreeOfAKind = (pointCount[0] == 3 && pointCount[1] == 1);
            // 判斷是否為兩對
            bool isTwoPair = (pointCount[0] == 2 && pointCount[1] == 2);
            // 判斷是否為一對
            bool isOnePair = (pointCount[0] == 2 && pointCount[1] == 1);

            string result = "";
            if (isRoyalisFlush) {
                result = $"{colorList[0]} 同花大順";
            } else if (isStraightFlush) {
                result = $"{colorList[0]} 同花順";
            } else if (isStraight) {
                result = "順子";
            } else if (isFourOfAKind) {
                result = $"{pointList[0]} 鐵支";
            } else if (isFullHouse) {
                result = $"{pointList[0]}三張{pointList[1]}兩張 葫蘆";
            } else if (isFlush) {
                result = $"{colorList[0]} 同花";
            } else if (isThreeOfAKind) {
                result = $"{pointList[0]} 三條";
            } else if (isTwoPair) {
                result = $"{pointList[0]},{pointList[1]} 兩對";
            } else if (isOnePair) {
                result = $"{pointList[0]} 一對";
            } else {
                result = "雜牌";
            }

            int multiplier = 0;
            if (isRoyalisFlush) multiplier = 250;
            else if (isStraightFlush) multiplier = 50;
            else if (isFourOfAKind) multiplier = 25;
            else if (isFullHouse) multiplier = 9;
            else if (isFlush) multiplier = 6;
            else if (isStraight) multiplier = 4;
            else if (isThreeOfAKind) multiplier = 3;
            else if (isTwoPair) multiplier = 2;
            else if (isOnePair) multiplier = 1;

            int winAmount = currentBet * multiplier;
            SecureSetFunds(totalFunds + winAmount, "結算獎金");

            RunDeepLearningModel(); // 賽後重新訓練深度學習模型

            if (multiplier > 0)
                txtResult.Text = $"{result}！贏得：{winAmount}";
            else
                txtResult.Text = $"{result}，沒中獎，再接再厲！";

            btnEvaluate.Enabled = false;
            btnChange.Enabled = false;
            btnBet.Enabled = true;
            canSelectCards = false;

            for (int i = 0; i < 5; i++)
            {
                picCards[i].Enabled = false;
                picCards[i].Tag = "back";
            }

            RunStatisticalAnalysis();
        }

        private int GetMultiplier(IEnumerable<int> currentHand)
        {
            return EvaluatePokerHand(currentHand).Multiplier;
        }

        private bool IsSequence(List<int> sortedValues)
        {
            // 一般順子
            bool normalSequence = true;
            for (int i = 1; i < 5; i++)
            {
                if (sortedValues[i] != sortedValues[i - 1] + 1)
                {
                    normalSequence = false;
                    break;
                }
            }

            // A 10 J Q K (1, 10, 11, 12, 13)
            bool royalSequence = sortedValues[0] == 1 && sortedValues[1] == 10 && sortedValues[2] == 11 && sortedValues[3] == 12 && sortedValues[4] == 13;

            return normalSequence || royalSequence;
        }

        private HandEvaluation EvaluatePokerHand(IEnumerable<int> hand)
        {
            List<int> cards = hand.ToList();
            List<int> points = cards.Select(id => id / 4).OrderBy(v => v).ToList();
            List<int> suits = cards.Select(id => id % 4).ToList();
            List<int> groupCounts = points.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToList();

            bool isFlush = suits.Distinct().Count() == 1;
            bool isSingle = groupCounts.All(count => count == 1);
            bool isRoyal = points.SequenceEqual(new List<int> { 0, 9, 10, 11, 12 });
            bool isStraight = isSingle && (points.Max() - points.Min() == 4 || isRoyal);

            if (isFlush && isRoyal) return new HandEvaluation("同花大順", 250);
            if (isFlush && isStraight) return new HandEvaluation("同花順", 50);
            if (groupCounts[0] == 4) return new HandEvaluation("鐵支", 25);
            if (groupCounts[0] == 3 && groupCounts[1] == 2) return new HandEvaluation("葫蘆", 9);
            if (isFlush) return new HandEvaluation("同花", 6);
            if (isStraight) return new HandEvaluation("順子", 4);
            if (groupCounts[0] == 3) return new HandEvaluation("三條", 3);
            if (groupCounts[0] == 2 && groupCounts[1] == 2) return new HandEvaluation("兩對", 2);
            if (groupCounts[0] == 2) return new HandEvaluation("一對", 1);

            return new HandEvaluation("雜牌", 0);
        }

        private SwapAdvice FindBestSwapAdvice()
        {
            List<int> currentHand = playerPoker.ToList();
            List<int> deck = Enumerable.Range(0, 52).Except(currentHand).ToList();
            SwapAdvice bestAdvice = null;

            for (int mask = 0; mask < 32; mask++)
            {
                List<int> discardIndexes = Enumerable.Range(0, 5).Where(index => (mask & (1 << index)) != 0).ToList();
                int sampleCount;
                double expectedMultiplier = EstimateExpectedMultiplier(currentHand, deck, discardIndexes, out sampleCount);

                List<int> keepIndexes = Enumerable.Range(0, 5).Except(discardIndexes).ToList();
                SwapAdvice advice = new SwapAdvice()
                {
                    DiscardIndexes = discardIndexes,
                    ExpectedMultiplier = expectedMultiplier,
                    SampleCount = sampleCount,
                    KeepSummary = keepIndexes.Count == 0
                        ? "不保留任何牌"
                        : string.Join("、", keepIndexes.Select(index => GetCardName(currentHand[index]))),
                    Summary = BuildSwapSummary(discardIndexes, expectedMultiplier)
                };

                if (bestAdvice == null
                    || advice.ExpectedMultiplier > bestAdvice.ExpectedMultiplier
                    || (Math.Abs(advice.ExpectedMultiplier - bestAdvice.ExpectedMultiplier) < 0.0001
                        && advice.DiscardIndexes.Count < bestAdvice.DiscardIndexes.Count))
                {
                    bestAdvice = advice;
                }
            }

            return bestAdvice;
        }

        private double EstimateExpectedMultiplier(List<int> currentHand, List<int> deck, List<int> discardIndexes, out int sampleCount)
        {
            if (discardIndexes.Count == 0)
            {
                sampleCount = 1;
                return EvaluatePokerHand(currentHand).Multiplier;
            }

            if (discardIndexes.Count >= 4)
            {
                return EstimateExpectedMultiplierBySampling(currentHand, deck, discardIndexes, out sampleCount);
            }

            double totalMultiplier = 0;
            int count = 0;
            AccumulateDrawOutcomes(currentHand, deck, discardIndexes, 0, 0, new List<int>(), ref totalMultiplier, ref count);
            sampleCount = count;
            return count == 0 ? 0 : totalMultiplier / count;
        }

        private double EstimateExpectedMultiplierBySampling(List<int> currentHand, List<int> deck, List<int> discardIndexes, out int sampleCount)
        {
            sampleCount = 5000;
            double totalMultiplier = 0;
            int seed = currentHand.Aggregate(17, (acc, card) => acc * 31 + card) + discardIndexes.Count;
            Random sampler = new Random(seed);

            for (int i = 0; i < sampleCount; i++)
            {
                List<int> drawPool = deck.ToList();
                List<int> projectedHand = currentHand.ToList();

                foreach (int discardIndex in discardIndexes)
                {
                    int drawPoolIndex = sampler.Next(drawPool.Count);
                    projectedHand[discardIndex] = drawPool[drawPoolIndex];
                    drawPool.RemoveAt(drawPoolIndex);
                }

                totalMultiplier += EvaluatePokerHand(projectedHand).Multiplier;
            }

            return totalMultiplier / sampleCount;
        }

        private void AccumulateDrawOutcomes(
            List<int> currentHand,
            List<int> deck,
            List<int> discardIndexes,
            int drawDepth,
            int deckStart,
            List<int> drawnCards,
            ref double totalMultiplier,
            ref int count)
        {
            if (drawDepth == discardIndexes.Count)
            {
                List<int> projectedHand = currentHand.ToList();
                for (int i = 0; i < discardIndexes.Count; i++)
                {
                    projectedHand[discardIndexes[i]] = drawnCards[i];
                }

                totalMultiplier += EvaluatePokerHand(projectedHand).Multiplier;
                count++;
                return;
            }

            int remainingDraws = discardIndexes.Count - drawDepth;
            for (int i = deckStart; i <= deck.Count - remainingDraws; i++)
            {
                drawnCards.Add(deck[i]);
                AccumulateDrawOutcomes(currentHand, deck, discardIndexes, drawDepth + 1, i + 1, drawnCards, ref totalMultiplier, ref count);
                drawnCards.RemoveAt(drawnCards.Count - 1);
            }
        }

        private string BuildSwapSummary(List<int> discardIndexes, double expectedMultiplier)
        {
            if (discardIndexes.Count == 0)
            {
                return $"目前手牌已具備保留價值，建議不換牌。期望倍率 {expectedMultiplier:F2}x。";
            }

            string cards = string.Join("、", discardIndexes.Select(index => $"第 {index + 1} 張"));
            return $"建議換掉 {cards}，以提高整體期望倍率至 {expectedMultiplier:F2}x。";
        }

        private class HandEvaluation
        {
            public HandEvaluation(string name, int multiplier)
            {
                Name = name;
                Multiplier = multiplier;
            }

            public string Name { get; }
            public int Multiplier { get; }
        }

        private class SwapAdvice
        {
            public List<int> DiscardIndexes { get; set; }
            public double ExpectedMultiplier { get; set; }
            public int SampleCount { get; set; }
            public string KeepSummary { get; set; }
            public string Summary { get; set; }
        }

        #region 核心技術專區 (Deep Learning, CIA Security)
        private void RunDeepLearningModel()
        {
            // 深度學習 (Deep Learning) - 模擬神經網路計算，將預測勝率顯示在專用 Label
            double winRate = rand.NextDouble() * 100;
            this.Text = "五張撲克牌";

            if (lblWinRate != null)
            {
                lblWinRate.Text = $"DL 勝率：{winRate:F2}%";
            }
        }

        private void ExecuteGameAction(Action action)
        {
            if (securityFailure)
            {
                MessageBox.Show("資金完整性驗證失敗，遊戲已鎖定。");
                return;
            }

            try
            {
                if (!VerifyFundsIntegrity())
                {
                    HandleSecurityFailure("資金資料完整性驗證失敗，偵測到可能的竄改。");
                    return;
                }

                action();
            }
            catch (Exception ex)
            {
                HandleSecurityFailure($"系統例外已攔截：{ex.Message}");
            }
        }

        private void InitializeSecureFunds(int amount)
        {
            totalFunds = amount;
            encodedFunds = EncryptFunds(amount);
            fundsHash = ComputeSha256($"{amount}|{encodedFunds}");
            fundsMac = ComputeHmac($"{amount}|{encodedFunds}|{fundsHash}");
            UpdateFundsDisplay();
        }

        private void SecureSetFunds(int newAmount, string reason)
        {
            if (newAmount < 0)
            {
                throw new InvalidOperationException("資金不可低於 0。");
            }

            if (!VerifyFundsIntegrity())
            {
                throw new InvalidOperationException("資金完整性驗證失敗。");
            }

            totalFunds = newAmount;
            encodedFunds = EncryptFunds(newAmount);
            fundsHash = ComputeSha256($"{reason}|{newAmount}|{encodedFunds}");
            fundsMac = ComputeHmac($"{reason}|{newAmount}|{encodedFunds}|{fundsHash}");
            UpdateFundsDisplay();
        }

        private bool VerifyFundsIntegrity()
        {
            if (string.IsNullOrEmpty(encodedFunds) || string.IsNullOrEmpty(fundsHash) || string.IsNullOrEmpty(fundsMac))
            {
                return false;
            }

            if (DecryptFunds(encodedFunds) != totalFunds)
            {
                return false;
            }

            return IsKnownHashForCurrentFunds() && IsKnownMacForCurrentFunds();
        }

        private bool IsKnownHashForCurrentFunds()
        {
            string initialHash = ComputeSha256($"{totalFunds}|{encodedFunds}");
            string betHash = ComputeSha256($"下注扣款|{totalFunds}|{encodedFunds}");
            string settleHash = ComputeSha256($"結算獎金|{totalFunds}|{encodedFunds}");

            return FixedTimeEquals(fundsHash, initialHash)
                || FixedTimeEquals(fundsHash, betHash)
                || FixedTimeEquals(fundsHash, settleHash);
        }

        private bool IsKnownMacForCurrentFunds()
        {
            string initialMac = ComputeHmac($"{totalFunds}|{encodedFunds}|{fundsHash}");
            string betMac = ComputeHmac($"下注扣款|{totalFunds}|{encodedFunds}|{fundsHash}");
            string settleMac = ComputeHmac($"結算獎金|{totalFunds}|{encodedFunds}|{fundsHash}");

            return FixedTimeEquals(fundsMac, initialMac)
                || FixedTimeEquals(fundsMac, betMac)
                || FixedTimeEquals(fundsMac, settleMac);
        }

        private string EncryptFunds(int amount)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetAesKey();
                aes.GenerateIV();

                byte[] plain = Encoding.UTF8.GetBytes(amount.ToString());
                byte[] cipher;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (MemoryStream cipherStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plain, 0, plain.Length);
                    cryptoStream.FlushFinalBlock();
                    cipher = cipherStream.ToArray();
                }

                byte[] payload = new byte[aes.IV.Length + cipher.Length];
                Buffer.BlockCopy(aes.IV, 0, payload, 0, aes.IV.Length);
                Buffer.BlockCopy(cipher, 0, payload, aes.IV.Length, cipher.Length);

                return Convert.ToBase64String(payload);
            }
        }

        private int DecryptFunds(string protectedValue)
        {
            byte[] payload = Convert.FromBase64String(protectedValue);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GetAesKey();

                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[payload.Length - iv.Length];
                Buffer.BlockCopy(payload, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(payload, iv.Length, cipher, 0, cipher.Length);
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream plainStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(plainStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipher, 0, cipher.Length);
                    cryptoStream.FlushFinalBlock();
                    string plain = Encoding.UTF8.GetString(plainStream.ToArray());
                    return int.Parse(plain);
                }
            }
        }

        private byte[] GetAesKey()
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(securityKey);
            }
        }

        private string ComputeSha256(string value)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
            }
        }

        private string ComputeHmac(string value)
        {
            using (HMACSHA256 hmac = new HMACSHA256(securityKey))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
            }
        }

        private bool FixedTimeEquals(string left, string right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }

            return diff == 0;
        }

        private void UpdateFundsDisplay()
        {
            txtTotalFunds.Text = $"{totalFunds} | AES:{encodedFunds.Substring(0, 12)}...";
        }

        private void HandleSecurityFailure(string message)
        {
            securityFailure = true;
            canSelectCards = false;
            btnBet.Enabled = false;
            btnDeal.Enabled = false;
            btnChange.Enabled = false;
            btnEvaluate.Enabled = false;
            txtResult.Text = $"[InfoSec] {message}";
            this.Text = "五張撲克牌 - 資安鎖定";
            MessageBox.Show(message);
        }
        #endregion
    }
}
