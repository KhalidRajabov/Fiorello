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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bio>().HasData(

                new Bio
                {
                    Id = 1, ImageUrl= "logo.png", AuthorName = "Me Myself", 
                    Linkedin = "www.linkedin.com", Facebook = "www.facebook.com"
                }

                );

            builder.Entity<AuthorSlider>().HasData(

                new AuthorSlider
                {
                    Id = 1, Quote = "Nullam dictum felis eu pede mollis pretium. " +
                    "Integer tincidunt. Cras dapibus lingua. ",
                    Name = "Ann Barnes",
                    Position = "Florist",
                    ImageUrl = "testimonial-img-1.png"
                },
                new AuthorSlider
                {
                    Id = 2,
                    Quote = "Nullam dictum felis eu pede mollis pretium. " +
                    "Integer tincidunt. Cras dapibus lingua. ",
                    Name = "Ann Barnes",
                    Position = "Florist",
                    ImageUrl = "testimonial-img-2.png"
                }

                ) ;

            builder.Entity<Blogs>().HasData(
                
                new Blogs
                {
                    Id = 1, Subject="Local Florist",
                    BlogDetail = $"{Faker.Lorem.Paragraph()}",
                    ImageUrl="blog-feature-img-1.jpg"
                },
                new Blogs
                {
                    Id = 2,
                    Subject = "Local Florist",
                    BlogDetail = $"{Faker.Lorem.Paragraph()}",
                    ImageUrl = "blog-feature-img-1.jpg"
                },
                new Blogs
                {
                    Id = 3,
                    Subject = "Local Florist",
                    BlogDetail = $"{Faker.Lorem.Paragraph()}",
                    ImageUrl = "blog-feature-img-1.jpg"
                }

                );
            builder.Entity<BottomSlider>().HasData(
                
                new BottomSlider
                {
                    Id=1, ImageUrl="instagram1.jpg"
                },
                new BottomSlider
                {
                    Id = 2,
                    ImageUrl = "instagram2.jpg"
                },
                new BottomSlider
                {
                    Id = 3,
                    ImageUrl = "instagram3.jpg"
                },
                new BottomSlider
                {
                    Id = 4,
                    ImageUrl = "instagram4.jpg"
                },
                new BottomSlider
                {
                    Id = 5,
                    ImageUrl = "instagram5.jpg"
                },
                new BottomSlider
                {
                    Id = 6,
                    ImageUrl = "instagram6.jpg"
                },
                new BottomSlider
                {
                    Id = 7,
                    ImageUrl = "instagram7.jpg"
                },
                new BottomSlider
                {
                    Id = 8,
                    ImageUrl = "instagram8.jpg"
                }

                );

            builder.Entity<Category>().HasData(
                
                new Category
                {
                    Id = 1,
                    Name = "Winter",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 2,
                    Name = "Various",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 3,
                    Name = "Exotic",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 4,
                    Name = "Green",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 5,
                    Name = "Cactus",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 6,
                    Name = "Popular",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 7,
                    Name = "Cats",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                },
                new Category
                {
                    Id = 8,
                    Name = "Useless",
                    Desc = $"{Faker.Lorem.Paragraph()}"
                }
                );
            builder.Entity<Employees>().HasData(
                
                new Employees
                {
                    Id=1, Name = $"{Faker.Name.FullName()}", Position = "Florist",
                    ImageUrl = "h3-team-img-1.png"
                },
                new Employees
                {
                    Id = 2,
                    Name = $"{Faker.Name.FullName()}",
                    Position = "Manager",
                    ImageUrl = "h3-team-img-2.png"
                },
                new Employees
                {
                    Id = 3,
                    Name = $"{Faker.Name.FullName()}",
                    Position = "Florist",
                    ImageUrl = "h3-team-img-3.png"
                },
                new Employees
                {
                    Id = 4,
                    Name = $"{Faker.Name.FullName()}",
                    Position = "Florist",
                    ImageUrl = "h3-team-img-4.png"
                }

                );

            builder.Entity<Product>().HasData(
                
                new Product
                {
                    Id=1, Name="Alyssa", ImageUrl="shop-12-img.img",
                    Price=150, CategoryId=1, Count=10
                }
                
            );
            builder.Entity<SliderContent>().HasData(
                
                new SliderContent
                {
                    Id=1, Title= "<h1>Send <span>flowers</span> like</h1>< h1 > you mean it </ h1 > ",
                    Desc = $"{Faker.Lorem.Paragraph()}", ImageUrl="h2-sign-img.png"
                }
                
                );
            builder.Entity<Slider>().HasData(

                new Slider
                {
                    Id = 1,
                    ImageUrl = "356cf1f9-68e4-4f02-b4d6-b6d18695c26bh3-slider-background.jpg"
                },
                new Slider
                {
                    Id = 2,
                    ImageUrl = "f50fb178]-6841-44a5-8da4-4d2c462632d3h3-slider-background-2.jpg"
                },
                new Slider
                {
                    Id = 3,
                    ImageUrl = "5d3af72d-f7b6-49c7-a60b-d01cc558ed6eh3-slider-background-3.jpg"
                }
                );





        }


    }
}