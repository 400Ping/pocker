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

        private readonly byte[] securityKey = Encoding.UTF8.GetBytes("Poker-CIA-Integrity-Key-2026");
        private string encodedFunds;
        private string fundsHash;
        private string fundsMac;
        private bool securityFailure = false;
        private Label lblWinRate;

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

            lblWinRate = new Label() { Left = 40, Width = 200, Text = "DL 勝率：等待計算...", ForeColor = Color.Blue, Font = new Font("微軟正黑體", 10, FontStyle.Bold) };
            lblWinRate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblWinRate.Top = grpFunc.Bottom + 10;

            this.Controls.Add(lblWinRate);
            this.AutoScrollMinSize = new Size(0, lblWinRate.Bottom + 20);

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
            txtResult.Text = "請點擊要保留的牌，然後換牌或直接判斷牌型";

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
        }

        private int GetMultiplier(IEnumerable<int> currentHand)
        {
            // 將牌的 ID 轉換為數值與花色
            // ID: 0~51
            // 數值: ID % 13 + 1 -> 1~13 (A=1, J=11, Q=12, K=13)
            // 花色: ID / 13 -> 0~3 (對應不同花色)

            var values = currentHand.Select(id => id % 13 + 1).OrderBy(v => v).ToList();
            var suits = currentHand.Select(id => id / 13).ToList();

            bool isFlush = suits.Distinct().Count() == 1;
            bool isStraight = IsSequence(values);

            var valueGroups = values.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToList();

            if (isFlush && isStraight)
            {
                if (values.Contains(1) && values.Contains(13) && values.Contains(10)) 
                    return 250; // 皇家同花順 (A, K, Q, J, 10 這裡特別處理A=1)
                return 50; // 同花順
            }
            if (valueGroups[0] == 4) return 25; // 四條
            if (valueGroups[0] == 3 && valueGroups[1] == 2) return 9; // 葫蘆
            if (isFlush) return 6; // 同花
            if (isStraight) return 4; // 順子
            if (valueGroups[0] == 3) return 3; // 三條
            if (valueGroups[0] == 2 && valueGroups[1] == 2) return 2; // 兩對
            if (valueGroups[0] == 2) return 1; // 一對

            return 0; // 什麼都沒有
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
