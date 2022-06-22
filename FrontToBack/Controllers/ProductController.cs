using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.Take(2).Include(p=>p.Category).ToList();
            return View(products);
        }

        public IActionResult LoadMore()
        {
            /*List<Product> products = _context.Products.Skip(2).Take(2).Include(p => p.Category).ToList();
            List<ProductReturnVM> returnedproduct = new List<ProductReturnVM>();
            foreach (var item in products)
            {
                ProductReturnVM productReturnVM = new ProductReturnVM();
                productReturnVM.Id = item.Id;
                productReturnVM.Name=item.Name;
                productReturnVM.Price=item.Price;
                productReturnVM.CategoryId=item.CategoryId;
                productReturnVM.Category = item.Category.Name;
                returnedproduct.Add(productReturnVM);
            }*/
            /*
                        List<ProductReturnVM> products = _context.Products.Select(p => new ProductReturnVM
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CategoryId = p.CategoryId,
                            Category = p.Category.Name
                        }).ToList();*/

            //List<Product> products = _context.Products.Skip(2).Take(2).Include(p => p.Category).ToList();
            return View();
        }
    }
}
