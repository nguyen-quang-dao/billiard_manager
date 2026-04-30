using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GUI
{
    public class FrmTableManager : Form
    {
        private TableBLL tableBLL = new TableBLL();
        private UserDTO currentUser;

        DataGridView dgv;
        TextBox txtName, txtPrice;
        Button btnAdd, btnUpdate, btnDelete;

        public FrmTableManager(UserDTO user)
        {
            currentUser = user;
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            this.Text = "Quản lý bàn";
            this.Size = new Size(750, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);

            Panel card = new Panel();
            card.Size = new Size(700, 420);
            card.BackColor = Color.FromArgb(32, 32, 32);
            card.Location = new Point(20, 20);
            card.Padding = new Padding(15);
            this.Controls.Add(card);

            Label title = new Label();
            title.Text = "QUẢN LÝ BÀN";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            title.Location = new Point(10, 5);
            card.Controls.Add(title);

            // ===== TABLE =====
            dgv = new DataGridView();
            dgv.SetBounds(10, 40, 660, 200);

            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;

            dgv.RowTemplate.Height = 40;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(60, 60, 60);

            dgv.BackgroundColor = Color.FromArgb(40, 40, 40);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            dgv.DefaultCellStyle.Padding = new Padding(5);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 32, 32);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 45;

            dgv.EnableHeadersVisualStyles = false;

            dgv.CellClick += Dgv_CellClick;

            card.Controls.Add(dgv);

            // ===== INPUT =====
            Label lblName = new Label()
            {
                Text = "Tên bàn",
                ForeColor = Color.White,
                Left = 20,
                Top = 260
            };

            txtName = new TextBox()
            {
                Left = 120,
                Top = 255,
                Width = 220
            };

            Label lblPrice = new Label()
            {
                Text = "Giá",
                ForeColor = Color.White,
                Left = 20,
                Top = 310
            };

            txtPrice = new TextBox()
            {
                Left = 120,
                Top = 305,
                Width = 220
            };

            card.Controls.Add(lblName);
            card.Controls.Add(txtName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(txtPrice);

            // ===== BUTTON =====
            btnAdd = CreateButton("Thêm", Color.FromArgb(0, 150, 100), 260);
            btnUpdate = CreateButton("Sửa", Color.FromArgb(59, 130, 246), 310);
            btnDelete = CreateButton("Xóa", Color.FromArgb(200, 60, 60), 360);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            card.Controls.Add(btnAdd);
            card.Controls.Add(btnUpdate);
            card.Controls.Add(btnDelete);
        }

        private Button CreateButton(string text, Color color, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Left = 380;
            btn.Top = top;
            btn.Width = 120;
            btn.Height = 35;
            btn.FlatStyle = FlatStyle.Flat;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(
                    Math.Min(color.R + 30, 255),
                    Math.Min(color.G + 30, 255),
                    Math.Min(color.B + 30, 255));
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = color;
            };

            return btn;
        }

        private void LoadData()
        {
            var data = tableBLL.GetTables();

            dgv.DataSource = null;
            dgv.DataSource = data;

            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["TableId"].HeaderText = "ID";
                dgv.Columns["TableName"].HeaderText = "Tên bàn";
                dgv.Columns["PricePerHour"].HeaderText = "Giá";
                dgv.Columns["Status"].Visible = false;
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.CurrentRow == null) return;

            txtName.Text = dgv.CurrentRow.Cells["TableName"].Value?.ToString();
            txtPrice.Text = dgv.CurrentRow.Cells["PricePerHour"].Value?.ToString();
        }

        private int GetId()
        {
            if (dgv.CurrentRow == null) return -1;
            return Convert.ToInt32(dgv.CurrentRow.Cells["TableId"].Value);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                double price = double.Parse(txtPrice.Text);
                tableBLL.AddTable(txtName.Text, price, currentUser);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int id = GetId();
                if (id == -1) return;

                double price = double.Parse(txtPrice.Text);
                tableBLL.UpdateTable(id, txtName.Text, price, currentUser);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int id = GetId();
                if (id == -1) return;

                tableBLL.DeleteTable(id, currentUser);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}