namespace Manisero.YouShallNotPass.Samples.Custom_validation_data
{
    // User & UpdateUserCommand

    public class User
    {
        public int UserId { get; set; }
    }

    public class UpdateUserCommand
    {
        public int UserId { get; set; }
        // ...
    }

    // UserRepository

    public interface IUserRepository
    {
        User Get(int userId);
    }

    public class UserRepository : IUserRepository
    {
        public User Get(int userId) => new User { UserId = userId };
    }
}
