using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_ISTP_1.Context;
using Laba_ISTP_1.Models;
using ClosedXML.Excel;

namespace Laba_ISTP_1.Controllers
{
    public class SpecialitiesController : Controller
    {
        private readonly ProjectDbContext _context;

        public SpecialitiesController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: Specialities
        public async Task<IActionResult> Index()
        {
            var projectDbContext = _context.Specialities.Include(s => s.Department);
            return View(await projectDbContext.ToListAsync());
        }

        // GET: Specialities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var specialities = await _context.Specialities
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialities == null)
            {
                return NotFound();
            }

            return View(specialities);
        }

        // GET: Specialities/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            return View();
        }

        // POST: Specialities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details,DepartmentId")] Specialities specialities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", specialities.DepartmentId);
            return View(specialities);
        }

        // GET: Specialities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var specialities = await _context.Specialities.FindAsync(id);
            if (specialities == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", specialities.DepartmentId);
            return View(specialities);
        }

        // POST: Specialities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details,DepartmentId")] Specialities specialities)
        {
            if (id != specialities.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialitiesExists(specialities.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", specialities.DepartmentId);
            return View(specialities);
        }

        // GET: Specialities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specialities == null)
            {
                return NotFound();
            }

            var specialities = await _context.Specialities
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialities == null)
            {
                return NotFound();
            }

            return View(specialities);
        }

        // POST: Specialities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specialities == null)
            {
                return Problem("Entity set 'ProjectDbContext.Specialities'  is null.");
            }
            var specialities = await _context.Specialities.FindAsync(id);
            if (specialities != null)
            {
                _context.Specialities.Remove(specialities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (fileExcel != null)
            {
                using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                {
                    await fileExcel.CopyToAsync(stream);
                    using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                    {
                        foreach (var worksheet in workBook.Worksheets)
                        {
                            foreach (var row in worksheet.RowsUsed())
                            {
                                try
                                {
                                    //var department = await _productRepository.GetCategoryByCellAsync(row.Cell(4).Value.ToString());
                                    var department = await _context.Departments.Where(x => x.Id == Convert.ToInt32(row.Cell(2).Value)).FirstOrDefaultAsync();
                                    if (department is null)
                                    {
                                        return RedirectToAction("Create");
                                    }
                                    var speciality = new Specialities();
                                    speciality.Details = row.Cell(1).Value.ToString();
                                    speciality.DepartmentId = department.Id;
                                    await _context.Specialities.AddAsync(speciality);
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        }
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        private bool SpecialitiesExists(int id)
        {
            return (_context.Specialities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
