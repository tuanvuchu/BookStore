using DTO;
using System.Data;

namespace DAL
{
    public class LuongDAL
    {
        DatabaseHelper csdl = new DatabaseHelper();
        public DataTable getData()
        {
            string sql = "Select * from BangLuong";
            return csdl.GetData(sql);

        }
        public DataTable getData1()
        {
            string sql = "select MaNhanVien from NhanVien";
            return csdl.GetData(sql);
        }


        public int LayMaLuongTiepTheo()
        {
            string sql = "SELECT MAX(CAST (MaBangLuong AS INT)) FROM BangLuong";
            int maBangLuonght = csdl.LayGiaTri(sql);
            int maBangLuongMoi = maBangLuonght + 1;
            return maBangLuongMoi;
        }

        public bool ThemLuong(NhanVienDTO nv)
        {
            int maBangLuongMoi = LayMaLuongTiepTheo();
            string sql = string.Format("INSERT INTO BangLuong VALUES ('{0}', '{1}', '{2}', '{3}')", maBangLuongMoi, nv.MaNhanVien, nv.Luong, nv.NgayNhanLuong.ToString("yyyy-MM-dd"));
            csdl.Chaycodesql(sql);
            return true;
        }

        public bool SuaLuong(NhanVienDTO nv)
        {
            string sql = string.Format("UPDATE BangLuong SET NgayNhanLuong = '{0}', TongLuong = '{1}' WHERE MaBangLuong = '{2}'", nv.NgayNhanLuong.ToString("yyyy-MM-dd"), nv.Luong, nv.MaBangLuong);
            csdl.Chaycodesql(sql);
            return true;

        }

        public bool XoaLuong(NhanVienDTO nv)
        {
            string sql = string.Format("DELETE FROM BangLuong WHERE MaBangLuong = '{0}'", nv.MaBangLuong);
            csdl.Chaycodesql(sql);
            return true;
        }
        public DataTable TimKiemLuong(string keyword)
        {
            string sql = string.Format("SELECT * FROM BangLuong WHERE MaNhanVien LIKE '%{0}%' ", keyword);
            return csdl.GetData(sql);
        }
    }
}
