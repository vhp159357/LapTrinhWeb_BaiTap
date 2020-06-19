using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_WebBanSach.Models;

namespace Lab_WebBanSach.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        dbQLBansachDataContext db = new dbQLBansachDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection,KHACHHANG kh)
        {
            var hoten = collection["HoTenKH"];
            var tenDN = collection["TenDN"];
            var matKhau = collection["MatKhau"];
            var matKhauNhapLai = collection["MatKhauNhapLai"];
            var diaChi = collection["DiaChi"];
            var email = collection["Email"];
            var dienThoai = collection["DienThoai"];
            var ngaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Vui Lòng Nhập Họ Tên Khách Hàng";
            }else if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi2"] = "Vui Lòng Nhập Tên Đăng Nhập";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi3"] = "Vui Lòng Nhập Mật Khẩu";
            }
            else if (String.IsNullOrEmpty(matKhauNhapLai))
            {
                ViewData["Loi4"] = "Vui Lòng Nhập Lại Mật Khẩu";
            }
            else if (String.IsNullOrEmpty(dienThoai))
            {
                ViewData["Loi5"] = "Vui Lòng Nhập SĐT";
            }
            else if (String.IsNullOrEmpty(diaChi))
            {
                ViewData["Loi6"] = "Vui Lòng Nhập Địa Chỉ";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = "Vui Lòng Nhập Email";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tenDN;
                kh.Matkhau = matKhau;
                kh.Email = email;
                kh.DiachiKH = diaChi;
                kh.DienthoaiKH = dienThoai;
                kh.Ngaysinh = Convert.ToDateTime(ngaySinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy();
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tenDN = collection["TenDN"];
            var matKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi1"] = "Bạn Phải Nhập Tên Đăng Nhập ";
            }
            else if(String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi2"] = "Bạn Phải Nhập Mật Khẩu ";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tenDN && n.Matkhau == matKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng Nhập Thành Công ";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                {
                    ViewBag.ThongBao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "BookStore");
        }
    }
}