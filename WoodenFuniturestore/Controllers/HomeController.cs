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
         
            var culture = new CultureInfo("vi-VN");

           
            var sanPhamsTuDb = await _context.SanPhams
                                         .Include(sp => sp.KhuyenMai) 
                                         .Include(sp => sp.DanhGia)  
                                         .Where(sp => sp.IsActive == true) 
                                         .OrderByDescending(sp => sp.SanPhamId)
                                         .Take(8)
                                         .ToListAsync();

            
            var viewModels = sanPhamsTuDb.Select(sp =>
            {
             
                decimal giaBan = sp.Gia;
                bool coKhuyenMai = false;
                decimal phantramgiam = 0;
                
                if (sp.KhuyenMai != null &&
                    sp.KhuyenMai.NgayBatDau <= DateTime.Now &&
                    sp.KhuyenMai.NgayKetThuc >= DateTime.Now)
                {
                    coKhuyenMai = true;
          
                    if (sp.KhuyenMai.PhanTramGiam > 0)
                    {
                        giaBan = sp.Gia * (1 - sp.KhuyenMai.PhanTramGiam / 100);
                        phantramgiam = sp.KhuyenMai.PhanTramGiam;
                    }
                   
                }

          
                double rating = 0;
                if (sp.DanhGia != null && sp.DanhGia.Any())
                {
                   
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
                    Rating = rating,
                    PhanTramGiam=phantramgiam
                };
            }).ToList();
            return View(viewModels);
        }
    }
}