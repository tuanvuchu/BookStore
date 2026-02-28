using BUS;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Doan1
{
    public partial class KhachHang : UserControl
    {
        private string olodo;
        KhachHangDTO khachHangDTO = new KhachHangDTO();
        KhachHangBUS khachHangBUS = new KhachHangBUS();
        public KhachHang()
        {
            InitializeComponent();
        }
        void loaddgv()
        {
            dgvkhachhang.DataSource = khachHangBUS.getData();
            dgvkhachhang.Columns[0].HeaderText = "Mã Khách Hàng";
            dgvkhachhang.Columns[1].HeaderText = "Tên Khách Hàng";
            dgvkhachhang.Columns[2].HeaderText = "Địa chỉ";
            dgvkhachhang.Columns[3].HeaderText = "Sổ điện thoại";
            dgvkhachhang.Columns[4].HeaderText = "Email";
            dgvkhachhang.Columns[0].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvkhachhang.Columns[1].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvkhachhang.Columns[2].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvkhachhang.Columns[3].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvkhachhang.Columns[4].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvkhachhang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
        }

        private void KhachHang_Load(object sender, EventArgs e)
        {
            loaddgv();
            tbMa.ReadOnly = true;
        }

        private void dgvkhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvkhachhang.Rows.Count)
            {
                int i = e.RowIndex;
                olodo = dgvkhachhang[0, i].Value.ToString();
                tbMa.Text = dgvkhachhang[0, i].Value.ToString();
                tbTen.Text = dgvkhachhang[1, i].Value.ToString();
                tbDiachi.Text = dgvkhachhang[2, i].Value.ToString();
                tbSDT.Text = dgvkhachhang[3, i].Value.ToString();
                tbEmail.Text = dgvkhachhang[4, i].Value.ToString();
            }
        }
        private bool ValidatePhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^0\d{9}$");
        }

        private bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w\.-]+@gmail\.com$");
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTen.Text) ||
                string.IsNullOrEmpty(tbDiachi.Text) ||
                string.IsNullOrEmpty(tbSDT.Text) ||
                string.IsNullOrEmpty(tbEmail.Text) ||
                !ValidatePhoneNumber(tbSDT.Text) ||
                !ValidateEmail(tbEmail.Text))
            {
                if (string.IsNullOrEmpty(tbTen.Text) || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text) || string.IsNullOrEmpty(tbEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!ValidatePhoneNumber(tbSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!ValidateEmail(tbEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Email phải có định dạng @gmail.com.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            else
            {
                int count = dgvkhachhang.Rows.Count;
                if (count > 0)
                {
                    int lastCustomerID = Convert.ToInt32(dgvkhachhang.Rows[count - 1].Cells[0].Value);
                    int nextCustomerID = lastCustomerID + 1;
                    tbMa.Text = nextCustomerID.ToString();
                }
                else
                {
                    tbMa.Text = "1";
                }
                khachHangDTO.MaKhachHang = tbMa.Text;
                khachHangDTO.HoTen = tbTen.Text;
                khachHangDTO.DiaChi = tbDiachi.Text;
                khachHangDTO.SDT = tbSDT.Text;
                khachHangDTO.Email = tbEmail.Text;

                string result = khachHangBUS.Themhk(khachHangDTO);
                if (result == "Thêm thành công.")
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvkhachhang.DataSource = khachHangBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearFields()
        {
            tbMa.Text = "";
            tbTen.Text = "";
            tbDiachi.Text = "";
            tbSDT.Text = "";
            tbEmail.Text = "";
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbMa.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            khachHangDTO.MaKhachHang = olodo;
            khachHangDTO.HoTen = tbTen.Text;
            khachHangDTO.DiaChi = tbDiachi.Text;
            khachHangDTO.SDT = tbSDT.Text;
            khachHangDTO.Email = tbEmail.Text;
            string currentTen = dgvkhachhang.CurrentRow.Cells[1].Value.ToString();
            string currentDiaChi = dgvkhachhang.CurrentRow.Cells[2].Value.ToString();
            string currentSDT = dgvkhachhang.CurrentRow.Cells[3].Value.ToString();
            string currentEmail = dgvkhachhang.CurrentRow.Cells[4].Value.ToString();

            if (currentTen == tbTen.Text && currentDiaChi == tbDiachi.Text && currentSDT == tbSDT.Text && currentEmail == tbEmail.Text)
            {
                MessageBox.Show("Bạn không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string result = khachHangBUS.Suakh(khachHangDTO);
            if (result == "Sửa thông tin thành công.")
            {

                MessageBox.Show("Sửa thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvkhachhang.DataSource = khachHangBUS.getData();
                ClearFields();
                loaddgv();
            }
            else
            {
                MessageBox.Show("Sửa thông tin khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbMa.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKh = tbMa.Text;
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                khachHangDTO.MaKhachHang = maKh;
                string result = khachHangBUS.XoanKH(khachHangDTO);
                if (result == "Xóa khách hàng thành công.")
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvkhachhang.DataSource = khachHangBUS.getData();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btmoi_Click(this, EventArgs.Empty);
                }
            }
        }

        private void btmoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            loaddgv();
        }

        private void bttim_Click(object sender, EventArgs e)
        {
            string keyword = tbtim.Text;
            DataTable searchResult = khachHangBUS.TimKiemKhachHang(keyword);

            if (searchResult.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khách hàng nào phù hợp với từ khóa đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dgvkhachhang.DataSource = searchResult;
            }
        }
    }
}
