using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_ISTP_1.Context;
using Laba_ISTP_1.Models;

namespace Laba_ISTP_1.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ProjectDbContext _context;

        public LessonsController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
              return _context.Lessons != null ? 
                          View(await _context.Lessons.ToListAsync()) :
                          Problem("Entity set 'ProjectDbContext.Lessons'  is null.");
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessons == null)
            {
                return NotFound();
            }

            return View(lessons);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details")] Lessons lessons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lessons);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons == null)
            {
                return NotFound();
            }
            return View(lessons);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details")] Lessons lessons)
        {
            if (id != lessons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonsExists(lessons.Id))
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
            return View(lessons);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessons == null)
            {
                return NotFound();
            }

            return View(lessons);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'ProjectDbContext.Lessons'  is null.");
            }
            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons != null)
            {
                _context.Lessons.Remove(lessons);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonsExists(int id)
        {
          return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
