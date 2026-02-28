using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class KhachHangBUS
    {
        KhachHangDAL khachHangDAL = new KhachHangDAL();
        public DataTable getData()
        {
            return khachHangDAL.getData();
        }
        public int kiemtramatrung(string ma)
        {
            return khachHangDAL.Kiemtramatrung(ma);
        }
        public string Themhk(KhachHangDTO Them)
        {
            //if (kiemtramatrung(Them.MaKhachHang) > 0)
            //{
            //    return "Mã khách hàng đã được đăng kí.";
            //}
            /*else*/
            if (khachHangDAL.ThemKhachHang(Them))
            {
                return "Thêm thành công.";
            }
            return "Thêm Thất Bại";
        }
        public string Suakh(KhachHangDTO Sua)
        {
            if (khachHangDAL.SuaKhachHang(Sua))
            {
                return "Sửa thông tin thành công.";
            }
            return "Sửa thông tin thất bại.";
        }

        public string XoanKH(KhachHangDTO Xoa)
        {
            if (khachHangDAL.XoaKhachHang(Xoa.MaKhachHang))
            {
                return "Xóa khách hàng thành công.";
            }
            return "Xóa khách hàng thất bại.";
        }
        public DataTable TimKiemKhachHang(string keyword)
        {
            return khachHangDAL.TimKiem(keyword);
        }
    }
}
