using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebApp.Controllers
{
    public class UsersController : Controller
    {
        IUserRepository userRepository = null;
        IRoleRepository roleRepository = null;

        public UsersController()
        {
            userRepository = new UserRepository();
            roleRepository = new RoleRepository();
        }


        // GET: UsersController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userList = userRepository.GetUserList();
            return View(userList);
        }

        public ActionResult Filter(string status)
        {
            IEnumerable<User> accountList;
            if (status == null)
            {
                return RedirectToAction(nameof(Index));

            }
            else if (status.Equals("true"))
            {
                accountList = userRepository.GetActiveUser();
            }
            else
            {
                accountList = userRepository.GetInactiveUser();
            }
            return View(accountList);

        }


        // GET: UsersController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var user = userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // GET: UsersController/Edit/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Edit(int id)
        //{
        //    User user = userRepository.GetUserById(id);
        //    ViewData["Role"] = new SelectList(roleRepository.GetRolesList(), "RoleId", "RoleName", user.Role);
        //    return View(user);
        //}

        // POST: UsersController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, User user)
        //{
        //    try
        //    {
        //        if (id != user.UserId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            userRepository.UpdateUser(user);
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //[HttpPost]
        //public ActionResult SetStatus([FromForm] int id, [FromForm] bool status)
        //{
        //    try
        //    {
        //        userRepository.SetAccountStatus(id, status);

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;

        //    }
        //    return RedirectToAction(nameof(Index));
        //}


    }
}

