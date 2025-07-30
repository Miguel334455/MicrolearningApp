using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Models
{
    public class UserCourse
    {
        public int UserCourseId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User{ get; set; } = null!;
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
