using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.ViewModels
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public int DeveloperId { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }
        public string Category { get; set; }
        public string Developer { get; set; }
    }

    public class ProductDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public int DeveloperId { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }
        public List<string> Categories { get; set; }
        public string Developer { get; set; }
    }
}
