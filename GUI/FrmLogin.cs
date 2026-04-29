using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        UserBLL bll = new UserBLL();
        public FrmLogin()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "Login";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Background
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackColor = Color.FromArgb(15, 23, 42);

            // Panel card
            Panel card = new Panel();
            card.Size = new Size(350, 400);
            card.BackColor = Color.FromArgb(30, 41, 59);
            card.Location = new Point((this.Width - card.Width) / 2, (this.Height - card.Height) / 2);
            this.Controls.Add(card);

            // Title
            Label title = new Label();
            title.Text = "BILLIARD\nMANAGEMENT";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            title.AutoSize = false;
            title.Size = new Size(300, 80);
            title.Location = new Point(25, 20);
            card.Controls.Add(title);

            // Username
            TextBox txtUser = new TextBox();
            txtUser.PlaceholderText = "Username";
            txtUser.Size = new Size(300, 30);
            txtUser.Location = new Point(25, 120);
            card.Controls.Add(txtUser);

            // Password
            TextBox txtPass = new TextBox();
            txtPass.PlaceholderText = "Password";
            txtPass.UseSystemPasswordChar = true;
            txtPass.Size = new Size(300, 30);
            txtPass.Location = new Point(25, 170);
            card.Controls.Add(txtPass);

            // Button login
            Button btnLogin = new Button();
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.BackColor = Color.FromArgb(59, 130, 246);
            btnLogin.ForeColor = Color.White;
            btnLogin.Size = new Size(300, 40);
            btnLogin.Location = new Point(25, 230);
            btnLogin.FlatStyle = FlatStyle.Flat;

            btnLogin.Click += (s, e) =>
            {
                bool ok = bll.Login(txtUser.Text, txtPass.Text);

                if (ok)
                {
                    MessageBox.Show("Đăng nhập thành công");

                    this.Hide();                 
                    FrmDashboard f = new FrmDashboard();
                    f.Show();                    
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                }
            };
            card.Controls.Add(btnLogin);
        }
    }
}