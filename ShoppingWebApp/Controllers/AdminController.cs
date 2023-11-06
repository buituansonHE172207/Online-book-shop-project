using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;
using System.Security.Claims;

namespace ShoppingWebApp.Controllers
{
    public class AdminController : Controller
    {
        IUserRepository userRepository;

        public AdminController()
        {
            userRepository = new UserRepository();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

                User newUser = new User()
                {
                    UserId = user.UserId.Value,
                    Email = user.Email,
                    FullName = user.FullName,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Avatar = user.Avatar,
                    Gender = user.Gender,
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

