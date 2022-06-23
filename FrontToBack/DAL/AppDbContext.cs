using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Blogs> Blogs { get; set; }

        public DbSet <AuthorSlider> AuthorSliders { get; set; }

        public DbSet<BottomSlider> BottomSliders { get; set; }

        public DbSet<Bio> Bios { get; set; }
    }
}