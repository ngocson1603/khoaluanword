using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class HomePageViewModel
    {
        public List<Product> BestSellers { get; set; }
        public List<Product> FreeGames { get; set; }
        public List<ProductDetail> PopularGames { get; set; }
        public List<ProductDetail> RecentReleases { get; set; }
        public List<Category> FirstCategories { get; set; }
        public List<Category> SecondCategories { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}
