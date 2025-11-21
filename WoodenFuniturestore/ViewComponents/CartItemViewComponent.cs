using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime;
using System.Security.Claims;
using WoodenFuniturestore.Data;
using WoodenFuniturestore.Models;
namespace WoodenFuniturestore.ViewComponents
{
    public class CartItemViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CartItemViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if(User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = (ClaimsPrincipal)User;
                var userIdString = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier); if (int.TryParse(userIdString,out int userId))
                {
                    var gioHang = await _context.GioHangs
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(g => g.MaNguoiDung == userId);
                    if (gioHang != null) { 
                        int totalItem =await _context.ChiTietGioHangs.Where(ct=>ct.MaGioHang==gioHang.MaGioHang).CountAsync();
                        return View(totalItem);
                    }
                }
               
            }
            return View(0);
        }
    }
}
