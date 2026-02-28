using BUS;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Doan1
{
    public partial class SanPham : UserControl
    {
        SanPhamBUS sanPhamBUS = new SanPhamBUS();
        SanPhamDTO sanPhamDTO = new SanPhamDTO();
        private byte[] bytes;
        public SanPham()
        {
            InitializeComponent();
        }
        private void Check()
        {
            foreach (DataGridViewRow row in dgvSanpham.Rows)
            {
                if (row.Cells["Column6"] != null && row.Cells["Column6"].Value != null)
                {
                    int quantity = Convert.ToInt32(row.Cells["Column6"].Value);
                    if (quantity == 0)
                    {
                        string productName = row.Cells["Column2"].Value.ToString();
                        MessageBox.Show($"Sản phẩm '{productName}' đang hết hàng. Hãy nhập thêm sản phẩm ở chức năng nhập kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        void Loaddgv()
        {
            dgvSanpham.DataSource = sanPhamBUS.getData();
            dgvSanpham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
            foreach (DataGridViewColumn column in dgvSanpham.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Arial", 10);
            }
            Check();
        }

        private void LoadDataIntoComboBox()
        {
            DataTable dataTable = sanPhamBUS.getData1();
            tbMacc.DataSource = dataTable;
            tbMacc.DisplayMember = "TenNhaCungCap";
            tbMacc.ValueMember = "MaNhaCungCap";
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            Loaddgv();
            LoadDataIntoComboBox();
        }

        private void DgvSanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSanpham.Rows.Count)
            {
                int i = e.RowIndex;
                tbMasp.Text = dgvSanpham[0, i].Value?.ToString();
                tbTensp.Text = dgvSanpham[1, i].Value?.ToString();
                tbMacc.SelectedValue = dgvSanpham[2, i].Value?.ToString();
                tbGia.Text = dgvSanpham[3, i].Value?.ToString();
                tbMota.Text = dgvSanpham[4, i].Value?.ToString();
                tbSoluong.Text = dgvSanpham[5, i].Value?.ToString();

                if (dgvSanpham[6, i].Value != DBNull.Value)
                {
                    bytes = (byte[])dgvSanpham[6, i].Value;
                    using (MemoryStream memoryStream = new MemoryStream(bytes))
                    {
                        guna2PictureBox1.Image = Image.FromStream(memoryStream);
                    }
                }
                else
                {
                    guna2PictureBox1.Image = null;
                }
            }
        }

        private void bttThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTensp.Text) || string.IsNullOrWhiteSpace(tbGia.Text) ||
                string.IsNullOrWhiteSpace(tbMota.Text) || string.IsNullOrWhiteSpace(tbMacc.Text) ||
                string.IsNullOrWhiteSpace(tbSoluong.Text))
            {
                MessageBox.Show("Không được bỏ trống các trường.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int count = dgvSanpham.Rows.Count;
            if (count > 0)
            {
                int lastEmployeeID = Convert.ToInt32(dgvSanpham.Rows[count - 1].Cells[0].Value);
                int nextEmployeeID = lastEmployeeID + 1;
                string newEmployeeID = nextEmployeeID.ToString();
                tbMasp.Text = newEmployeeID;
            }
            else
            {
                tbMasp.Text = "1";
            }
            sanPhamDTO.MaSanPham = tbMasp.Text;
            sanPhamDTO.TenSanPham = tbTensp.Text;
            sanPhamDTO.Gia = tbGia.Text;
            sanPhamDTO.Mota = tbMota.Text;
            sanPhamDTO.MaNhaCungCap = tbMacc.SelectedValue.ToString();
            sanPhamDTO.SoLuong = tbSoluong.Text;
            sanPhamDTO.HinhAnh = bytes;
            string result = sanPhamBUS.Themsp(sanPhamDTO);
            if (result == "Thêm thành công")
            {
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvSanpham.DataSource = sanPhamBUS.getData();
                ClearFields();
            }

            else
            {
                MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFields()
        {
            tbMasp.Text = "";
            tbTensp.Text = "";
            tbGia.Text = "";
            tbMota.Text = "";
            tbMacc.Text = "";
            tbSoluong.Text = "1";
            guna2PictureBox1.Image = null;
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMasp.Text) ||
                string.IsNullOrWhiteSpace(tbTensp.Text) ||
                string.IsNullOrWhiteSpace(tbGia.Text) ||
                string.IsNullOrWhiteSpace(tbMota.Text) ||
                string.IsNullOrWhiteSpace(tbMacc.Text) ||
                string.IsNullOrWhiteSpace(tbSoluong.Text))
            {
                MessageBox.Show("Không được bỏ trống các trường.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sanPhamDTO.MaSanPham = tbMasp.Text;
            sanPhamDTO.TenSanPham = tbTensp.Text;
            sanPhamDTO.Gia = tbGia.Text;
            sanPhamDTO.Mota = tbMota.Text;
            sanPhamDTO.MaNhaCungCap = tbMacc.SelectedValue.ToString();
            sanPhamDTO.SoLuong = tbSoluong.Text;
            sanPhamDTO.HinhAnh = bytes;
            string result = sanPhamBUS.Suasp(sanPhamDTO);
            if (result == "Sửa thành công")
            {
                MessageBox.Show("Sửa thông tin Sản Phẩn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvSanpham.DataSource = sanPhamBUS.getData();
                ClearFields();
                Loaddgv();
            }
            else
            {
                MessageBox.Show("Sửa thông tin Sản Phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMasp.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maSp = tbMasp.Text;
            sanPhamDTO.MaSanPham = maSp;

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                string result = sanPhamBUS.Xoasp(sanPhamDTO);
                if (result == "Xóa thành công")
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvSanpham.DataSource = sanPhamBUS.getData();
                    ClearFields();
                    Loaddgv();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ClearFields();
            Loaddgv();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhapKho nk = new NhapKho();
            nk.ShowDialog();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string keyword = guna2TextBox1.Text;
            DataTable searchResult = sanPhamBUS.Timsp(keyword);

            if (searchResult.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy Sản Phẩm nào phù hợp với từ khóa đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dgvSanpham.DataSource = searchResult;
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ảnh (*.png;*.jpg)|*.png;*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                byte[] imageBytes = File.ReadAllBytes(filePath);
                bytes = imageBytes;
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    guna2PictureBox1.Image = Image.FromStream(memoryStream);
                }
            }
        }

        private void tbGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
