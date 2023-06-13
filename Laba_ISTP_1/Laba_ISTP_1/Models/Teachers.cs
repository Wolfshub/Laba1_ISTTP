using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Teachers
{
    public int Id { get; set; }
    [DisplayName("Ім'я")]
    public string Name { get; set; }
    [DisplayName("Прізвище")]
    public string Surname { get; set; }
    [DisplayName("Стать")]
    public string Gender { get; set; }
}
