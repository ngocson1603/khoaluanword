using System.Collections.Generic;

namespace GameStore.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}