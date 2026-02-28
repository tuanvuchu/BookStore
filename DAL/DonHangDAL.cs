using DTO;
using System.Data;

namespace DAL
{
    public class DonHangDAL
    {
        readonly DatabaseHelper databaseHelper = new DatabaseHelper();

        public int LayMaDonHangTiepTheo()
        {
            string sql = "SELECT MAX(CAST(MaDonHang AS INT)) FROM DonHang";
            int ma = databaseHelper.LayGiaTri(sql);
            int maDonHang = ma + 1;
            return maDonHang;
        }
        public int LayMaChiTietTiepTheo()
        {
            string sql = "SELECT MAX(CAST(MaChiTietHoaDon AS INT)) FROM ChiTietDonHang";
            int ma = databaseHelper.LayGiaTri(sql);
            int maChiTietDonHang = ma + 1;
            return maChiTietDonHang;
        }

        public DataTable Hien()
        {
            string sql = "SELECT * FROM ChiTietDonHang WHERE MaDonHang = (SELECT MAX(MaDonHang) FROM ChiTietDonHang)";
            return databaseHelper.GetData(sql);
        }

        public bool ThemDonHang(int maDonHang)
        {
            string sql = string.Format("INSERT INTO DonHang (MaDonHang) VALUES ('{0}')", maDonHang);
            databaseHelper.Chaycodesql(sql);
            return true;
        }

        public bool ThemNgayDonHang(DonHangDTO DH)
        {
            string sql = string.Format("UPDATE DonHang SET NgayDat = '{0}',MaKhachHang = '{1}' WHERE MaDonHang = '{2}'", DH.NgayDat.ToString("yyyy-MM-dd"), DH.MaKhachHang, DH.MaDonHang);
            databaseHelper.Chaycodesql(sql);
            return true;
        }
        public bool ThemChiTiet(DonHangDTO DH)
        {
            int maDonHang = LayMaChiTietTiepTheo();
            string sql = string.Format("insert into ChiTietDonHang values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
              DH.MaDonHang, DH.MaSanPham, DH.SoLuong, DH.Gia, DH.MaNhanVien, DH.MaGiamGia, maDonHang);
            databaseHelper.Chaycodesql(sql);
            return true;
        }
        public bool Suasoluong(SanPhamDTO sp)
        {
            string sql = string.Format("Update SanPham Set SoLuong = '{0}' Where MaSanPham = '{1}'", sp.SoLuong, sp.MaSanPham);
            databaseHelper.Chaycodesql(sql);
            return true;
        }
    }
}
