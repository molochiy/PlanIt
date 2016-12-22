using System.Collections.Generic;
using System.Linq;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;

namespace PlanIt.Services.Concrete
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepository repository) : base(repository)
        {
        }

        public User GetUserById(int id)
        {
            var user = _repository.GetSingle<User>(u => u.Id == id);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var userWithEmail = _repository.GetSingle<User>(u => u.Email == email);
            return userWithEmail;
        }

        public int GetUserIdByEmail(string email)
        {
            var user = GetUserByEmail(email);
            return user.Id;
        }

        public List<string> GetEmailsForSharing(string emailSubstring, string currentUserEmail)
        {
            var usersWithNecessaryEmail = _repository.Get<User>(u => u.Email.Contains(emailSubstring));
            var emailsForSharing = usersWithNecessaryEmail.Select(u => u.Email).Where(email => email != currentUserEmail).ToList();

            return emailsForSharing;
        }

        public User AddUser(User user)
        {
            var insertedUser = _repository.Insert(user);
            return insertedUser;
        }

        public bool UserWithSpecificEmailExists(string email)
        {
            var userExists = _repository.Exists<User>(u => u.Email == email);
            return userExists;
        }
    }
}
