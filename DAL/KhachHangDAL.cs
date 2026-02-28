using DTO;
using System.Data;

namespace DAL
{
    public class KhachHangDAL
    {
        DatabaseHelper csdl = new DatabaseHelper();
        public DataTable getData()
        {
            string sql = "Select * from KhachHang";
            return csdl.GetData(sql);
        }
        public int LayMaKhachHangTiepTheo()
        {
            string sql = "SELECT MAX(CAST(MaKhachHang AS INT)) FROM KhachHang";
            int maKhachHangHienTai = csdl.LayGiaTri(sql);
            int maKhachHangMoi = maKhachHangHienTai + 1;
            return maKhachHangMoi;
        }
        public int Kiemtramatrung(string ma)
        {
            string sql = "Select count(*) from KhachHang where MaKhachHang='" + ma.Trim() + "'";
            return csdl.KiemTraMaTrung(ma, sql);
        }
        public bool ThemKhachHang(KhachHangDTO HK)
        {
            int maKhachHangMoi = LayMaKhachHangTiepTheo();
            string sql = string.Format("INSERT INTO KhachHang VALUES ('{0}', N'{1}', N'{2}', '{3}', N'{4}')",
                maKhachHangMoi, HK.HoTen, HK.DiaChi, HK.SDT, HK.Email);
            csdl.Chaycodesql(sql);
            return true;

        }
        public bool SuaKhachHang(KhachHangDTO hk)
        {
            string sql = string.Format("UPDATE KhachHang SET HoTen = N'{0}', DiaChi = N'{1}', SDT = '{2}',Email=N'{3}' WHERE MaKhachHang = '{4}'", hk.HoTen, hk.DiaChi, hk.SDT, hk.Email, hk.MaKhachHang);
            csdl.Chaycodesql(sql);
            return true;
        }
        public bool XoaKhachHang(string ma)
        {
            string sql = string.Format("DELETE FROM KhachHang WHERE MaKhachHang = '{0}'", ma);
            csdl.Chaycodesql(sql);
            return true;
        }
        public DataTable TimKiem(string keyword)
        {
            string sql = string.Format("SELECT * FROM KhachHang WHERE MaKhachHang LIKE '%{0}%' OR HoTen LIKE '%{0}%'", keyword);
            return csdl.GetData(sql);
        }
    }
}
