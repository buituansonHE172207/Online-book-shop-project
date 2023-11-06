using BusinessObject;

namespace Data_Layer.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }
        //-----------------------------------------------------------------------------

        public IEnumerable<Category> GetCategoryList()
        {
            var categoryList = new List<Category>();
            try
            {
                using var context = new ClothesShoppingContext();
                categoryList = context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categoryList;

        }

        public Category GetCategoryById(int id)
        {
            Category category;
            try
            {
                using var context = new ClothesShoppingContext();
                category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public Category GetCategoryByName(string name)
        {
            Category category;
            try
            {
                using var context = new ClothesShoppingContext();
                category = context.Categories.FirstOrDefault(c => c.CategoryName == name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public void Create(Category category)
        {
            try
            {
                Category categoryObj = GetCategoryByName(category.CategoryName);
                if (categoryObj == null)
                {
                    using var context = new ClothesShoppingContext();
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The category is already exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Category category)
        {
            try
            {
                Category categoryObj = GetCategoryByName(category.CategoryName);
                using var context = new ClothesShoppingContext();
                if (categoryObj == null)
                {
                    context.Categories.Update(category);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The category is already exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int categoryId)
        {
            try
            {
                Category category = GetCategoryById(categoryId);
                if (category != null)
                {
                    using var context = new ClothesShoppingContext();
                    context.Categories.Remove(category);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The category not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
