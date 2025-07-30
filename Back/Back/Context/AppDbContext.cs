using Microsoft.EntityFrameworkCore;
using Back.Models;

namespace Back.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Course> Courses { get; set; } = null!;
        public DbSet<Models.User> Users { get; set; } = null!;
        public DbSet<Models.UserCourse> UserCourses { get; set; } = null!;
        public DbSet<Back.Models.Lesson> Lesson { get; set; } = default!;
    }
}
