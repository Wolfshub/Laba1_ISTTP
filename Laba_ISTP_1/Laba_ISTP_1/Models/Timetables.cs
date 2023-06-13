using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Laba_ISTP_1.Models;

public class Timetables
{
    public int Id { get; set; }
    [DisplayName("Назва")]
    public string Details { get; set; }

    [ForeignKey("Groups")]
    public int? GroupId { get; set; }
    [DisplayName("Група")]
    public Groups? Group { get; set; }

    [ForeignKey("Lessons")]
    public int? LessonId { get; set; }
    [DisplayName("Пара")]
    public Lessons? Lesson { get; set; }
}
