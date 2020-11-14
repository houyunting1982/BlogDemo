namespace Blog.Data.Repositories
{
    public interface IUserRepository
    {
        bool isEmailUniq(string email);
        bool IsUsernameUniq(string username);
    }
}
