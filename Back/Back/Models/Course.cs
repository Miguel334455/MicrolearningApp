namespace Back.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int IdCategory { get; set; }
        public List<UserCourse>? UserCourses { get; set; } = null!;
        public List<Lesson>? Lessons { get; set; } = null!;

    }
}
