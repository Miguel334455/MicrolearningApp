using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int courseId { get; set; }
        public string? content { get; set; }

    }
}
