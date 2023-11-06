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
    public class CategoryRepository : ICategoryRepository
    {
        public void CreateCategory(Category category) => CategoryDAO.Instance.Create(category);
        public void DeleteCategory(int id) => CategoryDAO.Instance.Remove(id);
        public Category GetCategoryById(int id) => CategoryDAO.Instance.GetCategoryById(id);
        public Category GetCategoryByName(string name) => CategoryDAO.Instance.GetCategoryByName(name);
        public IEnumerable<Category> GetCategoryList() => CategoryDAO.Instance.GetCategoryList();
        public void UpdateCategory(Category category) => CategoryDAO.Instance.Update(category);
    }
}

