using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Overview { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int DeveloperId { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }
        public Developer Developer { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
