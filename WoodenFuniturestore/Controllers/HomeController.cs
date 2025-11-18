using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization; // C?n cho ??nh d?ng ti?n t?
using WoodenFuniturestore.Data;
using WoodenFuniturestore.Models;

namespace WoodenFuniturestore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // ??nh d?ng ti?n t? VN?
            var culture = new CultureInfo("vi-VN");

            // 1. L?y d? li?u thô t? CSDL, bao g?m c? thông tin KhuyenMai và DanhGia
            var sanPhamsTuDb = await _context.SanPhams
                                         .Include(sp => sp.KhuyenMai) // L?y thông tin khuy?n mãi liên quan
                                         .Include(sp => sp.DanhGia)   // L?y các ?ánh giá liên quan
                                         .Where(sp => sp.IsActive == true) // Ch? l?y s?n ph?m ?ang ho?t ??ng
                                         .OrderByDescending(sp => sp.SanPhamId)
                                         .Take(8)
                                         .ToListAsync();

            // 2. Chuy?n ??i (Map/Project) t? List<SanPham> sang List<SanPhamsViewModel>
            var viewModels = sanPhamsTuDb.Select(sp =>
            {
                // --- X? lý logic giá và khuy?n mãi ---
                decimal giaBan = sp.Gia;
                bool coKhuyenMai = false;

                // Ki?m tra xem s?n ph?m có khuy?n mãi không và khuy?n mãi có còn hi?u l?c không
                if (sp.KhuyenMai != null &&
                    sp.KhuyenMai.NgayBatDau <= DateTime.Now &&
                    sp.KhuyenMai.NgayKetThuc >= DateTime.Now)
                {
                    coKhuyenMai = true;
                    // Gi? s? KhuyenMai có thu?c tính PhanTramGiam (ví d?: 10 cho 10%)
                    // B?n c?n ?i?u ch?nh logic này cho phù h?p v?i model KhuyenMai c?a b?n
                    if (sp.KhuyenMai.PhanTramGiam > 0)
                    {
                        giaBan = sp.Gia * (1 - sp.KhuyenMai.PhanTramGiam / 100);
                    }
                    // Có th? có tr??ng h?p gi?m giá tr?c ti?p m?t s? ti?n
                }

                // --- X? lý logic ?ánh giá ---
                double rating = 0;
                if (sp.DanhGia != null && sp.DanhGia.Any())
                {
                    // Gi? s? model DanhGium có thu?c tính SoSao (t? 1 ??n 5)
                    rating =sp.DanhGia.Average(dg => dg.Rating) ?? 0;
                }

                // --- T?o ??i t??ng ViewModel ---
                return new SanPhamsViewModel
                {
                    Id = sp.SanPhamId,
                    TenSanPham = sp.TenSanPham,
                    HinhAnh = sp.HinhAnh,
                    GiaGoc = sp.Gia,
                    GiaBan = giaBan,
                    CoKhuyenMai = coKhuyenMai,
                    Rating = rating
                };
            }).ToList();

            // 3. Tr? v? View v?i danh sách ViewModel ?ã ???c x? lý
            return View(viewModels);
        }
    }
}