using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;


namespace WebBao.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        public ActionResult KQTimKiem(string sTuKhoa)
        {
            // tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));

            return View(lstSP.OrderBy(n => n.TenSP));
        }
    }
}