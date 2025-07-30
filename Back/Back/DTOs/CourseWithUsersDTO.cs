namespace Back.DTOs
{
    public class CourseWithUsersDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<UserDTO> Users { get; set; } = new();
    }
}
