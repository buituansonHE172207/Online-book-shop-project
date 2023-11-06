using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Claims;
using System.Security.Policy;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Crypto.Macs;

namespace ShoppingWebApp.Controllers
{
    public class LoginController : Controller
    {
        IUserRepository userRepository = null;
        public LoginController()
        {
            userRepository = new UserRepository();
        }

        // GET: Login
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm] string email,
            [FromForm] string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                ViewBag.Email = email;
                User loginUser = userRepository.Login(email, password);

                if (loginUser == null)
                {
                    ViewBag.Message = "Login failed! Please check your email and password";
                    return View();
                }
                else
                {
                    var userClaims = new List<Claim>()
                    {
                        new Claim("UserId", loginUser.UserId.ToString()),
                        new Claim(ClaimTypes.Email, loginUser.Email),
                        new Claim(ClaimTypes.Role, loginUser.RoleNavigation.RoleName)
                    };

                    var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    HttpContext.SignInAsync(userPrincipal);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult UserAccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Google()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };
            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {

            try
            {
                var request = HttpContext.Request;
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                {
                    throw new Exception("External authentication error");
                }

                var externalUser = result.Principal;
                if (externalUser == null)
                {
                    throw new Exception("External authentication error");
                }

                var email = externalUser.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
                //IUserRepository _userRepository = new UserRepository();
                User user = userRepository.GetUser(email);

                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    //TempData["Email"] = email;
                    string fullname = externalUser.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name)).Value;
                    return RedirectToAction("Index", "Signup", new { email, fullname });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            string email = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                // User is authenticated
                email = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
            }

            ViewBag.Email = email;
            return View();
        }

        private string GeneratePassword(int minLength, int maxLength)
        {
            string password = string.Empty;

            var chars = "Z!X@C#V$B%N^M&L*K(J)H.G/FDSAQWERTYUIOPzxcvbnmlkjhgfdsaqwertyuiop0123456789";
            var random = new Random();
            try
            {
                int length = random.Next(minLength, maxLength);
                var passwordChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    passwordChars[i] = chars[random.Next(chars.Length)];
                }

                password = new string(passwordChars);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return password;
        }

        // Build Email Message
        private MimeMessage GenerateMessage(string fullname, string email, string password)
        {
            MimeMessage message = null;
            try
            {
                message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Clothes Shopping", "noreply@clothesshopping.com");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(fullname, email);
                message.To.Add(to);

                message.Subject = "Reset your Password - Clothes Shopping";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<h2>Reset your Password</h2>" +
                    $"<p>Dear {fullname},</p>" +
                    $"<p>We had recently received a request from our website to reset your password. " +
                    $"<strong>If you don't actually do this, please ignore this email and check your account at our website!</strong></p>" +
                    $"<h3>Your new Login Information</h3>" +
                    $"<p>Email: {email}</p>" +
                    $"<p>New Password: <strong><em>{password}</em></strong></p>" +
                    $"<p>Thank you for using our services!</p>" +
                    $"<p></p>" +
                    $"<p><em>This is automatic email, please do not reply to this email!!!</em></p>";

                message.Body = bodyBuilder.ToMessageBody();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return message;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (email != null && !string.IsNullOrEmpty(email))
                {
                    // Get User
                    //IUserRepository userRepository = new UserRepository();
                    User user = userRepository.GetUser(email);
                    if (user != null)
                    {
                        string fullname = user.FullName;
                        string newPassword = GeneratePassword(8, 15);

                        // Get Message
                        MimeMessage message = GenerateMessage(fullname, email, newPassword);

                        // Mail Server
                        SmtpClient client = new SmtpClient();
                        client.Connect("smtp.gmail.com", 587, true);
                        client.Authenticate("btson1407@gmail.com", "xczd yhgg zeup ijpe");

                        // Send
                        client.Send(message);
                        client.Disconnect(true);
                        client.Dispose();

                        // Set new Password
                        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Hashing Password
                        userRepository.Update(user);

                        HttpContext.SignOutAsync();
                    }
                    ViewBag.Success = "An email with your new Password is sent to <strong>" + email + "</strong>." +
                        " If the email is existed in our system, you will receive the new password soon!";

                }
                else
                {
                    throw new Exception("Email cannot be empty!");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

    }
}

