using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class QRMenuContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=QRMenu;uid=sa;pwd=1;multipleactiveresultsets=True");
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
