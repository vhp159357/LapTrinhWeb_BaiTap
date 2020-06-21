using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_WebBanSach.Models;
using PagedList;

namespace Lab_WebBanSach.Controllers
{
    public class AdminController : Controller
    {
        dbQLBansachDataContext db = new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sach(int? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNum, pageSize));
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection collection)
        {
            var tenDN = collection["userName"];
            var matKhau = collection["passWord"];
            if (tenDN != null && matKhau != null)
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tenDN && n.PassAdmin == matKhau);
                if (ad != null)
                {
                    Session["TaiKhoanAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng !");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["TaiKhoanAdmin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult ThemSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui Lòng Chọn Ảnh Bìa !";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sach.Anhbia = fileName;
                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }

                return RedirectToAction("Sach");
            }
        }

        public ActionResult ChiTietSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.MaSach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }

        [HttpGet]
        public ActionResult XoaSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.MaSach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult XacNhanXoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.MaSach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }

        public ActionResult SuaSach(int id)
        {
            SACH sach = db.SACHes.FirstOrDefault(n => n.Masach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(int id, SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            SACH sach123 = db.SACHes.Single(n => n.Masach == id);
            if (fileUpload != null)
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    }
                    else if (fileName.Length > 50)
                    {
                        ViewBag.ThongBao1 = "Tên File Quá Dài";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        sach.Anhbia = fileName;
                        sach123.Anhbia = sach.Anhbia;
                        sach123.Tensach = sach.Tensach;
                        sach123.Mota = sach.Mota;
                        sach123.Giaban = sach.Giaban;
                        sach123.Ngaycapnhat = sach.Ngaycapnhat;
                        sach123.Soluongton = sach.Soluongton;
                        sach123.MaCD = sach.MaCD;
                        sach123.MaNXB = sach.MaNXB;
                        db.SubmitChanges();
                    }
                }
                return RedirectToAction("Sach");
            }
            else
            {
                sach123.Tensach = sach.Tensach;
                sach123.Mota = sach.Mota;
                sach123.Giaban = sach.Giaban;
                sach123.Ngaycapnhat = sach.Ngaycapnhat;
                sach123.Soluongton = sach.Soluongton;
                sach123.MaCD = sach.MaCD;
                sach123.MaNXB = sach.MaNXB;
                db.SubmitChanges();
                return RedirectToAction("Sach");
            }
        }
    }
}