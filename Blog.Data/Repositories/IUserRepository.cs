using Blog.Model.Entities;

namespace Blog.Data.Repositories
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        bool isEmailUniq(string email);
        bool IsUsernameUniq(string username);
    }
}
