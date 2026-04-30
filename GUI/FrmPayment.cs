using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmPayment : Form
    {
        private int tableId;
        private UserDTO currentUser;
        private BillBLL billBLL = new BillBLL();
        private BillDTO bill;
        private System.Windows.Forms.Timer timer; 
        private Label lblTable, lblStart, lblTime, lblTotal;
        private Button btnPay;

        public FrmPayment(int tableId, UserDTO user)
        {
            this.tableId = tableId;
            this.currentUser = user;
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            this.Text = "Thanh toán";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);

            // ===== CARD =====
            Panel card = new Panel();
            card.Size = new Size(340, 240);
            card.BackColor = Color.FromArgb(32, 32, 32);
            card.Location = new Point(25, 25);
            card.Padding = new Padding(10);
            this.Controls.Add(card);

            // ===== TITLE =====
            Label title = new Label();
            title.Text = "THANH TOÁN";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(110, 10);
            card.Controls.Add(title);

            // ===== LABEL =====
            lblTable = CreateLabel(50);
            lblStart = CreateLabel(80);
            lblTime = CreateLabel(110);
            lblTotal = CreateLabel(140);

            lblTotal.ForeColor = Color.FromArgb(0, 200, 150);
            lblTotal.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            card.Controls.Add(lblTable);
            card.Controls.Add(lblStart);
            card.Controls.Add(lblTime);
            card.Controls.Add(lblTotal);

            // ===== BUTTON =====
            btnPay = new Button
            {
                Text = "THANH TOÁN",
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
                Width = 260,
                Height = 40,
                Location = new Point(40, 180),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            btnPay.MouseEnter += (s, e) =>
                btnPay.BackColor = Color.FromArgb(230, 80, 80);

            btnPay.MouseLeave += (s, e) =>
                btnPay.BackColor = Color.FromArgb(200, 60, 60);

            btnPay.Click += BtnPay_Click;

            card.Controls.Add(btnPay);
        }

        private Label CreateLabel(int top)
        {
            return new Label
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Left = 20,
                Top = top
            };
        }

        private void LoadData()
        {
            bill = billBLL.GetOpenBill(tableId);

            if (bill == null)
            {
                MessageBox.Show("Không có bill!");
                this.Close();
                return;
            }

            lblTable.Text = $"Bàn: {bill.TableId}";
            lblStart.Text = $"Bắt đầu: {bill.StartTime:HH:mm}";
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;

            timer.Tick += (s, e) =>
            {
                TimeSpan time = DateTime.Now - bill.StartTime;
                double total = billBLL.CalculateTotal(bill.StartTime);

                lblTime.Text = $"Thời gian: {time.Hours}h {time.Minutes}p";
                lblTotal.Text = $"Tổng tiền: {total:N0}đ";
            };

            timer.Start();
            UpdateUI();
        }

        private void UpdateUI()
        {
            TimeSpan time = DateTime.Now - bill.StartTime;
            double total = billBLL.CalculateTotal(bill.StartTime);

            lblTime.Text = $"Thời gian: {time.Hours}h {time.Minutes}p";
            lblTotal.Text = $"Tổng tiền: {total:N0}đ";
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            billBLL.Pay(tableId, currentUser);
            timer?.Stop();

            MessageBox.Show("Thanh toán thành công!");
            this.Close();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer?.Stop();
            base.OnFormClosing(e);
        }
    }
}