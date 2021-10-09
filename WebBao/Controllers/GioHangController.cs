using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using WebBao.Models;

namespace WebBao.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                //Nếu giỏ sesion giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if( sp == null)
            {
                // trang đường dẫn k hợp lệ 
                Response.StatusCode = 404;
                return null;
            }
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp thứ nhất sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck != null)
            {
                // kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
               if(sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        // tính tổng số lượng
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n=>n.SoLuong);
        }

        // Tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

     
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult XemGioHang()
        {
            // lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        //chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang(int MaSP)
        {
            // Kiểm tra sesesion Giỏ hàng đã tồn tại chưa   
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham1","Home");
            }
            // kiểm tra sản phẩm có tồn tại trong csdl k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn k hợp lệ 
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // kiểm tra sản phẩm đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("SanPham1", " Home");
            }
            // lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            
            //Nếu tồn tại rồi
            return View(spCheck);
        }

        // Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if(spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            // Cập nhật số lượng session của giỏ hàng

            //Bước 1:lấy List<GioHang> từ Session["GioHang"]
            List<ItemGioHang> lstGH = LayGioHang();

            //Bước 2: lấy sản phẩm cân từ list<GioHang>
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            
            //Bước 3: Tiến hành cập nhật
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGH.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        } 

        // Xóa giỏ hàng
        public ActionResult XoaGioHang(int MaSP)
        {
            // kiểm tra xem giỏ hàng có tồn tại hay chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham1", "Home");
            }
            // Kiểm tra sản phẩm có tồn tại trong csdl hay k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                // Trang đường dẫn không hợp lê
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();

            //Kiểm tra xem sản phẩm đã có trong giỏ hàng hay chưa
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("SanPham1", "Home");
            }
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang", "GioHang");
        }

        // Xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            // kiểm tra xem Session giỏ hàng đẫ tồn tại chưa
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham1", " Home");
            }

            KhachHang khachhangT = new KhachHang();
            if(Session["TaiKhoan"]== null)
            {
                //Thêm khách hàng chưa đăng nhập vào bảng khách hàng
                khachhangT = kh;
                db.KhachHangs.Add(khachhangT);
                db.SaveChanges();
            }
            else
            {
                // Xử lý đối với khách hàng là thành viên 
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachhangT.TenKH = tv.HoTen;
                khachhangT.DiaChi = tv.DiaChi;
                khachhangT.Email = tv.Email;
                khachhangT.SoDienThoai = tv.SoDienThoai;
                khachhangT.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khachhangT);
                db.SaveChanges();
            }

            // Thêm đơn đặt hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khachhangT.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            // Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);

            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang", "GioHang");
        }

        // Thểm giỏ hàng bằng Ajax cho trang sản phẩm chi tiết
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn k hợp lệ 
                Response.StatusCode = 404;
                return null;
            }
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp thứ nhất sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng\") </script>");
                    
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng\") </script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
    }
}