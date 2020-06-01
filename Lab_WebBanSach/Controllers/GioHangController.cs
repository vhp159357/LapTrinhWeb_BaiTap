using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Lab_WebBanSach.Models;

namespace Lab_WebBanSach.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        dbQLBansachDataContext data =  new dbQLBansachDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lst = Session["Giohang"] as List<GioHang>;
            if (lst == null)
            {
                lst = new List<GioHang>();
                Session["Giohang"] = lst;
            }
            return lst;
        }
        //Them vao gio hang
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            //Kiem Tra Sach Ton Tai Trong Gio Hang
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sanpham == null)
            {
                sanpham=new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        //tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }

            return iTongSoLuong;
        }
        //tinh tong tien
        private double TongTien()
        {
            double iTongtien = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongtien = lstGioHang.Sum(n => n.dThanhTien);
            }

            return iTongtien;
        }
        //Xay Dung Trang Gio Hang
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(int iMaSp)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSp);

            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSp);
                return RedirectToAction("GioHang");
            }

            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("Giohang");
        }
        //Cap Nhat Gio Hang
        public ActionResult CapNhatGioHang(int iMaSp, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSp);
            if (sanpham != null)
            {
                sanpham.iSoLuong = Convert.ToInt32(f["txtSoLuong"].ToString());
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "BookStore");
        }

        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }

            List<GioHang> lst = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lst);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh= new DONDATHANG();
            KHACHHANG kh = (KHACHHANG) Session["TaiKhoan"];
            List<GioHang> lst = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngayGiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngayGiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in lst)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMaSach;
                ctdh.Soluong = item.iSoLuong;
                ctdh.Dongia = (decimal) item.dDonGia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhanDonHang","GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}