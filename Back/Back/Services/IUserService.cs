using Back.Models;

namespace Back.Services
{
    public interface IUserService
    {
        User? GetByUsername(string Email);
        void Add(User user);
    }
}
