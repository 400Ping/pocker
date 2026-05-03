# Poker

這是一個使用 C# WinForms 製作的五張撲克牌遊戲。

## 核心功能

### 撲克牌遊戲
- 支援押注、發牌、換牌與牌型判斷。
- 依照牌型倍率計算獎金與更新資金。
- 可點擊牌面選擇要換掉的牌。

### 響應式介面設計 (Responsive UI)
- 支援視窗縮放。撲克牌及主要 UI 控制項會根據可用版面等比例擴展與置中。

### 深度學習 (Deep Learning)
- **智能勝率預測模型**：動態展示運算後的勝率百分比，並且已經實作專屬 UI 標籤顯示於主介面下方，給予最清爽明瞭的輔助參考。

### 資安防護 (InfoSec / CIA Triad)
- **機密性 (Confidentiality)**：所有的資金變動都將進行 Base64/Hash 加密轉換。
- **完整性 (Integrity)**：確保資金在變動過程中未被惡意竄改。
- **可用性 (Availability)**：嚴謹的 Exception Handling 防呆機制，維持系統的可用性。

---
*Developed with C# WinForms*
