using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ThongKeDAL
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();

        public int KhachHang()
        {
            string sql = "SELECT COUNT(*) FROM KhachHang";
            return databaseHelper.LayGiaTri(sql);
        }

        public int NhanVien()
        {
            string sql = "SELECT COUNT(*) FROM NhanVien";
            return databaseHelper.LayGiaTri(sql);
        }
        public int SanPham()
        {
            string sql = "SELECT COUNT(*) FROM SanPham";
            return databaseHelper.LayGiaTri(sql);
        }
        public object GetTongDoanhThu(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT SUM(TongTien) AS TongDoanhThu FROM DonHang WHERE NgayDat >= @StartDate AND NgayDat <= @EndDate";
            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            return databaseHelper.ExecuteScalar(sql, parameters);
        }
        public DataTable GetTopSanPhamBanChay(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT TOP 5 SP.TenSanPham AS ProductName, SUM(CTDH.SoLuong) AS QtySold " +
                         "FROM ChiTietDonHang CTDH " +
                         "JOIN SanPham SP ON CTDH.MaSanPham = SP.MaSanPham " +
                         "JOIN DonHang DH ON CTDH.MaDonHang = DH.MaDonHang " +
                         "WHERE DH.NgayDat >= @StartDate AND DH.NgayDat <= @EndDate " +
                         "GROUP BY SP.TenSanPham " +
                         "ORDER BY SUM(CTDH.SoLuong) DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            return databaseHelper.GetData(sql, parameters);
        }
    }
}