using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {

        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public async Task<IActionResult> AddItem(int? id)
        {
            if (id==null)return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();

            List<BasketVM> products;
            if (Request.Cookies["basket"]==null)
            {
               products = new List<BasketVM>();
            }
            else
            {
                products= JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            BasketVM IsExist = products.Find(x => x.Id == id);
            if (IsExist==null)
            {
                BasketVM basketvm = new BasketVM
                {
                    Id = dbProduct.Id,
                    ProductCount = 1,
                    Price = dbProduct.Price
                };
                products.Add(basketvm);
            }
            else
            {
                IsExist.ProductCount++;
            }
            
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });
            double price = 0;
            double count = 0;

            foreach (var product in products)
            {
                price += product.Price * product.ProductCount;
                count += product.ProductCount;
            }

            var obj = new
            {
                Price = price,
                Count = count,
            };
            //obj data-id ile baghlidir. response "obj" obyektidir,
            //Ok'in icnde return edilmelidir ki API'de response gorsun
            return Ok(obj);
        }
        public IActionResult ShowItem()
        {

            //string name = HttpContext.Session.GetString("name");
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;

            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in products)
                {
                    Product dbProducts = _context.Products.FirstOrDefault(x => x.Id == item.Id);
                    item.Name=dbProducts.Name;
                    item.Price = dbProducts.Price;
                    item.ImageUrl = dbProducts.ImageUrl;
                }
            }
            else
            {
                products = new List<BasketVM>();
            }
            return View(products);
        }
        public  IActionResult RemoveItem(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbProduct = products.Find(p=>p.Id==id);
            if (dbProduct == null) return NotFound();


            products.Remove(dbProduct);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });
            return RedirectToAction("showitem","basket");
        }

        public IActionResult Minus(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return NotFound();
            if (dbproducts.ProductCount>1)
            {
                dbproducts.ProductCount--;
            }
            else
            {
                products.Remove(dbproducts);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions 
            { 
                MaxAge = TimeSpan.FromDays(100) 
            });

            double price = 0;
            double count = 0;

            foreach (var product in products)
            {
                price += product.Price * product.ProductCount;
                count += product.ProductCount;
            }
            var obj = new
            {
                Price = price,
                Count = count,
                main = dbproducts.ProductCount
            };
            return Ok(obj);
        }
        public IActionResult Plus(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return NotFound();
            dbproducts.ProductCount++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions 
            {
                MaxAge = TimeSpan.FromDays(100) 
            });

            double price = 0;
            double count = 0;
          
            foreach (var product in products)
            {
                price += product.Price * product.ProductCount;
                count += product.ProductCount;
            }
            var obj = new
            {
                Price = price,
                Count = count,
                main = dbproducts.ProductCount
            };


            return Ok(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sale()
        {
            return View();
        }
    }
}
