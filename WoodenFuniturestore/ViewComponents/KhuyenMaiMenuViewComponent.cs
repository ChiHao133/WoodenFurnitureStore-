using Microsoft.AspNetCore.Mvc;
using WoodenFuniturestore.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace WoodenFuniturestore.ViewComponents
{
    public class KhuyenMaiMenuViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public KhuyenMaiMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await _context.KhuyenMais.ToListAsync();
            return View(item);
        }
    }
}
