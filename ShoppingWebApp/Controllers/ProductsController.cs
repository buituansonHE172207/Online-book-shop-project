using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        IProductRepository productRepository = null;
        ICategoryRepository categoryRepository = null;
        public ProductsController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }
        [Authorize(Roles = "Admin")]
        // GET: ProductsController
        public ActionResult Index(string status, string categoryId)
        {
            var productList = productRepository.GetProductList();

            if (!string.IsNullOrEmpty(categoryId))
            {
                productList = productList.Where(p => p.CategoryId.ToString() == categoryId);
            }
            if (status == "true")
            {
                productList = productList.Where(p => p.Status == true);
            }
            else if (status == "false")
            {
                productList = productList.Where(p => p.Status == false);
            }

            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View(productList);
        }
        [Authorize(Roles = "Admin")]
        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductsController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = new Product
                    {
                        ProductName = productViewModel.ProductName,
                        CategoryId = productViewModel.CategoryId,
                        Price = productViewModel.Price,
                        Quantity = productViewModel.Quantity,
                        Status = productViewModel.Status,
                        Image = productViewModel.Image
                    };
                    productRepository.CreatePostProduct(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
                ViewBag.Message = ex.Message;
                return View(productViewModel);
            }
        }

        // GET: ProductsController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Product product = productRepository.GetProductById(id);
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", product.CategoryId);
            ProductViewModel productViewModel = new ProductViewModel
            {
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                ProductId = product.ProductId,
                Status = product.Status
            };
            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductViewModel productViewModel)
        {
            try
            {
                if (id != productViewModel.ProductId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    Product product = new Product
                    {
                        ProductName = productViewModel.ProductName,
                        CategoryId = productViewModel.CategoryId,
                        Price = productViewModel.Price,
                        Quantity = productViewModel.Quantity,
                        Status = productViewModel.Status,
                        Image = productViewModel.Image,
                        ProductId = productViewModel.ProductId
                    };

                    productRepository.UpdateProduct(product);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", productViewModel.CategoryId);
                return View(productViewModel);
            }
        }

    }
}
