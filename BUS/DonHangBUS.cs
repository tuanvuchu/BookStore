using DAL;
using DTO;
using System.Data;

namespace BUS
{
    public class DonHangBUS
    {
        private readonly DonHangDAL donHangDAL = new DonHangDAL();
        private readonly KhachHangDAL khachHangDAL = new KhachHangDAL();

        public bool ThemDonHang(int t)
        {
            return donHangDAL.ThemDonHang(t);
        }
        public DataTable Hien()
        {
            return donHangDAL.Hien();
        }

        public DataTable HienKh()
        {
            return khachHangDAL.getData();
        }

        public bool ThemChiTiet(DonHangDTO dto)
        {
            return donHangDAL.ThemChiTiet(dto);
        }

        public bool Suasoluong(SanPhamDTO sp)
        {
            return donHangDAL.Suasoluong(sp);
        }

        public string ThemNgayDat(DonHangDTO dto)
        {
            donHangDAL.ThemNgayDonHang(dto);
            return "1";
        }

        public int LayMaDonHangTiepTheo()
        {
            return donHangDAL.LayMaDonHangTiepTheo();
        }
        public int LayMaChiTietTiepTheo()
        {
            return donHangDAL.LayMaChiTietTiepTheo();
        }
    }
}
