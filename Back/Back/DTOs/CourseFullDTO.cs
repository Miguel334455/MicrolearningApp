namespace Back.DTOs
{
    public class CourseFullDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int IdCategory { get; set; } 
        public List<UserDTO> Users { get; set; } = new();
        public List<LessonDTO> Lessons { get; set; } = new();
    }
}
