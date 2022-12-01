using Dapper;
using GameStore.Context;
using GameStore.Interfaces;
using GameStore.Interfaces.InterfaceRepositories;
using GameStore.Models;
using GameStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Repositories
{
    public class ProductRepository : GameStoreRepository<Product>, IProductRepository
    {
        public ProductRepository(GameStoreDbContext context) : base(context)
        {

        }

        /// <summary>
        /// Lấy Product Detail, bao gồm thông tin game đó và các Categories thuộc
        /// Dùng để trả về tất cả product detail của các game có trong database
        /// Hoặc dùng để trả về Product Detail của chỉ một game với id truyền vào
        /// </summary>
        /// <param name="id">id truyền vào, mặc định = 0 thì trả về tất cả product detail của các game có trong database</param>
        /// <returns></returns>
        public List<ProductDetail> GetProductWithCategories(int id = 0)
        {
            var query = @"select p.*, c.Name as Category, d.Name as Developer
                        from Product p, Category c, Developer d, ProductCategory pc
                        where p.DeveloperId = d.Id and p.Id = pc.ProductId and pc.CategoryId = c.Id";

            var data = Context.Database.GetDbConnection().Query<ProductDetailViewModel>(query);

            if (id != 0)
            {
                query += " and p.Id = @id";

                var para = new DynamicParameters();
                para.Add("id", id);

                data = Context.Database.GetDbConnection().Query<ProductDetailViewModel>(query, para);
            }

            var result = from p in data.ToList()
                         group p.Category by new
                         {
                             p.Id,
                             p.Name,
                             p.Overview,
                             p.Price,
                             p.Image,
                             p.Description,
                             p.ReleaseDate,
                             p.Developer
                         } into product_detail
                         select new ProductDetail
                         {
                             Id = product_detail.Key.Id,
                             Name = product_detail.Key.Name,
                             Overview = product_detail.Key.Overview,
                             Price = product_detail.Key.Price,
                             Image = product_detail.Key.Image,
                             Description = product_detail.Key.Description,
                             ReleaseDate = product_detail.Key.ReleaseDate,
                             Developer = product_detail.Key.Developer,
                             Categories = product_detail.ToList()
                         };

            return result.ToList();
        }

        public List<Product> GetProductByName(string name)
        {
            return this.GetAll().Where(t => t.Name.ToLower().Contains(name)).ToList();
        }
    }
}
