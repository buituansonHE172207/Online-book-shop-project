using BusinessObject;
using Data_Layer.DAO;
using Data_Layer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        public void DeleteUser(int userId) => UserDAO.Instance.Remove(userId);
        public User GetUser(string email) => UserDAO.Instance.GetUser(email);
        public User GetUserById(int id) => UserDAO.Instance.GetUserById(id);
        public IEnumerable<User> GetUserList() => UserDAO.Instance.GetUserList();
        public void SetAccountStatus(int id, bool status) => UserDAO.Instance.SetAccountStatus(id, status);
        public void UpdateUser(User user) => UserDAO.Instance.Update(user);
        public User Login(string email, string password) => UserDAO.Instance.Login(email, password);
        public void SignUp(User user) => UserDAO.Instance.SignUp(user);
        public void Update(User user) => UserDAO.Instance.Update(user);
        public IEnumerable<User> GetActiveUser() => UserDAO.Instance.GetActiveAccount();
        public IEnumerable<User> GetInactiveUser() => UserDAO.Instance.GetInactiveAccount();

    }
}
