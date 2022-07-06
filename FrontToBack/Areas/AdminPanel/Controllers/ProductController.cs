using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            return View(_context.Products.Include(p => p.Category).ToList());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.Include(c=>c.Category).FirstOrDefaultAsync(c=>c.Id==id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            if (product.Photo==null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }

            if (!product.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (product.Photo.Length/1024>200)
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }
            if (!(product.CategoryId>0))
            {
                ModelState.AddModelError("CategoryId", "Choose a category");
                return View();
            }

            string filename = Guid.NewGuid().ToString() + product.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", filename);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                product.Photo.CopyTo(stream);
            };

            Product NewProduct = new Product
            {
                Price = product.Price,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = filename
            };

            _context.Products.Add(NewProduct);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
