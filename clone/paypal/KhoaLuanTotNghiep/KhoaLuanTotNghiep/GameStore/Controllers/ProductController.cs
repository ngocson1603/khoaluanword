using AspNetCoreHero.ToastNotification.Abstractions;
using GameStore.Interfaces;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }

        public ProductController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
        }

        [HttpGet]
        [Route("product-homepage.html", Name = "HomePage")]
        public IActionResult HomePage()
        {
            var bestSellers = _unitOfWork.ProductRepository.GetAll().Take(10).ToList();
            var freeGames = _unitOfWork.ProductRepository.GetAll().Where(t => t.Price == 0).ToList();
            var popularGames = _unitOfWork.ProductRepository.GetProductWithCategories().Where(t => (t.Developer.Equals("EA")) || (t.Developer.Equals("Rockstar Games"))).Take(6).ToList();
            var recentReleases = _unitOfWork.ProductRepository.GetProductWithCategories().OrderByDescending(t => t.ReleaseDate).Take(6).ToList();

            HomePageViewModel homePage = new HomePageViewModel()
            {
                BestSellers = bestSellers,
                FreeGames = freeGames,
                PopularGames = popularGames,
                RecentReleases = recentReleases
            };

            return View(homePage);
        }

        [HttpGet]
        [Route("product-detail/{id}.html", Name = "ProductDetail")]
        public IActionResult ProductDetail(int id)
        {
            var productDetail = _unitOfWork.ProductRepository.GetProductWithCategories(id).FirstOrDefault();
            var relatedGames = _unitOfWork.ProductRepository.GetProductWithCategories().Take(6).ToList();
            var popularGames = _unitOfWork.ProductRepository.GetProductWithCategories().Where(t => (t.Developer.Equals("EA")) || (t.Developer.Equals("Rockstar Games"))).ToList();
            var categories = _unitOfWork.CategoryRepository.GetAll().OrderBy(i => i.Id).ToList();

            DetailPageViewModel detailPage = new DetailPageViewModel()
            {
                ProductDetail = productDetail,
                RelatedGames = relatedGames,
                PopularGames = popularGames,
                Categories = categories
            };

            return View(detailPage);
        }

        [HttpGet]
        public IActionResult FindProductsByName()
        {
            return View();
        }

        [HttpGet]
        [Route("search-product.html")]
        public IActionResult ProductsByName([FromQuery(Name = "name")]string name)
        {
            List<Product> products = new List<Product>();
            if (name == "all" || name == null)
            {
                products = _unitOfWork.ProductRepository.GetAll();
            }
            else
            {
                products = _unitOfWork.ProductRepository.GetProductByName(name.ToLower().Trim());
            }
            return View(products);
        }
    }
}
