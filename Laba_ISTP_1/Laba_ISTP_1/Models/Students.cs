using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Students
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public int? SpecialityId { get; set; }
    [DisplayName("Спеціальність")]
    public Specialities? Speciality { get; set; }
    public List<Groups>? Groups { get; set; } = new List<Groups>();
}
