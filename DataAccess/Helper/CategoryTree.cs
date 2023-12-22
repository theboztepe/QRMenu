using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using static DataAccess.Helper.InheriteClass;

namespace DataAccess.Helper
{
    public class CategoryTree
    {
        private readonly IProductDal _productDal;

        public List<CategoriesTree> Root { get; set; }
        private readonly List<CategoriesTree> categories;

        public CategoryTree(List<CategoriesTree> categories, IProductDal productDal = null)
        {
            this._productDal = productDal;
            this.categories = categories;
            Root = categories.Where(c => c.TopCategoryId == 0).ToList();
            if (!Root.Any())
            {
                throw new InvalidOperationException("Root category not found.");
            }
            foreach (var item in Root)
            {
                BuildTree(item);
            }
        }

        private void BuildTree(CategoriesTree parentCategory)
        {
            var subcategories = categories.Where(c => c.TopCategoryId == parentCategory.Id);
            foreach (var subcategory in subcategories)
            {
                if (_productDal != null && subcategory.Products != null)
                {
                    subcategory.Products.AddRange(_productDal.GetCategoryProducts(subcategory.UserId, subcategory.Id));
                }
                parentCategory.SubCategories.Add(subcategory);
                BuildTree(subcategory);
            }
        }
    }
}
