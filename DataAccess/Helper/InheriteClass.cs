using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Helper
{
    public class InheriteClass
    {
        public class CategoriesTree : Category
        {
            public List<Product> Products { get; set; }
            public List<CategoriesTree> SubCategories { get; set; }
        }
    }
}
