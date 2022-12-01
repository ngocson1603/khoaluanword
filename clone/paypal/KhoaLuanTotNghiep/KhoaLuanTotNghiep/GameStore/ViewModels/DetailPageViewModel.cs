using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class DetailPageViewModel
    {
        public ProductDetail ProductDetail { get; set; }
        public List<ProductDetail> RelatedGames { get; set; }
        public List<ProductDetail> PopularGames { get; set; }
        public List<Category> Categories { get; set; }
    }
}
