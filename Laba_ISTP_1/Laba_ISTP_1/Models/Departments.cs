using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Departments
{
    public int Id { get; set; }
    [DisplayName("Назва")]
    public string Details { get; set; }
}
