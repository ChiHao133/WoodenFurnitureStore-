using Microsoft.AspNetCore.Mvc;
using WoodenFuniturestore.Data; // Thay bằng namespace DbContext
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace WoodenFuniturestore.ViewComponents
{
    public class DanhMucMenuViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DanhMucMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.DanhMucs.ToListAsync(); // Lấy danh mục
            return View(items); // Trả về view và model
        }
    }
}
