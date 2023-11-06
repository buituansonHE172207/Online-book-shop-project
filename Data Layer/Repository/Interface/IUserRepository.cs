using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Repository.Interface
{
    public interface IUserRepository
    {
        public User Login(string email, string password);
        public User GetUser(string email);
        IEnumerable<User> GetUserList();
        IEnumerable<User> GetActiveUser();
        IEnumerable<User> GetInactiveUser();
        User GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        void SetAccountStatus(int id, bool status);
        public void SignUp(User user);
        public void Update(User user);

    }
}
