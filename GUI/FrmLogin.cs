using System;
using System.Drawing;
using System.Windows.Forms;
using DTO;
using BLL;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        private UserBLL bll = new UserBLL();

        public FrmLogin()
        {
            // bỏ Designer để tránh chồng UI
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "Login";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 23, 42);

            Panel card = new Panel();
            card.Size = new Size(350, 400);
            card.BackColor = Color.FromArgb(30, 41, 59);
            card.Location = new Point(
                (this.ClientSize.Width - card.Width) / 2,
                (this.ClientSize.Height - card.Height) / 2
            );
            card.Anchor = AnchorStyles.None;
            this.Controls.Add(card);

            Label title = new Label();
            title.Text = "BILLIARD\nMANAGEMENT";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            title.Size = new Size(300, 80);
            title.Location = new Point(25, 20);
            card.Controls.Add(title);

            TextBox txtUser = new TextBox();
            txtUser.PlaceholderText = "Username";
            txtUser.Size = new Size(300, 30);
            txtUser.Location = new Point(25, 120);
            card.Controls.Add(txtUser);

            TextBox txtPass = new TextBox();
            txtPass.PlaceholderText = "Password";
            txtPass.UseSystemPasswordChar = true;
            txtPass.Size = new Size(300, 30);
            txtPass.Location = new Point(25, 170);
            card.Controls.Add(txtPass);

            Button btnLogin = new Button();
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.BackColor = Color.FromArgb(59, 130, 246);
            btnLogin.ForeColor = Color.White;
            btnLogin.Size = new Size(300, 40);
            btnLogin.Location = new Point(25, 230);
            btnLogin.FlatStyle = FlatStyle.Flat;

            btnLogin.Click += (s, e) =>
            {
                var user = bll.Login(txtUser.Text, txtPass.Text);

                if (user == null)
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                    return;
                }

                FrmDashboard f = new FrmDashboard(user);
                f.Show();
                this.Hide();
            };

            card.Controls.Add(btnLogin);
        }
    }
}