using BUS;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Doan1
{
    public partial class NhaCungCap : UserControl
    {
        NhaCungCapDTO nhaCungCapDTO = new NhaCungCapDTO();
        NhaCungCapBUS nhaCungCapBUS = new NhaCungCapBUS();
        public NhaCungCap()
        {
            InitializeComponent();
        }
        void loaddgv()
        {
            dgvnhacungcap.DataSource = nhaCungCapBUS.getData();
            dgvnhacungcap.Columns[0].HeaderText = "Mã Nhà Cung Cấp ";
            dgvnhacungcap.Columns[1].HeaderText = "Tên nhà cung cấp";
            dgvnhacungcap.Columns[2].HeaderText = "Địa chỉ";
            dgvnhacungcap.Columns[3].HeaderText = "Sổ điện thoại";
            dgvnhacungcap.Columns[0].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvnhacungcap.Columns[1].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvnhacungcap.Columns[2].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvnhacungcap.Columns[3].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvnhacungcap.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            loaddgv();
            if ((DangNhap.TenDangNhapGlobal == "admin"))
            {
                MessageBox.Show("Chào mừng Quản Lý", "Chào Mừng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                btsua.Enabled = false;
                btthem.Enabled = false;
                btxoa.Enabled = false;
                MessageBox.Show("Bạn đang nhập tài khoản không phải tài khoản quản lý", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvnhacungcap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvnhacungcap.Rows.Count)
            {
                int i = e.RowIndex;
                tbManhacungcap.Text = dgvnhacungcap[0, i].Value.ToString();
                tbTennhacungcap.Text = dgvnhacungcap[1, i].Value.ToString();
                tbDiachi.Text = dgvnhacungcap[2, i].Value.ToString();
                tbSDT.Text = dgvnhacungcap[3, i].Value.ToString();
            }

        }
        private bool ValidatePhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^0\d{9}$");
        }
        private void btthem_Click(object sender, EventArgs e)
        {
            if (!ValidatePhoneNumber(tbSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tbTennhacungcap.Text) || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int count = dgvnhacungcap.Rows.Count;
                if (count > 0)
                {
                    int lastSupplierID = Convert.ToInt32(dgvnhacungcap.Rows[count - 1].Cells[0].Value);
                    int nextSupplierID = lastSupplierID + 1;
                    tbManhacungcap.Text = nextSupplierID.ToString();
                }
                else
                {
                    tbManhacungcap.Text = "1";
                }
                nhaCungCapDTO.MaNhaCungCap = tbManhacungcap.Text;
                nhaCungCapDTO.TenNhaCungCap = tbTennhacungcap.Text;
                nhaCungCapDTO.Diachi = tbDiachi.Text;
                nhaCungCapDTO.SDT = tbSDT.Text;
                string result = nhaCungCapBUS.Themnhacc(nhaCungCapDTO);
                if (result == "Thêm nhà cung cấp thành công.")
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dgvnhacungcap.DataSource = nhaCungCapBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void ClearFields()
        {
            tbManhacungcap.Text = "";
            tbTennhacungcap.Text = "";
            tbDiachi.Text = "";
            tbSDT.Text = "";
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbManhacungcap.Text) || string.IsNullOrEmpty(tbTennhacungcap.Text) || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidatePhoneNumber(tbSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NhaCungCapDTO nhaCungCapDTO = new NhaCungCapDTO();
            nhaCungCapDTO.MaNhaCungCap = tbManhacungcap.Text;
            nhaCungCapDTO.TenNhaCungCap = tbTennhacungcap.Text;
            nhaCungCapDTO.Diachi = tbDiachi.Text;
            nhaCungCapDTO.SDT = tbSDT.Text;
            string currentTen = dgvnhacungcap.CurrentRow.Cells[1].Value.ToString();
            string currentDiaChi = dgvnhacungcap.CurrentRow.Cells[2].Value.ToString();
            string currentSDT = dgvnhacungcap.CurrentRow.Cells[3].Value.ToString();
            if (currentTen == tbTennhacungcap.Text && currentDiaChi == tbDiachi.Text && currentSDT == tbSDT.Text)
            {
                MessageBox.Show("Bạn không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string result = nhaCungCapBUS.Suanhacc(nhaCungCapDTO);
            if (result == "Sửa thông tin nhà cung cấp thành công.")
            {

                MessageBox.Show("Sửa thông tin nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvnhacungcap.DataSource = nhaCungCapBUS.getData();
                ClearFields();
                loaddgv();
            }
            else
            {
                MessageBox.Show("Sửa thông tin nhà cung cấp thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbManhacungcap.Text) || string.IsNullOrEmpty(tbTennhacungcap.Text) || string.IsNullOrEmpty(tbDiachi.Text) || string.IsNullOrEmpty(tbSDT.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maNCC = tbManhacungcap.Text;
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nha cung cấp này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                NhaCungCapDTO nhaCungCapDTO = new NhaCungCapDTO();
                nhaCungCapDTO.MaNhaCungCap = maNCC;

                string result = nhaCungCapBUS.Xoanhacc(nhaCungCapDTO);
                if (result == "Xóa nhà cung cấp thành công.")
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvnhacungcap.DataSource = nhaCungCapBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bttim_Click(object sender, EventArgs e)
        {
            string keyword = tbtim.Text;
            DataTable searchResult = nhaCungCapBUS.TimKiemNhanVien(keyword);

            if (searchResult.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy nhà cung cấp nào phù hợp với từ khóa đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dgvnhacungcap.DataSource = searchResult;
            }
        }

        private void btmoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            loaddgv();
        }

        private void tbTennhacungcap_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTennhacungcap.Text))
            {
                errorProvider2.SetError(tbTennhacungcap, "Không được bỏ trống tên nhà cung cấp!");
            }
            else
            {
                errorProvider2.SetError(tbTennhacungcap, null);
            }
        }

        private void tbDiachi_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDiachi.Text))
            {
                errorProvider3.SetError(tbDiachi, "Không được bỏ trống địa chỉ !");
            }
            else
            {
                errorProvider3.SetError(tbDiachi, null);
            }
        }

        private void tbSDT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSDT.Text))
            {
                errorProvider4.SetError(tbSDT, "Không được bỏ trống số điện thoại !");
            }
            else
            {
                errorProvider4.SetError(tbSDT, null);
            }
        }
    }
}
