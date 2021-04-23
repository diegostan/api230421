using Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }

        public DbSet<Category> Categories{get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<User> Users{get;set;}
        
    }
    
}