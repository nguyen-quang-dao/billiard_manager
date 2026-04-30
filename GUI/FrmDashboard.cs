using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmDashboard : Form
    {
        TableBLL tableBLL = new TableBLL();
        BillBLL billBLL = new BillBLL();
        private UserDTO currentUser;
        private Button btnAdmin;
        private Panel header;
        private Panel sidebar;
        private Panel mainPanel;
        private FlowLayoutPanel tablePanel;

        public FrmDashboard(UserDTO user)
        {
            InitializeComponent();
            currentUser = user;
            InitUI();
            this.Load += FrmDashboard_Load;
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void InitUI()
        {
            this.Text = "Billiard Manager";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);

            // SIDEBAR
            sidebar = new Panel
            {
                Width = 200,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(32, 32, 32)
            };

            Label logo = new Label
            {
                Text = "🎱 Billiard",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            sidebar.Controls.Add(logo);

            btnAdmin = new Button();
            btnAdmin.Text = "Quản lý giá";
            btnAdmin.Width = 160;
            btnAdmin.Height = 40;
            btnAdmin.Top = 100;
            btnAdmin.Left = 20;
            btnAdmin.BackColor = Color.Gray;
            btnAdmin.ForeColor = Color.White;

            sidebar.Controls.Add(btnAdmin);
            if (currentUser.Role == 0)
            {
                btnAdmin.Visible = false;
            }

            // HEADER
            header = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            Label title = new Label
            {
                Text = "Dashboard",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Left,
                Padding = new Padding(20, 15, 0, 0)
            };

            header.Controls.Add(title);

            // MAIN
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(24, 24, 24)
            };

            tablePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true,
                WrapContents = true
            };

            mainPanel.Controls.Add(tablePanel);

            // ADD
            this.Controls.Add(mainPanel);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }

        private void LoadTables()
        {
            tablePanel.Controls.Clear();

            var tables = tableBLL.GetTables();

            foreach (var t in tables)
            {
                var bill = billBLL.GetOpenBill(t.TableId);

                Button btn = new Button
                {
                    Width = 150,
                    Height = 120,
                    Margin = new Padding(10),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btn.FlatAppearance.BorderSize = 0;

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
                        FrmPayment f = new FrmPayment(t.TableId, currentUser);
                        f.ShowDialog();
                    }

                    LoadTables();
                };
                btnAdmin.Click += (s, e) =>
                {
                    FrmTableManager f = new FrmTableManager(currentUser);
                    f.ShowDialog();
                    LoadTables();
                };
                tablePanel.Controls.Add(btn);
            }
        }
    }
}