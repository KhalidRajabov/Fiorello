using FrontToBack.Models;
using FrontToBack.Services;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack.Controllers
{

    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISum _sum;

        public HomeController(AppDbContext context, ISum sum)
        {
            _context = context;
            _sum = sum;
        }

        
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            //slider ToList metodu ishledir chunki tekrarlanan datalar var
            homeVM.Sliders = _context.Sliders.ToList();

            //tekrarlanan data olmadigi uchun ToList yazilmir, firstordefault yazilir ki data choxdursa
            //en birinci datani, eger yoxdursa null gelsin
            homeVM.SliderContent = _context.SliderContents.FirstOrDefault();

            //eyni qaydada, tekrarlanan datalar oldugu uchun tolist atilir
            homeVM.Categories = _context.Categories.ToList();

            //eyni qayda
            homeVM.Products = _context.Products.Skip(3).ToList();

            homeVM.Employees=_context.Employees.ToList();

            homeVM.Blogs= _context.Blogs.ToList();
            homeVM.AuthorSliders = _context.AuthorSliders.ToList();

            homeVM.BottomSliders=_context.BottomSliders.ToList();

            return View(homeVM);
        }

        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                .Include(p=>p.Category)
                .OrderBy(p => p.Id).Where(p => p.Name.ToLower().Contains(search.ToLower()))
                .Take(10).ToList();

            return PartialView("_SearchPartial", products);
        }
    }
}
