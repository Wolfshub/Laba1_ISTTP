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
    public class GroupsController : Controller
    {
        private readonly ProjectDbContext _context;

        public GroupsController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var projectDbContext = _context.Groups.Include(g => g.Speciality).Include(g => g.Teacher);
            return View(await projectDbContext.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Speciality)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Details");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details,TeacherId,SpecialityId")] Groups groups, int id)
        {
            if (ModelState.IsValid)
            {
                var student = _context.Students.FirstOrDefault(s => s.Id == id);
                var group = new Groups()
                {
                    Details = groups.Details,
                    TeacherId = groups.TeacherId,
                    SpecialityId = groups.SpecialityId
                };
                group.Students.Add(student);
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", groups.SpecialityId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", groups.TeacherId);
            return View(groups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", groups.SpecialityId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", groups.TeacherId);
            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details,TeacherId,SpecialityId")] Groups groups)
        {
            if (id != groups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(groups.Id))
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
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", groups.SpecialityId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", groups.TeacherId);
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Speciality)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'ProjectDbContext.Groups'  is null.");
            }
            var groups = await _context.Groups.FindAsync(id);
            if (groups != null)
            {
                _context.Groups.Remove(groups);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
