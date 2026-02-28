using DTO;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class SanPhamDAL
    {
        DatabaseHelper csdl = new DatabaseHelper();
        public string connectionString = @"Data Source=localhost,1433;Initial Catalog=da1;User ID=sa;Password=MsSQL2022@;";
        public DataTable getData()
        {
            string sql = "Select MaSanPham, TenSanPham, MaNhaCungCap, Gia, MoTa, SoLuong, HinhAnh from SanPham";
            return csdl.GetData(sql);
        }
        public DataTable Lay()
        {
            string sql = "Select MaSanPham, TenSanPham, MaNhaCungCap, Gia, MoTa, SoLuong, HinhAnh from SanPham";
            return csdl.GetData(sql);
        }

        public DataTable getData1()
        {
            string sql = "SELECT * FROM NhaCungCap";
            return csdl.GetData(sql);
        }

        public DataTable LayDanhSachMaVaTenSanPham()
        {
            string sql = "SELECT MaSanPham, TenSanPham, SoLuong, Gia FROM SanPham";
            return csdl.GetData(sql);
        }

        public int LayMaSanPhamTiepTheo()
        {
            string sql = "SELECT MAX(CAST (MaSanPham AS INT)) FROM SanPham";
            int maSanPhamHienTai = csdl.LayGiaTri(sql);
            int maSanPhamMoi = maSanPhamHienTai + 1;
            return maSanPhamMoi;
        }

        public int kiemtramatrung(string ma)
        {
            string sql = "Select count(*) from SanPham where MaSanPham='" + ma.Trim() + "'";
            return csdl.KiemTraMaTrung(ma, sql);
        }

        public bool Themsp(SanPhamDTO sp)
        {
            int maSanPhamMoi = LayMaSanPhamTiepTheo();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO SanPham (MaSanPham, TenSanPham, Gia, MoTa, MaNhaCungCap, SoLuong, HinhAnh) 
                   VALUES (@MaSanPham, @TenSanPham, @Gia, @MoTa, @MaNhaCungCap, @SoLuong, @HinhAnh)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSanPham", maSanPhamMoi);
                    cmd.Parameters.AddWithValue("@TenSanPham", sp.TenSanPham);
                    cmd.Parameters.AddWithValue("@Gia", sp.Gia);
                    cmd.Parameters.AddWithValue("@MoTa", sp.Mota);
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", sp.MaNhaCungCap);
                    cmd.Parameters.AddWithValue("@SoLuong", sp.SoLuong);
                    cmd.Parameters.AddWithValue("@HinhAnh", sp.HinhAnh);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool Suasp(SanPhamDTO sp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE SanPham 
                       SET TenSanPham = @TenSanPham, 
                           Gia = @Gia, 
                           MoTa = @MoTa, 
                           MaNhaCungCap = @MaNhaCungCap, 
                           SoLuong = @SoLuong, 
                           HinhAnh = @HinhAnh 
                       WHERE MaSanPham = @MaSanPham";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenSanPham", sp.TenSanPham);
                    cmd.Parameters.AddWithValue("@Gia", sp.Gia);
                    cmd.Parameters.AddWithValue("@MoTa", sp.Mota);
                    cmd.Parameters.AddWithValue("@MaNhaCungCap", sp.MaNhaCungCap);
                    cmd.Parameters.AddWithValue("@SoLuong", sp.SoLuong);
                    cmd.Parameters.AddWithValue("@HinhAnh", sp.HinhAnh);
                    cmd.Parameters.AddWithValue("@MaSanPham", sp.MaSanPham);
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }


        public bool Xoanv(SanPhamDTO sp)
        {
            string sql = string.Format("Delete from SanPham Where MaSanPham = '{0}'", sp.MaSanPham);
            csdl.Chaycodesql(sql);
            return true;
        }

        public DataTable TimKiemSP(string keyword)
        {
            string sql = string.Format("Select * from SanPham Where MaSanPham like '%{0}%' OR TenSanPham like '%{0}%'", keyword);
            return csdl.GetData(sql);
        }
        public bool Suasoluong(int olodo, int olodo1)
        {
            string sql1 = string.Format("UPDATE SanPham SET SoLuong =  {0} WHERE MaSanPham = '{1}'", olodo1, olodo);
            csdl.Chaycodesql(sql1);
            return true;
        }
    }
}
