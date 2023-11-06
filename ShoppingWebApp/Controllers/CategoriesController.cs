using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        ICategoryRepository categoryRepository = null;

        public CategoriesController()
        {
            categoryRepository = new CategoryRepository();
        }

        // GET: Categories
        public ActionResult Index()
        {

            return View(categoryRepository.GetCategoryList());
        }

        // GET: Categories/Details/5
        //public ActionResult Details(int id)
        //{

        //    var category = categoryRepository.GetCategoryById(id);

        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = new Category
                    {
                        CategoryName = categoryViewModel.CategoryName
                    };
                    categoryRepository.CreateCategory(category);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(categoryViewModel);
            }
        }

        // GET: Categories/Edit/5
        //public ActionResult Edit(int id)
        //{

        //    var category = categoryRepository.GetCategoryById(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, Category category)
        //{
        //    try
        //    {
        //        if (id != category.CategoryId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            categoryRepository.UpdateCategory(category);                   
        //        }
        //        return RedirectToAction(nameof(Index));

        //    } catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        return View();
        //    }  
        //}

        // GET: Categories/Delete/5
        //public ActionResult Delete(int id)
        //{

        //    var category = categoryRepository.GetCategoryById(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        categoryRepository.DeleteCategory(id);
        //        return RedirectToAction(nameof(Index));
        //    } catch(Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        return View();
        //    }

        //}

    }
}
