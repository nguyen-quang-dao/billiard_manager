using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using DTO;
using System.Collections.Generic;

namespace GUI
{
    public partial class FrmDashboard : Form
    {
        TableBLL tableBLL = new TableBLL();

        public FrmDashboard()
        {
            InitializeComponent();
            InitUI();
            this.Load += FrmDashboard_Load;
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            LoadTables();
        }
        private Panel header;
        private Panel sidebar;
        private Panel mainPanel;
        private FlowLayoutPanel tablePanel;

        private void InitUI()
        {
            this.Text = "Billiard Manager";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);

            // ===== SIDEBAR =====
            sidebar = new Panel();
            sidebar.Width = 200;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(32, 32, 32);

            Label logo = new Label();
            logo.Text = "🎱 Billiard";
            logo.ForeColor = Color.White;
            logo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            logo.Dock = DockStyle.Top;
            logo.Height = 60;
            logo.TextAlign = ContentAlignment.MiddleCenter;

            sidebar.Controls.Add(logo);

            // ===== HEADER =====
            header = new Panel();
            header.Height = 60;
            header.Dock = DockStyle.Top;
            header.BackColor = Color.FromArgb(40, 40, 40);

            Label title = new Label();
            title.Text = "Dashboard";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            title.Dock = DockStyle.Left;
            title.Padding = new Padding(20, 15, 0, 0);

            header.Controls.Add(title);

            // ===== MAIN =====
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(24, 24, 24);

            tablePanel = new FlowLayoutPanel();
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Padding = new Padding(20);
            tablePanel.AutoScroll = true;
            tablePanel.WrapContents = true;

            mainPanel.Controls.Add(tablePanel);

            // ===== ADD =====
            this.Controls.Add(mainPanel);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }

        private void LoadTables()
        {
            tablePanel.Controls.Clear();

            var tables = tableBLL.GetTables();
            BillBLL billBLL = new BillBLL();

            foreach (var t in tables)
            {
                var bill = billBLL.GetOpenBill(t.TableId);

                Button btn = new Button();
                btn.Width = 150;
                btn.Height = 120;

                if (bill == null)
                {
                    btn.Text = $"{t.TableName}\nTrống";
                    btn.BackColor = Color.Green;
                }
                else
                {
                    btn.BackColor = Color.Red;
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1000;

                    timer.Tick += (s, e) =>
                    {
                        TimeSpan time = DateTime.Now - bill.StartTime;
                        double total = billBLL.CalculateTotal(bill.StartTime);

                        btn.Text = $"{t.TableName}\n{time.Hours}h {time.Minutes}p\n{total:N0}đ";
                    };

                    timer.Start();
                }

                btn.Click += (s, e) =>
                {
                    var currentBill = billBLL.GetOpenBill(t.TableId);

                    if (currentBill == null)
                    {
                        billBLL.StartTable(t.TableId);
                    }
                    else
                    {
                        billBLL.Pay(t.TableId);
                    }

                    LoadTables();
                };
                tablePanel.Controls.Add(btn);
            }
        }
        private Button CreateTableButton(TableDTO t)
        {
            Button btn = new Button();

            btn.Width = 150;
            btn.Height = 100;
            btn.Margin = new Padding(15);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.ForeColor = Color.White;

            string statusText = t.Status == 0 ? "Trống" : "Đang chơi";

            btn.Text = $"🎱 {t.TableName}\n{statusText}";

            Color idle = Color.FromArgb(0, 150, 100);
            Color busy = Color.FromArgb(200, 60, 60);

            btn.BackColor = t.Status == 0 ? idle : busy;

            // hover effect
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = t.Status == 0
                    ? Color.FromArgb(0, 180, 120)
                    : Color.FromArgb(230, 80, 80);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = t.Status == 0 ? idle : busy;
            };

            btn.Click += (s, e) =>
            {
                MessageBox.Show($"Bạn chọn {t.TableName}");
            };

            return btn;
        }
    }
}