using DTO;
using System.Data;

namespace DAL
{
    public class NhanvienDAL
    {
        DatabaseHelper csdl = new DatabaseHelper();
        public DataTable getData()
        {
            string sql = "Select * from NhanVien";
            return csdl.GetData(sql);
        }

        public int LayMaNhanVienTiepTheo()
        {
            string sql = "SELECT MAX(CAST(MaNhanVien AS INT)) FROM NhanVien";
            int maNhanVienHienTai = csdl.LayGiaTri(sql);
            int maNhanVienMoi = maNhanVienHienTai + 1;
            return maNhanVienMoi;
        }

        public int kiemtramatrung(string ma)
        {
            string sql = "Select count(*) from NhanVien where MaNhanVien='" + ma.Trim() + "'";
            return csdl.KiemTraMaTrung(ma, sql);
        }
        public bool Themnv(NhanVienDTO nv)
        {

            int maNhanVienMoi = LayMaNhanVienTiepTheo();
            string sql = string.Format("Insert into NhanVien values('{0}',N'{1}', N'{2}', '{3}', N'{4}', '{5}', N'{6}')", maNhanVienMoi, nv.HoTen, nv.Gioitinh, nv.Ngaysinh.ToString("yyyy-MM-dd"), nv.Diachi, nv.Sodienthoai, nv.Email);
            csdl.Chaycodesql(sql);
            return true;
        }
        public bool Suanv(NhanVienDTO nv)
        {
            string sql = string.Format("UPDATE NhanVien SET HoTen = N'{0}', GioiTinh = N'{1}', NgaySinh = '{2}', DiaChi = N'{3}', SoDienThoai = '{4}', Email = N'{5}' WHERE MaNhanVien = '{6}'", nv.HoTen, nv.Gioitinh, nv.Ngaysinh.ToString("yyyy-MM-dd"), nv.Diachi, nv.Sodienthoai, nv.Email, nv.MaNhanVien);
            csdl.Chaycodesql(sql);
            return true;
        }
        public bool Xoanv(string ma)
        {
            string sql = string.Format("DELETE FROM NhanVien WHERE MaNhanVien = '{0}'", ma);
            csdl.Chaycodesql(sql);
            return true;
        }

        public DataTable TimKiemNhanVien(string keyword)
        {
            string sql = string.Format("SELECT * FROM NhanVien WHERE MaNhanVien LIKE '%{0}%' OR HoTen LIKE N'%{0}%'", keyword);
            return csdl.GetData(sql);
        }
    }
}
