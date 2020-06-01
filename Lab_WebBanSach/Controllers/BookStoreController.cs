using Lab_WebBanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Lab_WebBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: BookStore
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var sachMoi = LaySachMoi(18);
            return View(sachMoi.ToPagedList(pageNum,pageSize));
        }
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var nxb = from nx in data.NHAXUATBANs select nx;
            return PartialView(nxb);
        }
        public ActionResult SPTheochude(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach.ToPagedList(pageNum,pageSize));
        }
        public ActionResult SPTheoNXB(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}