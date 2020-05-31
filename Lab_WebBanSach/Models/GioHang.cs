using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_WebBanSach.Models
{
    public class GioHang
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public Double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        //khoi tao gio hang theo masach duoc truyen vao voi so luong bang 1
        public GioHang(int maSach)
        {
            iMaSach = maSach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMaSach);
            sTenSach = sach.Tensach;
            sAnhBia = sach.Anhbia;
            dDonGia = double.Parse(sach.Giaban.ToString());
            iSoLuong = 1;
        }
    }
}