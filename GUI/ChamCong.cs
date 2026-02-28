using BUS;
using ClosedXML.Excel;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace Doan1
{
    public partial class ChamCong : Form
    {
        ChamCongBUS chamCongBUS = new ChamCongBUS();
        ChamCongDTO chamCongDTO = new ChamCongDTO();

        public ChamCong()
        {
            InitializeComponent();
        }
        void loaddgv()
        {
            dgvchamcong.DataSource = chamCongBUS.GetData();
            dgvchamcong.Columns[0].HeaderText = "Mã Chấm Công ";
            dgvchamcong.Columns[1].HeaderText = "Mã NV";
            dgvchamcong.Columns[2].HeaderText = "Ngày Chấm Công";
            dgvchamcong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);

        }
        private void LoadDataIntoComboBox()
        {
            DataTable dataTable = chamCongBUS.GetData1();
            cbbnhanvien.DataSource = dataTable;
            cbbnhanvien.DisplayMember = "MaNhanVien";
            cbbnhanvien.ValueMember = "MaNhanVien";
        }

        private void ChamCong_Load(object sender, EventArgs e)
        {
            loaddgv();
            LoadDataIntoComboBox();
        }

        private void dgvchamcong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int i = e.RowIndex;
                tbmacc.Text = dgvchamcong[0, i].Value.ToString();
                dtpngay.Text = dgvchamcong[2, i].Value.ToString();
                cbbnhanvien.Text = dgvchamcong[1, i].Value.ToString();
            }
        }
        private void btthem_Click(object sender, EventArgs e)
        {
            int count = dgvchamcong.Rows.Count;
            if (count > 0)
            {
                int lastEmployeeID = Convert.ToInt32(dgvchamcong.Rows[count - 1].Cells[0].Value);
                int nextEmployeeID = lastEmployeeID + 1;
                tbmacc.Text = nextEmployeeID.ToString();
            }
            else
            {
                tbmacc.Text = "1";
            }

            chamCongDTO.Mavn = cbbnhanvien.SelectedValue.ToString();
            chamCongDTO.Ngaycc = dtpngay.Value;
            string result = chamCongBUS.Themcc(chamCongDTO);
            if (result == "Thêm chấm công thành công!")
            {
                MessageBox.Show("Thêm chấm công thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvchamcong.DataSource = chamCongBUS.GetData();
                loaddgv();
                ClearFields();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFields()
        {
            tbmacc.Text = "";
            dtpngay.Value = DateTime.Now;
            cbbnhanvien.Text = "";
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            string maCC = tbmacc.Text;
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa chấm công này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                chamCongDTO.Macc = maCC;

                string result = chamCongBUS.Xoacc(chamCongDTO);
                if (result == "Xóa chấm công thành công!")
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvchamcong.DataSource = chamCongBUS.GetData();
                    loaddgv();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string maNhanVien = cbbnhanvien.Text;
            DateTime ngayChamCong = dtpngay.Value;
            DataTable result = chamCongBUS.TimKiemCC(maNhanVien, ngayChamCong);
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (result.Rows.Count > 0)
            {
                dgvchamcong.DataSource = result;
            }
            else
            {
                string message = "Nhân viên " + maNhanVien + " không có chấm công trong ngày này.";
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dataTable = chamCongBUS.GetData();
                            workbook.Worksheets.Add(dataTable, "NhanVien");
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Lưu thành công!", ShowIcon.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhanVien nv = new NhanVien();
            nv.Show();
        }
        private void Ptb2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
