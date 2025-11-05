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
    public class ChatLieuxController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatLieuxController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatLieux
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChatLieus.ToListAsync());
        }

        // GET: ChatLieux/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus
                .FirstOrDefaultAsync(m => m.ChatLieuId == id);
            if (chatLieu == null)
            {
                return NotFound();
            }

            return View(chatLieu);
        }

        // GET: ChatLieux/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatLieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChatLieuId,TenChatLieu")] ChatLieu chatLieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatLieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatLieu);
        }

        // GET: ChatLieux/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus.FindAsync(id);
            if (chatLieu == null)
            {
                return NotFound();
            }
            return View(chatLieu);
        }

        // POST: ChatLieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChatLieuId,TenChatLieu")] ChatLieu chatLieu)
        {
            if (id != chatLieu.ChatLieuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatLieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatLieuExists(chatLieu.ChatLieuId))
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
            return View(chatLieu);
        }

        // GET: ChatLieux/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLieu = await _context.ChatLieus
                .FirstOrDefaultAsync(m => m.ChatLieuId == id);
            if (chatLieu == null)
            {
                return NotFound();
            }

            return View(chatLieu);
        }

        // POST: ChatLieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatLieu = await _context.ChatLieus.FindAsync(id);
            if (chatLieu != null)
            {
                _context.ChatLieus.Remove(chatLieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatLieuExists(int id)
        {
            return _context.ChatLieus.Any(e => e.ChatLieuId == id);
        }
    }
}
