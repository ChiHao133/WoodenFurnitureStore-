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
    public class DanhGiumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DanhGiums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DanhGia.Include(d => d.SanPham).Include(d => d.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DanhGiums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGium = await _context.DanhGia
                .Include(d => d.SanPham)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DanhGiaId == id);
            if (danhGium == null)
            {
                return NotFound();
            }

            return View(danhGium);
        }

        // GET: DanhGiums/Create
        public IActionResult Create()
        {
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "SanPhamId", "SanPhamId");
            ViewData["UserId"] = new SelectList(_context.NguoiDungs, "UserId", "UserId");
            return View();
        }

        // POST: DanhGiums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DanhGiaId,SanPhamId,UserId,Rating,NoiDung,NgayDanhGia,IsDuyet")] DanhGium danhGium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhGium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "SanPhamId", "SanPhamId", danhGium.SanPhamId);
            ViewData["UserId"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", danhGium.UserId);
            return View(danhGium);
        }

        // GET: DanhGiums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGium = await _context.DanhGia.FindAsync(id);
            if (danhGium == null)
            {
                return NotFound();
            }
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "SanPhamId", "SanPhamId", danhGium.SanPhamId);
            ViewData["UserId"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", danhGium.UserId);
            return View(danhGium);
        }

        // POST: DanhGiums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DanhGiaId,SanPhamId,UserId,Rating,NoiDung,NgayDanhGia,IsDuyet")] DanhGium danhGium)
        {
            if (id != danhGium.DanhGiaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhGium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhGiumExists(danhGium.DanhGiaId))
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
            ViewData["SanPhamId"] = new SelectList(_context.SanPhams, "SanPhamId", "SanPhamId", danhGium.SanPhamId);
            ViewData["UserId"] = new SelectList(_context.NguoiDungs, "UserId", "UserId", danhGium.UserId);
            return View(danhGium);
        }

        // GET: DanhGiums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGium = await _context.DanhGia
                .Include(d => d.SanPham)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DanhGiaId == id);
            if (danhGium == null)
            {
                return NotFound();
            }

            return View(danhGium);
        }

        // POST: DanhGiums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhGium = await _context.DanhGia.FindAsync(id);
            if (danhGium != null)
            {
                _context.DanhGia.Remove(danhGium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhGiumExists(int id)
        {
            return _context.DanhGia.Any(e => e.DanhGiaId == id);
        }
    }
}
