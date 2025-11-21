using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WoodenFuniturestore.Data;
using WoodenFuniturestore.Models;

namespace WoodenFuniturestore.Controllers
{
    [Authorize]
    public class GioHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int userId);
            return userId;
        }
        public async Task<GioHang> GetOrCreatCartAsync(int userId)
        {
            var gioHang = await _context.GioHangs.FirstOrDefaultAsync(g => g.MaNguoiDung == userId);
            if(gioHang == null)
            {
                gioHang = new GioHang()
                {
                    MaNguoiDung = userId,
                    NgayTao = DateTime.Now
                };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
            }
            return gioHang;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong = 1)
        {
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized("Vui lòng đăng nhập để thêm vào giỏ hàng.");
            }
            try
            {
                var gioHang = await GetOrCreatCartAsync(userId);
                var sanPham = await _context.SanPhams.Include(sp => sp.KhuyenMai).FirstOrDefaultAsync(sp => sp.SanPhamId == sanPhamId);

                if (sanPham == null)
                {
                    return NotFound("Sản phẩm không tồn tại.");
                }
                if (sanPham.SoLuongTon < soLuong)
                {
                    TempData["CartError"] = "Số lượng tồn kho không đủ";
                    return RedirectToAction("Details", "SanPhamscustomer", new { id = sanPhamId });
                }
                var gia = sanPham.Gia;
                if (sanPham.KhuyenMaiId != null)
                {
                    gia = sanPham.Gia * (1-(sanPham.KhuyenMai.PhanTramGiam / 100));
                }
                var chitiet = await _context.ChiTietGioHangs
                    .FirstOrDefaultAsync(ct => ct.MaGioHang == gioHang.MaGioHang && ct.MaSanPham == sanPhamId);
                if (chitiet != null)
                {
                    chitiet.SoLuong += soLuong;
                }
                else
                {
                    chitiet = new ChiTietGioHang
                    {
                        MaGioHang = gioHang.MaGioHang,
                        MaSanPham = sanPhamId,
                        SoLuong = soLuong,
                        GiaBan = gia
                    };
                    _context.ChiTietGioHangs.Add(chitiet);
                }
                await _context.SaveChangesAsync();
                TempData["CartSuccess"] = "Đã thêm sản phẩm vào giỏ hàng";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                {
                    TempData["CartError"] = "Có lỗi xảy ra: " + ex.Message;
                    return RedirectToAction("Details", "SanPhamscustomer", new { id = sanPhamId });
                }
            }
        }
        [HttpGet]
        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var gioHang= await GetOrCreatCartAsync(userId);
            var itemInCart = await _context.ChiTietGioHangs
                .Where(ct => ct.MaGioHang == gioHang.MaGioHang)
                .Include(ct => ct.MaSanPhamNavigation).ToListAsync();
            return View(itemInCart);
           
        }

        // GET: GioHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaGioHang == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // GET: GioHangs/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "UserId", "UserId");
            return View();
        }

        // POST: GioHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGioHang,MaNguoiDung,NgayTao")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", gioHang.MaNguoiDung);
            return View(gioHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int maChiTiet, int soLuong)
        {
            int userId = GetCurrentUserId();
            var chiTiet = await _context.ChiTietGioHangs.FirstOrDefaultAsync(ct => ct.MaChiTiet == maChiTiet);
            if(chiTiet==null)
            {
                return NotFound("Không tìm thấy mục trong giỏ hàng");
            }
            var gioHang = await _context.GioHangs
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.MaNguoiDung == userId);
            if(gioHang ==null || chiTiet.MaGioHang != gioHang.MaGioHang)
            {
                return Unauthorized("Bạn không có quyền cập nhật mục này");
            }
            if(soLuong<=0)
            {
                _context.ChiTietGioHangs.Remove(chiTiet);
                TempData["CartSuccess"] = "Đã xóa sản phẩm khỏi giỏ hàng";
            }
            else
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSanPham);
                if(sanPham==null || sanPham.SoLuongTon< soLuong)
                {
                    TempData["CartError"] = $"Số lượng tồn kho của '{sanPham?.TenSanPham}' không đủ (chỉ còn {sanPham?.SoLuongTon}).";
                    return RedirectToAction("Index");
                }
                chiTiet.SoLuong = soLuong;
                _context.ChiTietGioHangs.Update(chiTiet);
                TempData["CartSuccess"] = "Đã cập nhật số lượng sản phẩm.";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int maChiTiet)
        {
            var userId = GetCurrentUserId();
            var chitietGioHang = await _context.ChiTietGioHangs.Include(ct=>ct.MaSanPhamNavigation).FirstOrDefaultAsync(ct=>ct.MaChiTiet==maChiTiet);
            if(chitietGioHang==null)
            {
                return NotFound("Không tìm thấy chi tiết giỏ hàng này");
            }
            var giohang = await _context.GioHangs.AsNoTracking().FirstOrDefaultAsync(gh => gh.MaNguoiDung == userId);
            if(giohang==null || giohang.MaGioHang != chitietGioHang.MaGioHang)
            {
                return Unauthorized("Bnaj không có quyền truy cập phần này.");
            }
            _context.ChiTietGioHangs.Remove(chitietGioHang);
            TempData["CartSuccess"] = $"Bạn đã xóa {chitietGioHang?.MaSanPhamNavigation.TenSanPham}";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        // GET: GioHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", gioHang.MaNguoiDung);
            return View(gioHang);
        }

        // POST: GioHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGioHang,MaNguoiDung,NgayTao")] GioHang gioHang)
        {
            if (id != gioHang.MaGioHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangExists(gioHang.MaGioHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", gioHang.MaNguoiDung);
            return View(gioHang);
        }

        // GET: GioHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaGioHang == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gioHang = await _context.GioHangs.FindAsync(id);
            if (gioHang != null)
            {
                _context.GioHangs.Remove(gioHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangExists(int id)
        {
            return _context.GioHangs.Any(e => e.MaGioHang == id);
        }
    }
}
