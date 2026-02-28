using BUS;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Doan1
{
    public partial class NhanVien : UserControl
    {
        NhanVienBUS nhanVienBUS = new NhanVienBUS();
        NhanVienDTO nhanVienDTO = new NhanVienDTO();
        public NhanVien()
        {
            InitializeComponent();
        }
        void loaddgv()
        {
            dgvNhanvien.DataSource = nhanVienBUS.getData();
            dgvNhanvien.Columns[0].HeaderText = "Mã NV ";
            dgvNhanvien.Columns[1].HeaderText = "Tên NV";
            dgvNhanvien.Columns[2].HeaderText = "Giới Tính";
            dgvNhanvien.Columns[3].HeaderText = "Ngày sinh";
            dgvNhanvien.Columns[4].HeaderText = "Địa Chỉ";
            dgvNhanvien.Columns[5].HeaderText = "SĐT";
            dgvNhanvien.Columns[6].HeaderText = "Email";
            dgvNhanvien.Columns[0].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[1].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[2].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[3].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[4].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[5].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.Columns[6].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvNhanvien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
        }

        private void Nhanvien_Load(object sender, EventArgs e)
        {
            loaddgv();
            tbMa.ReadOnly = true;
            if ((DangNhap.TenDangNhapGlobal == "admin"))
            {
                MessageBox.Show("Chào mừng Quản Lý", "Chào Mừng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                btSua.Enabled = false;
                bttThem.Enabled = false;
                btXoa.Enabled = false;
                guna2Button1.Enabled = false;
                guna2Button2.Enabled = false;
                MessageBox.Show("Bạn đang nhập tài khoản không phải tài khoản quản lý", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        public void bttThem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tbTen.Text) ||
                cbGioiTinh.SelectedItem == null ||
                string.IsNullOrEmpty(tbDiachi.Text) ||
                string.IsNullOrEmpty(tbSDT.Text) ||
                string.IsNullOrEmpty(tbEmail.Text) ||
                !ValidatePhoneNumber(tbSDT.Text) ||
                !ValidateEmail(tbEmail.Text))
            {
                if (string.IsNullOrEmpty(tbTen.Text) || cbGioiTinh.SelectedItem == null || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text) || string.IsNullOrEmpty(tbEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidatePhoneNumber(tbSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!ValidateEmail(tbEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Email phải có định dạng @gmail.com.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            else
            {
                int count = dgvNhanvien.Rows.Count;
                if (count > 0)
                {
                    int lastEmployeeID = Convert.ToInt32(dgvNhanvien.Rows[count - 1].Cells[0].Value);
                    int nextEmployeeID = lastEmployeeID + 1;
                    tbMa.Text = nextEmployeeID.ToString();
                }
                else
                {
                    tbMa.Text = "1";
                }
                nhanVienDTO.HoTen = tbTen.Text;
                nhanVienDTO.Gioitinh = cbGioiTinh.SelectedItem.ToString();
                nhanVienDTO.Ngaysinh = dtNgay.Value;
                nhanVienDTO.Diachi = tbDiachi.Text;
                nhanVienDTO.Sodienthoai = tbSDT.Text;
                nhanVienDTO.Email = tbEmail.Text;

                string result = nhanVienBUS.Themnv(nhanVienDTO);
                if (result == "Đăng kí thành công")
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvNhanvien.DataSource = nhanVienBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearFields()
        {
            tbMa.Text = "";
            tbTen.Text = "";
            cbGioiTinh.SelectedIndex = -1;
            dtNgay.Value = DateTime.Now;
            tbDiachi.Text = "";
            tbSDT.Text = "";
            tbEmail.Text = "";
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTen.Text) || cbGioiTinh.SelectedItem == null || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text) || string.IsNullOrEmpty(tbEmail.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            nhanVienDTO.MaNhanVien = tbMa.Text;
            nhanVienDTO.HoTen = tbTen.Text;
            nhanVienDTO.Gioitinh = cbGioiTinh.SelectedItem.ToString();
            nhanVienDTO.Ngaysinh = dtNgay.Value;
            nhanVienDTO.Diachi = tbDiachi.Text;
            nhanVienDTO.Sodienthoai = tbSDT.Text;
            nhanVienDTO.Email = tbEmail.Text;
            string currentTen = dgvNhanvien.CurrentRow.Cells[1].Value.ToString();
            string currentGioiTinh = dgvNhanvien.CurrentRow.Cells[2].Value.ToString();
            DateTime currentNgaySinh = (DateTime)dgvNhanvien.CurrentRow.Cells[3].Value;
            string currentDiaChi = dgvNhanvien.CurrentRow.Cells[4].Value.ToString();
            string currentSDT = dgvNhanvien.CurrentRow.Cells[5].Value.ToString();
            string currentEmail = dgvNhanvien.CurrentRow.Cells[6].Value.ToString();
            if (currentTen == tbTen.Text && currentGioiTinh == cbGioiTinh.SelectedItem.ToString() && currentNgaySinh == dtNgay.Value &&
                currentDiaChi == tbDiachi.Text && currentSDT == tbSDT.Text && currentEmail == tbEmail.Text)
            {
                MessageBox.Show("Bạn không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string result = nhanVienBUS.Suanv(nhanVienDTO);
            if (result == "Sửa thành công")
            {
                MessageBox.Show("Sửa thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvNhanvien.DataSource = nhanVienBUS.getData();
                ClearFields();
                loaddgv();
            }
            else
            {
                MessageBox.Show("Sửa thông tin nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbMa.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maNV = tbMa.Text;
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                string result = nhanVienBUS.Xoanv(maNV);
                if (result == "Xóa nhân viên thành công")
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvNhanvien.DataSource = nhanVienBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btTimkiem_Click(object sender, EventArgs e)
        {
            string keyword = tbtimkiem.Text;

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable searchResult = nhanVienBUS.TimKiemNhanVien(keyword);

                if (searchResult.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp với từ khóa đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dgvNhanvien.DataSource = searchResult;
                }
            }
        }
        private void btMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            loaddgv();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ChamCong cc = new ChamCong();
            cc.Show();
        }
        private void dgvNhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanvien.Rows.Count)
            {
                int i = e.RowIndex;
                tbMa.Text = dgvNhanvien[0, i].Value.ToString();
                tbTen.Text = dgvNhanvien[1, i].Value.ToString();
                cbGioiTinh.Text = dgvNhanvien[2, i].Value.ToString();
                dtNgay.Text = dgvNhanvien[3, i].Value.ToString();
                tbDiachi.Text = dgvNhanvien[4, i].Value.ToString();
                tbSDT.Text = dgvNhanvien[5, i].Value.ToString();
                tbEmail.Text = dgvNhanvien[6, i].Value.ToString();
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            LuongNhanVien l = new LuongNhanVien();
            l.Show();
        }
    }
}