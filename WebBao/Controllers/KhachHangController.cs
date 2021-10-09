using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;


namespace WebBao.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            // Truy vấn dữ liệu thông qua câu lệnh
            //Đối lstKH sẽ lấy toàn bộ dữ liệu của khách hàng
            // Cách 1: lấy dữ liệu từ 1 danh sách khách hàng
            // var lstKH = from KH in db.KhachHangs select KH;
            //Cách 2: dùng phương thức hỗ trợ sẵn
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }

        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHangs select KH;

            return View(lstKH);
        }

        public ActionResult TruyVan1DoiTuong()
        {
            //Cách 1:
            //Bước 1: lấy ra danh sách khách hàng
            // var lstKH = from kh in db.KhachHangs where kh.MaKH==1 select kh;
            //Bước 2: Lấy ra đối tượng khách hàng
            // KhachHang khang = lstKH.FirstOrDefault();

            // Cách 2:lấy đối tượng dựa trên phương thức hỗ trợ 
            KhachHang khang = db.KhachHangs.Single(n => n.MaKH == 1);
            return View(khang);
        }

        public ActionResult SortDuLieu()
        {
            //phương thức sắp xếp dữ liệu
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }

        public ActionResult GroupDuLieu()
        {
            // có 2 cách là Group tại View và tại Controller
            // Cách 1: Group tại View
            // Bước 1: trả về 1 danh sách khách hàng
            List<ThanhVien> lstKH = db.ThanhViens.OrderBy(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }


        public ActionResult Login()
        {
            return View();
        }
    }
}