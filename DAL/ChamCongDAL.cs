using DTO;
using System;
using System.Data;

namespace DAL
{
    public class ChamCongDAL
    {
        DatabaseHelper cSDL_DAL = new DatabaseHelper();
        public DataTable getData()
        {
            string sql = "Select * from ChamCong";
            return cSDL_DAL.GetData(sql);
        }
        public DataTable getData1()
        {
            string sql = "SELECT * FROM NhanVien";
            return cSDL_DAL.GetData(sql);
        }

        public int LayMaCCTiepTheo()
        {
            string sql = "SELECT MAX(CAST(Macc AS INT)) FROM ChamCong";
            int maBangLuonght = cSDL_DAL.LayGiaTri(sql);
            int mamoi = maBangLuonght + 1;
            return mamoi;
        }

        public bool ThemCC(ChamCongDTO cc)
        {
            int mamoi = LayMaCCTiepTheo();
            string sql = string.Format("Insert into ChamCong values('{0}','{1}', '{2}')", mamoi, cc.Mavn, cc.Ngaycc.ToString("yyyy-MM-dd"));
            cSDL_DAL.Chaycodesql(sql);
            return true;
        }
        public bool XoaCC(ChamCongDTO cc)
        {
            string sql = string.Format("DELETE FROM ChamCong WHERE Macc = '{0}'", cc.Macc);
            cSDL_DAL.Chaycodesql(sql);
            return true;
        }

        public DataTable TimKiemNhanVien(string keyword, DateTime ngayChamCong)
        {
            string sql = string.Format("SELECT * FROM ChamCong WHERE MaNhanVien LIKE '%{0}%' AND Ngaycc = '{1}'", keyword, ngayChamCong.ToString("yyyy-MM-dd"));
            return cSDL_DAL.GetData(sql);
        }
    }
}
