using BUS;
using ClosedXML.Excel;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Doan1
{
    public partial class LuongNhanVien : Form
    {
        LuongBUS luongBUS = new LuongBUS();
        NhanVienDTO nhanVienDTO = new NhanVienDTO();
        public LuongNhanVien()
        {
            InitializeComponent();
        }


        private void LoadDataIntoComboBox()
        {
            DataTable dataTable = luongBUS.getData1();
            cbbMaNhanVien.DataSource = dataTable;
            cbbMaNhanVien.DisplayMember = "MaNhanVien";
            cbbMaNhanVien.ValueMember = "MaNhanVien";
        }
        void loaddgv()
        {
            dgvLuong.DataSource = luongBUS.getData();
            dgvLuong.Columns[0].HeaderText = "Mã Lương ";
            dgvLuong.Columns[1].HeaderText = "Mã Nhân Viên";
            dgvLuong.Columns[2].HeaderText = "Lương";
            dgvLuong.Columns[3].HeaderText = "Ngày Nhận Lương";
            dgvLuong.Columns[0].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvLuong.Columns[1].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvLuong.Columns[2].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvLuong.Columns[3].DefaultCellStyle.Font = new Font("Arial", 10);
            dgvLuong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
        }

        private void LuongNhanVien_Load(object sender, EventArgs e)
        {
            loaddgv();
            LoadDataIntoComboBox();
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            int count = dgvLuong.Rows.Count;

            if (count > 0)
            {
                int lastEmployeeID = Convert.ToInt32(dgvLuong.Rows[count - 1].Cells[0].Value);
                int nextEmployeeID = lastEmployeeID + 1;
                tbMaLuong.Text = nextEmployeeID.ToString();
            }
            else
            {
                tbMaLuong.Text = "1";
            }
            nhanVienDTO.MaBangLuong = tbMaLuong.Text;
            nhanVienDTO.MaNhanVien = cbbMaNhanVien.Text;
            nhanVienDTO.Luong = tbLuong.Text;
            nhanVienDTO.NgayNhanLuong = dtpngay.Value;
            string result = luongBUS.Themnv(nhanVienDTO);
            if (result == "1")
            {
                MessageBox.Show("Thêm Lương nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvLuong.DataSource = luongBUS.getData();
                ClearFields();
                loaddgv();
            }
            else
            {
                MessageBox.Show("Thêm Lương nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFields()
        {

            dtpngay.Value = DateTime.Now;
            tbMaLuong.Text = "";
            cbbMaNhanVien.Text = "";
            tbLuong.Text = "";
        }

        private void dgvLuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvLuong.Rows.Count)
            {
                int i = e.RowIndex;
                tbMaLuong.Text = dgvLuong[0, i].Value.ToString();
                cbbMaNhanVien.Text = dgvLuong[1, i].Value.ToString();
                tbLuong.Text = dgvLuong[2, i].Value.ToString();
                dtpngay.Text = dgvLuong[3, i].Value.ToString();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            nhanVienDTO.MaBangLuong = tbMaLuong.Text;
            nhanVienDTO.MaNhanVien = cbbMaNhanVien.Text;
            nhanVienDTO.Luong = tbLuong.Text;
            nhanVienDTO.NgayNhanLuong = dtpngay.Value;
            string result = luongBUS.SuaLuong(nhanVienDTO);
            if (result == "1")
            {
                MessageBox.Show($"Sửa thông tin lương nhân viên: {nhanVienDTO.MaNhanVien} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvLuong.DataSource = luongBUS.getData();
                ClearFields();
                loaddgv();
            }
            else
            {
                MessageBox.Show("Sửa thông tin lương nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            string maBangLuong = tbMaLuong.Text;
            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa lương nhân viên: {nhanVienDTO.MaNhanVien} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                nhanVienDTO.MaBangLuong = maBangLuong;
                string result = luongBUS.XoaLuong(nhanVienDTO);
                if (result == "1")
                {
                    MessageBox.Show($"Xóa thông tin lương nhân viên thành công");
                    dgvLuong.DataSource = luongBUS.getData();
                    ClearFields();
                    loaddgv();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ptb2_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string keyword = tbtimkiem.Text;
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable searchResult = luongBUS.TimKiemLuong(keyword);
                if (searchResult.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp với từ khóa đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dgvLuong.DataSource = searchResult;
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dataTable = luongBUS.getData();
                            workbook.Worksheets.Add(dataTable, "BangLuong");
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Lưu thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error occurred: " + ex.Message);
                    }
                }
            }
        }
    }
}
