using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class User : IdentityUser
{
   
    [DisplayName("Ім'я")]
    public string FirstName { get; set; }
    [DisplayName("Прізвище")]
    public string LastName { get; set; }
}
