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
        dbQLBansachDataContext db =new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sach(int? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(db.SACHes.ToList().OrderBy(n=>n.Masach).ToPagedList(pageNum,pageSize));
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
                    ModelState.AddModelError("","Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng !");
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
        public ActionResult ThemSach(SACH sach,HttpPostedFileBase fileUpload)
        {
            var fileName = Path.GetFileName(fileUpload.FileName);

            return View();
        }
    }
}