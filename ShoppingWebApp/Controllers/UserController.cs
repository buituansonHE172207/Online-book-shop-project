using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;
using System.Security.Claims;

namespace ShoppingWebApp.Controllers
{
    public class UserController : Controller
    {

        IUserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                string email = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
                User user = userRepository.GetUser(email);

                UserEditModel userEditModel = new UserEditModel()
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    Password = user.Password
                };
                return View(userEditModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserEditModel user)
        {
            try
            {
                string email = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
                User oldUser = userRepository.GetUser(email);

                if (!email.Equals(user.Email))
                {
                    throw new Exception("Email is invalid!! Please check again your information");
                }
                if (userRepository.Login(email, user.Password) == null)
                {
                    throw new Exception("Your password is not matched!! Please try again...");
                }
                if (!string.IsNullOrEmpty(user.NewPassword))
                {
                    if (!user.NewPassword.Equals(user.Confirm))
                    {
                        throw new Exception("Confirm and New Password are not matched!! Please check again");
                    }
                    user.Password = user.NewPassword;
                }

                if (string.IsNullOrEmpty(user.Avatar))
                {
                    user.Avatar = oldUser.Avatar;
                }

                // Update
                User newUser = new User()
                {
                    UserId = user.UserId.Value,
                    Email = user.Email,
                    FullName = user.FullName,
                    Address = user.Address,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Avatar = user.Avatar,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Role = oldUser.Role
                };
                userRepository.Update(newUser);
                ViewBag.Success = "Update your information successfully!!";
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(user);
            }
        }
    }
}
