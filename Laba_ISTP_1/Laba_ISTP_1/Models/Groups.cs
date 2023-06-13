using Microsoft.Build.Framework;
using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Groups
{
    public int Id { get; set; }
    [DisplayName("Назва")]
    public string Details { get; set; }
    public int? TeacherId { get; set; }
    [DisplayName("Викладач")]
    public Teachers? Teacher { get; set; }
    public int? SpecialityId { get; set; }
    [DisplayName("Спеціальність")]
    public Specialities? Speciality { get; set; }
    public List<Students>? Students { get; set; } = new List<Students>();
}
