using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;
using System.Net;

namespace WebBao.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        // tạo 2 partialview 1 và 2 để hiển thị 2 loại sản phẩm theo phong cách khách nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {

            return PartialView();
        }

        // Xây dựng trang chi tiết
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            // kiểm tra tham số truyền vào có rỗng hay không
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP==id && n.DaXoa==0);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }

        //Xây dựng 1 action load sản phẩm theo mã loại sản phẩm và nhà sản xuất
        public  ActionResult SanPham(int? MaLoaiSp,int? MaNSx)
        {
            if (MaLoaiSp == null || MaNSx == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Load sản phẩm dựa trên 2 tiêu chí là MaxLoaiSp và mã NSX
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSp && n.MaNSX == MaNSx);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(lstSP);
        }

    }
}