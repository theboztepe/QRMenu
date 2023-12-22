using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Helper;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using static DataAccess.Helper.InheriteClass;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, QRMenuContext>, ICategoryDal
    {
        public List<Category> GetUserCategories(int userId, int topCategoryId)
        {
            using QRMenuContext context = new();
            IQueryable<Category> result = from c in context.Categories
                                          where c.UserId == userId && c.TopCategoryId == topCategoryId
                                          select c;
            return result.ToList();
        }

        public CategoryTree GetUserCategoriesTree(int userId)
        {
            using QRMenuContext context = new();
            List<CategoriesTree> categories = (from c in context.Categories
                                                  where c.UserId == userId
                                                  select new CategoriesTree
                                                  {
                                                      Id = c.Id,
                                                      Description = c.Description,
                                                      Name = c.Name,
                                                      UserId = c.UserId,
                                                      TopCategoryId = c.TopCategoryId,
                                                      SubCategories = new List<CategoriesTree>(),
                                                  }).ToList();

            CategoryTree result = new(categories);
            return result;
        }

        public CategoryTree GetUserCategoriesTreeWithProduct(int userId, IProductDal productDal)
        {
            using QRMenuContext context = new();
            List<CategoriesTree> categories = (from c in context.Categories
                                               where c.UserId == userId
                                               select new CategoriesTree
                                               {
                                                   Id = c.Id,
                                                   Description = c.Description,
                                                   Name = c.Name,
                                                   UserId = c.UserId,
                                                   TopCategoryId = c.TopCategoryId,
                                                   SubCategories = new List<CategoriesTree>(),
                                                   Products = new List<Product>()
                                               }).ToList();

            CategoryTree result = new(categories, productDal);
            return result;
        }

    }
}
