using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;

namespace WebBao.Controllers
{
    public class DemoAjaxController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }

        public ActionResult LoadAjaxActionLink()
        {
            System.Threading.Thread.Sleep(3000);
            return Content("Hello Ajax");
        }

        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            System.Threading.Thread.Sleep(3000);
            string KQ = f["txt1"].ToString();
            return Content(KQ);
        }

        //Xu ly Ajax Jquery
        public ActionResult LoadAjaxJquery(int a, int b)
        {
            System.Threading.Thread.Sleep(3000);
            return Content((a + b).ToString());
        }

        // Load Ajax tra ve 1 partila view
        
        public ActionResult LoadSanPhamPartial()
        {
            var lstSanPham = db.SanPhams;

            return PartialView("LoadSanPhamPartial", lstSanPham);
        }
    }
}