using BUS;
using ClosedXML.Excel;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Doan1
{
    public partial class Mua : UserControl
    {
        readonly SanPhamBUS sanPhamBUS = new SanPhamBUS();
        readonly ChamCongBUS chamCongBUS = new ChamCongBUS();
        readonly DonHangBUS donHangBUS = new DonHangBUS();
        readonly DonHangDTO donHangDTO = new DonHangDTO();
        readonly SanPhamDTO sanPhamDTO = new SanPhamDTO();
        public Mua()
        {
            InitializeComponent();
        }

        void Loaddgv()
        {
            dgvSanpham.DataSource = sanPhamBUS.Lay();
            dgvSanpham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14);
            foreach (DataGridViewColumn column in dgvSanpham.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Arial", 14);
            }
            dgvGioHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvGioHang.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            foreach (DataGridViewColumn column in dgvGioHang.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Arial", 10);
            }
            dtNgay.Value = DateTime.Now;
        }

        public void LoadDataIntoComboBox()
        {
            DataTable dataTable = chamCongBUS.GetData1();
            cbbmanhanvien.DataSource = dataTable;
            cbbmanhanvien.DisplayMember = "HoTen";
            cbbmanhanvien.ValueMember = "MaNhanVien";
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGiamGia");
            dt.Columns.Add("GiaTri");
            dt.Rows.Add("1", "Giờ hot");
            dt.Rows.Add("2", "Ưu đãi lớn");
            dt.Rows.Add("3", "Giảm giá trong ngày");
            dt.Rows.Add("4", "Mừng đại lễ");
            cbbmagiamgia.DataSource = dt;
            cbbmagiamgia.DisplayMember = "GiaTri";
            cbbmagiamgia.ValueMember = "MaGiamGia";
            DataTable dataTable3 = donHangBUS.HienKh();
            cbbkh.DataSource = dataTable3;
            cbbkh.DisplayMember = "HoTen";
            cbbkh.ValueMember = "MaKhachHang";
        }

        public void MUA_Load(object sender, EventArgs e)
        {
            if (dgvSanpham != null)
            {
                Loaddgv();
            }
            LoadDataIntoComboBox();
            dgvGioHang.CellValueChanged += DgvGioHang_CellValueChanged;
        }

        public void DgvSanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSanpham.Columns[e.ColumnIndex].Name == "Column13")
            {
                int soLuongTrongKho = Convert.ToInt32(dgvSanpham.Rows[e.RowIndex].Cells["Column4"].Value);
                if (soLuongTrongKho == 0)
                {
                    MessageBox.Show("Sản phẩm này đã hết hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                them.Enabled = true;
                NUDSoluong.Enabled = true;

                dgvSanpham.Columns["Column13"].Visible = false;
                dgvGioHang.Rows.Add(dgvSanpham.Rows[e.RowIndex].Cells["Column1"].Value.ToString(),
                    dgvSanpham.Rows[e.RowIndex].Cells["Column2"].Value.ToString(),
                    dgvSanpham.Rows[e.RowIndex].Cells["Column3"].Value.ToString(), 0);

            }

            if (e.RowIndex >= 0 && e.RowIndex < dgvSanpham.Rows.Count)
            {
                DataGridViewRow hang = dgvSanpham.Rows[e.RowIndex];
                tbsLtrongkho.Text = hang.Cells["Column4"].Value.ToString();
                tbmasp.Text = hang.Cells["Column2"].Value.ToString();
            }
        }
        public void Guna2Button1_Click(object sender, EventArgs e)
        {
            dgvGioHang.Rows.Clear();

        }
        public void Guna2Button6_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            donHangDTO.MaChiTietHoaDon = tbhoadon.Text;
                            DataTable dataTable = donHangBUS.Hien();
                            string sheetName = tbhoadon.Text;
                            workbook.Worksheets.Add(dataTable, sheetName);
                            workbook.SaveAs(saveFileDialog.FileName);
                        }
                        MessageBox.Show("Lưu thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private int i;
        public void DgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGioHang.Columns[e.ColumnIndex].Name == "Column10")
            {
                dgvGioHang.Rows.RemoveAt(e.RowIndex);
                dgvSanpham.Columns["Column13"].Visible = true;
                them.Enabled = false;
                tbsLtrongkho.Text = string.Empty;
                tbmasp.Text = string.Empty;
                soluongmua = Convert.ToInt32(dgvSanpham[3, i].Value.ToString());
            }
        }
        public int TinhTongTien()
        {
            int tongTien = 0;
            foreach (DataGridViewRow row in dgvGioHang.Rows)
            {
                if (!row.IsNewRow)
                {
                    int giaTriSanPham = Convert.ToInt32(row.Cells["Column8"].Value);
                    int soLuong = Convert.ToInt32(row.Cells["Column9"].Value);
                    if (soLuong > 0)
                    {
                        tongTien += giaTriSanPham * soLuong;
                    }
                }
            }
            string maGiamGia = cbbmagiamgia.SelectedValue.ToString();
            switch (maGiamGia)
            {
                case "1":
                    tongTien -= 10000;
                    break;
                case "2":
                    tongTien -= 25000;
                    break;
                case "3":
                    tongTien -= 50000;
                    break;
                case "4":
                    tongTien -= 100000;
                    break;
                default:
                    break;
            }
            if (tongTien < 0)
            {
                tongTien = 0;
            }
            tbTong.Text = tongTien.ToString();
            return tongTien;
        }
        public void TbSoluong_ValueChanged(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow != null && dgvGioHang.CurrentRow.Cells["Column9"].Value != null)
            {
                int giaTriMoi = (int)NUDSoluong.Value;

                int soLuongHienTai = Convert.ToInt32(dgvGioHang.CurrentRow.Cells["Column9"].Value);
                if (giaTriMoi > soLuongHienTai)
                {
                    dgvGioHang.CurrentRow.Cells["Column9"].Value = soLuongHienTai + 1;
                }
                else if (giaTriMoi < soLuongHienTai)
                {
                    dgvGioHang.CurrentRow.Cells["Column9"].Value = soLuongHienTai - 1;
                }
            }
        }

        int t;
        public void Guna2Button4_Click(object sender, EventArgs e)
        {
            t = donHangBUS.LayMaDonHangTiepTheo();
            if (donHangBUS.ThemDonHang(t))
            {
                dgvGioHang.Columns["Column10"].Visible = true;
                dgvSanpham.Columns["Column13"].Visible = true;
                p.Visible = true;
                them.Visible = true;
                cbbmagiamgia.Enabled = true;
                dtNgay.Enabled = true;
                tbhoadon.Enabled = true;
                tbhoadon.Text = t.ToString();
                cbbmanhanvien.Enabled = true;
                btIn.Enabled = true;
                bttaomoi.Enabled = false;
            }
            else
            {
                MessageBox.Show("Thêm đơn hàng thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int soluongmua;
        public void Guna2Button2_Click(object sender, EventArgs e)
        {
            if (NUDSoluong.Value == 0)
            {
                MessageBox.Show("Vui lòng chọn số lượng sản phẩm để mua.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btThanhToan.Enabled = true;
            them.Enabled = false;
            dgvSanpham.Columns["Column13"].Visible = true;
            donHangDTO.MaGiamGia = cbbmagiamgia.SelectedValue.ToString();
            donHangDTO.MaDonHang = tbhoadon.Text;
            int rowIndex = dgvGioHang.CurrentCell.RowIndex;
            donHangDTO.MaSanPham = dgvGioHang.Rows[rowIndex].Cells["Column7"].Value.ToString();
            donHangDTO.SoLuong = dgvGioHang.Rows[rowIndex].Cells["Column9"].Value.ToString();
            donHangDTO.MaNhanVien = cbbmanhanvien.SelectedValue.ToString();
            donHangDTO.Gia = tbTong.Text;
            donHangDTO.NgayDat = dtNgay.Value;
            donHangDTO.MaKhachHang = cbbkh.SelectedValue.ToString();
            int.TryParse(tbsLtrongkho.Text, out int sltrongkho);
            int.TryParse(donHangDTO.SoLuong, out int soluongmua);
            if (soluongmua > sltrongkho)
            {
                DialogResult dialogResult = MessageBox.Show("Số lượng mua vượt quá số lượng trong kho!", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    dgvGioHang.Rows.Clear();
                }
                return;
            }
            string result3 = donHangBUS.ThemNgayDat(donHangDTO);
            if (donHangBUS.ThemChiTiet(donHangDTO))
            {
                MessageBox.Show("Thông tin sản phẩm vào Giỏ Hàng Thành Công!", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                dgvGioHang.Rows.Clear();
                NUDSoluong.Enabled = false;
                NUDSoluong.Value = 0;
            }

            int slmoi = sltrongkho - soluongmua;
            sanPhamDTO.SoLuong = slmoi.ToString();
            sanPhamDTO.MaSanPham = tbmasp.Text;
            if (donHangBUS.Suasoluong(sanPhamDTO))
            {
                MessageBox.Show($"Số lượng sản phẩm trong kho hiện tại là: {slmoi}");
                Loaddgv();
                tbsLtrongkho.Text = string.Empty;
                tbmasp.Text = string.Empty;
            }
        }
        public void DgvGioHang_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvGioHang.Columns["Column9"].Index)
            {
                TinhTongTien();
                soluongmua = Convert.ToInt32(dgvGioHang.Rows[e.RowIndex].Cells["Column9"].Value);
            }
        }
        private void Cbbmagiamgia_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }
        private void btThanhToan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanh toán thành công!");
        }
    }
}
