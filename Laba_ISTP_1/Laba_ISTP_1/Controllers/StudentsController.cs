using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba_ISTP_1.Context;
using Laba_ISTP_1.Models;
using Microsoft.AspNetCore.Identity;
using static Laba_ISTP_1.Helpers.Roles;
using System.Security.Claims;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;

namespace Laba_ISTP_1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly UserManager<User> _userManager;

        public StudentsController(ProjectDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var projectDbContext = _context.Students.Include(s => s.Speciality).Include(s => s.User);
            return View(await projectDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Include(s => s.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,SpecialityId")] Students students)
        {
            if (ModelState.IsValid)
            {
                _context.Add(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", students.SpecialityId);
            return View(students);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", students.SpecialityId);
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,SpecialityId")] Students students)
        {
            if (id != students.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.Id))
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
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", students.SpecialityId);
            return View(students);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Include(s => s.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ProjectDbContext.Students'  is null.");
            }
            var students = await _context.Students.FindAsync(id);
            var user = await _context.Users.FindAsync(students.UserId);
            if (students != null)
            {
                _context.Students.Remove(students);
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BecomeStudent()
        {
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Details");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BecomeStudent(Students model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var student = new Students()
            {
                UserId = userId,
                SpecialityId = model.SpecialityId
            };
            await _userManager.RemoveFromRoleAsync(user, Role.User);
            await _userManager.AddToRoleAsync(user, Role.Student);
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var students = _context.Students.Include(s => s.User).Include(s=>s.Speciality).ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проєктах ТАК НЕ РОБИТИ (писати лише вибрані)
                var worksheet = workbook.Worksheets.Add("Всі студенти");
                worksheet.Cell("A1").Value = "Ім'я";
                worksheet.Cell("B1").Value = "Прізвище";
                worksheet.Cell("C1").Value = "Спеціальність";
                int row = 2;
                foreach (var item in students)
                {
                    worksheet.Cell(row, 1).Value = item.User.FirstName;
                    worksheet.Cell(row, 2).Value = item.User.LastName;
                    worksheet.Cell(row, 3).Value = item.Speciality.Details;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        private bool StudentsExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
