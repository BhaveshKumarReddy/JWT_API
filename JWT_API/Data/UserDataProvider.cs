using JWT_API.Models;

namespace JWT_API.Data
{
    public class UserDataProvider
    {
        private readonly List<User> users = new List<User>()
        {
            new User{ Username="admin",Password="admin"},
            new User{ Username="888881",Password="test@123"},
            new User{ Username="888882",Password="test@123"},
            new User{ Username="888883",Password="test@123"},
            new User{ Username="888884",Password="test@123"},
            new User{ Username="888885",Password="test@123"},
            new User{ Username="888886",Password="test@123"}
        };

        public User GetUserDetail(User login)
        {
            return users.SingleOrDefault(x => x.Username == login.Username && x.Password == login.Password);
        }
    }
}
