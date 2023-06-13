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
    public class TimetablesController : Controller
    {
        private readonly ProjectDbContext _context;

        public TimetablesController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: Timetables
        public async Task<IActionResult> Index()
        {
            var projectDbContext = _context.Timetables.Include(t => t.Group).Include(t => t.Lesson);
            return View(await projectDbContext.ToListAsync());
        }

        // GET: Timetables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Timetables == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables
                .Include(t => t.Group)
                .Include(t => t.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetables == null)
            {
                return NotFound();
            }

            return View(timetables);
        }

        // GET: Timetables/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id");
            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details,GroupId,LessonId")] Timetables timetables)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timetables);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", timetables.GroupId);
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", timetables.LessonId);
            return View(timetables);
        }

        // GET: Timetables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Timetables == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables.FindAsync(id);
            if (timetables == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", timetables.GroupId);
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", timetables.LessonId);
            return View(timetables);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details,GroupId,LessonId")] Timetables timetables)
        {
            if (id != timetables.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetablesExists(timetables.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", timetables.GroupId);
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", timetables.LessonId);
            return View(timetables);
        }

        // GET: Timetables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Timetables == null)
            {
                return NotFound();
            }

            var timetables = await _context.Timetables
                .Include(t => t.Group)
                .Include(t => t.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetables == null)
            {
                return NotFound();
            }

            return View(timetables);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Timetables == null)
            {
                return Problem("Entity set 'ProjectDbContext.Timetables'  is null.");
            }
            var timetables = await _context.Timetables.FindAsync(id);
            if (timetables != null)
            {
                _context.Timetables.Remove(timetables);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimetablesExists(int id)
        {
          return (_context.Timetables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
