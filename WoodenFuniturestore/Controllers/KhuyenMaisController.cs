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
    public class KhuyenMaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhuyenMaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KhuyenMais
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhuyenMais.ToListAsync());
        }

        // GET: KhuyenMais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(m => m.KhuyenMaiId == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KhuyenMaiId,TenSuKien,MoTa,PhanTramGiam,NgayBatDau,NgayKetThuc")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KhuyenMaiId,TenSuKien,MoTa,PhanTramGiam,NgayBatDau,NgayKetThuc")] KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.KhuyenMaiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khuyenMai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuyenMaiExists(khuyenMai.KhuyenMaiId))
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
            return View(khuyenMai);
        }

        // GET: KhuyenMais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(m => m.KhuyenMaiId == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // POST: KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                _context.KhuyenMais.Remove(khuyenMai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiExists(int id)
        {
            return _context.KhuyenMais.Any(e => e.KhuyenMaiId == id);
        }
    }
}
