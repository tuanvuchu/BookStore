using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class NhaCungCapBUS
    {
        NhaCungCapDAL nhaCungCapDAL = new NhaCungCapDAL();
        public DataTable getData()
        {
            return nhaCungCapDAL.getData();
        }
        public int kiemtramatrung(string ma)
        {
            return nhaCungCapDAL.kiemtramatrung(ma);
        }
        public string Themnhacc(NhaCungCapDTO Them)
        {
            //if (kiemtramatrung(Them.MaNhaCungCap) > 0)
            //{
            //    return "Mã nhà cung cấp đã được đăng kí.";
            //}
            /*else*/
            if (nhaCungCapDAL.Themnhacc(Them))
            {
                return "Thêm nhà cung cấp thành công.";
            }
            return "Thêm Nhà Cung Cấp Thất Bại";
        }

        public string Suanhacc(NhaCungCapDTO nccDTO)
        {
            if (nhaCungCapDAL.Suanhacc(nccDTO))
            {
                return "Sửa thông tin nhà cung cấp thành công.";
            }
            else
            {
                return "Sửa thông tin nhà cung cấp thất bại.";
            }
        }

        public string Xoanhacc(NhaCungCapDTO nccDTO)
        {
            if (nhaCungCapDAL.Xoanhacc(nccDTO.MaNhaCungCap))
            {
                return "Xóa nhà cung cấp thành công.";
            }
            else
            {
                return "Xóa nhà cung cấp thất bại.";
            }
        }
        public DataTable TimKiemNhanVien(string keyword)
        {
            return nhaCungCapDAL.TimKiem(keyword);
        }
    }
}
