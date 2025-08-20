using Back.Models;

namespace Back.Services
{
    public class InMemoryUserService: IUserService
    {
        private readonly List<Models.User> _users = new();

        public User? GetByUsername(string Email)
        {
            return _users.FirstOrDefault(u => u.Email == Email);
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

    }
}
