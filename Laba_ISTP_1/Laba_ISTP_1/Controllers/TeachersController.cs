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
    public class TeachersController : Controller
    {
        private readonly ProjectDbContext _context;

        public TeachersController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
              return _context.Teachers != null ? 
                          View(await _context.Teachers.ToListAsync()) :
                          Problem("Entity set 'ProjectDbContext.Teachers'  is null.");
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Gender")] Teachers teachers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teachers);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }
            return View(teachers);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Gender")] Teachers teachers)
        {
            if (id != teachers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachersExists(teachers.Id))
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
            return View(teachers);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'ProjectDbContext.Teachers'  is null.");
            }
            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers != null)
            {
                _context.Teachers.Remove(teachers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachersExists(int id)
        {
          return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
