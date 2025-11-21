using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WoodenFuniturestore.Data;
using WoodenFuniturestore.Models;

namespace WoodenFuniturestore.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SanPhams.Include(s => s.ChatLieu).Include(s => s.DanhMuc).Include(s => s.KhuyenMai);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.ChatLieu)
                .Include(s => s.DanhMuc)
                .Include(s => s.KhuyenMai)
                .FirstOrDefaultAsync(m => m.SanPhamId == id);
            if (sanPham == null)
            {
                return NotFound();
            }
            var query = await _context.SanPhams
                .Where(sp => sp.DanhMucId == sanPham.DanhMucId && sp.SanPhamId != id)
                .Select(sp => new SanPhamsViewModel
                {
                    Id = sp.SanPhamId,
                    TenSanPham = sp.TenSanPham,
                    HinhAnh = sp.HinhAnh,
                    GiaGoc = sp.Gia,
                    danhMuc = sp.DanhMucId,
                    GiaBan = (sp.KhuyenMai != null && sp.KhuyenMai.PhanTramGiam > 0)
                                ? (sp.Gia - (sp.Gia * sp.KhuyenMai.PhanTramGiam / 100))
                                : sp.Gia,
                    Rating = _context.DanhGia
                        .Where(dg => dg.SanPhamId == sp.SanPhamId && dg.IsDuyet == true)
                        .Average(dg => (double?)dg.Rating) ?? 0
                })
                .Take(4)
                .ToListAsync();
            var viewModel = new SanPhamDetailViewModel
            {
                SanPham = sanPham,
                SanPhamLienQuan = query
            };
            return View(viewModel);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["ChatLieuId"] = new SelectList(_context.ChatLieus, "ChatLieuId", "ChatLieuId");
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "DanhMucId", "DanhMucId");
            ViewData["KhuyenMaiId"] = new SelectList(_context.KhuyenMais, "KhuyenMaiId", "KhuyenMaiId");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SanPhamId,TenSanPham,MoTa,Gia,KichThuoc,SoLuongTon,HinhAnh,IsNoiBat,IsActive,NgayTao,DanhMucId,ChatLieuId,KhuyenMaiId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChatLieuId"] = new SelectList(_context.ChatLieus, "ChatLieuId", "ChatLieuId", sanPham.ChatLieuId);
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "DanhMucId", "DanhMucId", sanPham.DanhMucId);
            ViewData["KhuyenMaiId"] = new SelectList(_context.KhuyenMais, "KhuyenMaiId", "KhuyenMaiId", sanPham.KhuyenMaiId);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["ChatLieuId"] = new SelectList(_context.ChatLieus, "ChatLieuId", "ChatLieuId", sanPham.ChatLieuId);
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "DanhMucId", "DanhMucId", sanPham.DanhMucId);
            ViewData["KhuyenMaiId"] = new SelectList(_context.KhuyenMais, "KhuyenMaiId", "KhuyenMaiId", sanPham.KhuyenMaiId);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SanPhamId,TenSanPham,MoTa,Gia,KichThuoc,SoLuongTon,HinhAnh,IsNoiBat,IsActive,NgayTao,DanhMucId,ChatLieuId,KhuyenMaiId")] SanPham sanPham)
        {
            if (id != sanPham.SanPhamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.SanPhamId))
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
            ViewData["ChatLieuId"] = new SelectList(_context.ChatLieus, "ChatLieuId", "ChatLieuId", sanPham.ChatLieuId);
            ViewData["DanhMucId"] = new SelectList(_context.DanhMucs, "DanhMucId", "DanhMucId", sanPham.DanhMucId);
            ViewData["KhuyenMaiId"] = new SelectList(_context.KhuyenMais, "KhuyenMaiId", "KhuyenMaiId", sanPham.KhuyenMaiId);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.ChatLieu)
                .Include(s => s.DanhMuc)
                .Include(s => s.KhuyenMai)
                .FirstOrDefaultAsync(m => m.SanPhamId == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.SanPhamId == id);
        }
    }
}
