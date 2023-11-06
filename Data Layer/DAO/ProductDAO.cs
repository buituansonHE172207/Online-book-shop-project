using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetProductList()
        {
            var products = new List<Product>();
            try
            {
                using var context = new ClothesShoppingContext();
                products = context.Products.Include(p => p.Category).OrderByDescending(p => p.ProductId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public Product GetProductByName(string name)
        {
            Product product;
            try
            {
                using var context = new ClothesShoppingContext();
                product = context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductName == name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public Product GetProductById(int Id)
        {
            Product product;
            try
            {
                using var context = new ClothesShoppingContext();
                product = context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void CreatePost(Product product)
        {
            try
            {
                Product productObj = GetProductByName(product.ProductName);
                if (productObj == null)
                {
                    using var context = new ClothesShoppingContext();
                    context.Products.Add(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product is already exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Product product)
        {
            try
            {
                List<Product> list = GetProductList().ToList();
                list.RemoveAll(p => p.ProductId == product.ProductId);
                using var context = new ClothesShoppingContext();
                Product producTest = list.FirstOrDefault(p => p.ProductName.Trim().ToUpper() == product.ProductName.Trim().ToUpper());

                if (producTest == null)
                {
                    context.Products.Update(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product is already exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int productId)
        {
            try
            {
                Product product = GetProductById(productId);
                if (product != null)
                {
                    using var context = new ClothesShoppingContext();
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
