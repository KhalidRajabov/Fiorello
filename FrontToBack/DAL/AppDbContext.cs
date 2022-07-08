using FrontToBack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack
{
    public class AppDbContext : IdentityDbContext<AppUser>
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