using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class SignupController : Controller
    {
        IUserRepository userRepository = new UserRepository();

        //private string defaultAvatar = "https://firebasestorage.googleapis.com/v0/b/clothshopping-6fd63.appspot.com/o/UserAvatar_Default.png?alt=media&token=3b553ee8-9965-474d-a6ef-5149e4c86273";


        [AllowAnonymous]
        public IActionResult Index(string? email, string? fullname)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            SignUpModel userEditModel = new SignUpModel();

            return View(userEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SignUpModel user)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            bool isError = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string password = user.Password;
                string confirm = user.Confirm;
                if (!password.Equals(confirm))
                {
                    isError = true;
                    message = "Confirm and Password are not matched!!";
                }
                else
                {
                    string email = user.Email;
                    string fullname = user.FullName;
                    bool status = true; 
                    int role = 2; 

                    try
                    {
                        User newUser = new User()
                        {
                            FullName = fullname,
                            Email = email,
                            Password = BCrypt.Net.BCrypt.HashPassword(password),
                            Role = role,
                            Status = status,
                            //Avatar = defaultAvatar
                        };

                        userRepository.SignUp(newUser);

                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        message = ex.Message;
                    }
                }
                
            }// else
            //{
            //    foreach (var key in ModelState.Keys)
            //    {
            //        var entry = ModelState[key];
            //        foreach (var error in entry.Errors)
            //        {
            //            Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
            //        }
            //    }
            //}
           

            if (isError)
            {
                ViewBag.Error = message;
                return View(user);
            }
            TempData["SignUp"] = "Sign up successfully! You can login now!";
            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        public IActionResult Google()
        {
            return RedirectToAction("Google", "Login");
        }
    }
}

