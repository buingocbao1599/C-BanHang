using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;

namespace WebBao.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        
        public ActionResult SanPham1()
        {
            var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.ListSP = lstSanPhamLTM;

            var lstSanPhamDT = db.SanPhams.Where(n => n.MaLoaiSP == 1);
            ViewBag.ListDT = lstSanPhamDT;
            return View();
        }
        

        public ActionResult SanPham2()
        {
            return View();

        }
        /*
        public ActionResult SanPhamPartial()
        {
            //Lấy dữ liệu nạp vào model(dữ liệu là những sản phẩm laptop và mới)
            // var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP==2 && n.Moi==1);
            // return PartialView(lstSanPhamLTM);
            return PartialView();
        }
        */

        public ActionResult MenuPartial()
        {
            // truy vấn lấy về 1 list các sản phẩm
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        public ActionResult GioiThieu()
        {
            if(Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("SanPham1", "Home");
            }
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv, FormCollection f)
        {
            /* cách khi bên DangKy sai tên 
            string s = f["HoVaTen"].ToString(); */

            ViewBag.CauHoi = new SelectList(LoadCauHoi());

            //Thêm khách hàng vào csdl 
            db.ThanhViens.Add(tv);
            db.SaveChanges();
            ViewBag.ThongBao = "Thêm Thành Công";
            
            return View();
        }

        public ActionResult DangKy1()
        {
            return View();
        }

        // Load câu hỏi bí mật 
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích");
            lstCauHoi.Add("Nước hoa mà bạn yêu thích");
            lstCauHoi.Add("Kiểu người mà bạn yêu thích");
            lstCauHoi.Add("Người yêu cũ mà bạn yêu thích");
            lstCauHoi.Add("Công việc mà bạn yêu thích");
            lstCauHoi.Add("Xe ô tô mà bạn yêu thích");
            lstCauHoi.Add("Đồng hồ mà bạn yêu thích");
            return lstCauHoi;
        }

        //Xây dựng Action Đăng nhập 
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //Kiem tra ten dang nhap va mật khẩu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if(tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("SanPham1");
            }
            return Content("Tài Khoản hoặc mật khẩu không đúng.");
        }
        // Xây dựng Action Đăng xuất
       
        public ActionResult DangXuat() 
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("SanPham1");
        }

        public ActionResult XemGioHang()
        {
            return View();
        }
    }
}