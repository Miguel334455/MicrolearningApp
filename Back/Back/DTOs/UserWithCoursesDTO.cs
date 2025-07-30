namespace Back.DTOs
{
    public class UserWithCoursesDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public List<CourseDTO> Courses { get; set; } = new();
    }
}
