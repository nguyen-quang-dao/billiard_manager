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
        private BillBLL billBLL = new BillBLL();
        private BillDTO bill;
        private System.Windows.Forms.Timer timer; 
        private Label lblTable, lblStart, lblTime, lblTotal;
        private Button btnPay;

        public FrmPayment(int tableId)
        {
            this.tableId = tableId;
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            this.Text = "Thanh toán";
            this.Size = new Size(350, 280);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            lblTable = CreateLabel(20);
            lblStart = CreateLabel(60);
            lblTime = CreateLabel(100);
            lblTotal = CreateLabel(140);

            btnPay = new Button
            {
                Text = "THANH TOÁN",
                BackColor = Color.Red,
                ForeColor = Color.White,
                Width = 250,
                Height = 40,
                Location = new Point(50, 190),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnPay.Click += BtnPay_Click;

            this.Controls.AddRange(new Control[] {
                lblTable, lblStart, lblTime, lblTotal, btnPay
            });
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
            billBLL.Pay(tableId);
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