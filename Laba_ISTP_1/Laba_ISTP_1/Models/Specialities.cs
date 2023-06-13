using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Specialities
{
    public int Id { get; set; }
    [DisplayName("Назва")]
    public string Details { get; set; }
    public int? DepartmentId { get; set; }
    [DisplayName("Департамент")]
    public  Departments? Department { get; set; }
    public List<Students>? Students { get; set; }
    public List<Groups>? Groups { get; set; }
}
