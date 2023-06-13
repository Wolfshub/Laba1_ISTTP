using Laba_ISTP_1.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laba_ISTP_1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChartController : ControllerBase
{
    private readonly ProjectDbContext _context;

    public ChartController(ProjectDbContext context)
	{
        _context = context;
    }

    [HttpGet("JsonData")]
    public JsonResult JsonData()
    {
        var specialities = _context.Specialities.Include(s => s.Students).ToList();
        var result = new List<object>();
        result.Add(new[] { "Спеціальність", "Кількість студентів" });

        foreach (var speciality in specialities)
        {
            int studentCount = speciality.Students.Count;
            result.Add(new object[] { speciality.Details, studentCount });
        }

        return new JsonResult(result);
    }

}
