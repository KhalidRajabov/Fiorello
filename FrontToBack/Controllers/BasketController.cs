using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _usermanager;

        public BasketController(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _usermanager = userManager;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public async Task<IActionResult> AddItem(int? id)
        {
            string username="";
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;   
            }
            if (id == null)
            if (id==null)return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();

            List<BasketVM> products;
            if (Request.Cookies[$"basket{username}"]==null)
            {
               products = new List<BasketVM>();
            }
            else
            {
                products= JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[$"basket{username}"]);
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
            
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });
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
            string username = "";
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            //string name = HttpContext.Session.GetString("name");
            string basket = Request.Cookies[$"basket{username}"];
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
            string username = "";
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return NotFound();
            string basket = Request.Cookies[$"basket{username}"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbProduct = products.Find(p=>p.Id==id);
            if (dbProduct == null) return NotFound();


            products.Remove(dbProduct);
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });

            double subtotal = 0;
            int basketCount = 0;

            if (products.Count > 0)
            {
                foreach (BasketVM item in products)
                {
                    subtotal += item.Price * dbProduct.ProductCount;
                    basketCount += item.ProductCount;
                }
            }

            var obj = new
            {
                Price = $"(${subtotal})",
                Count = basketCount
            };
            return Ok(obj);
        }

        public IActionResult Minus(int? id)
        {
            string username = "";
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return NotFound();
            string basket = Request.Cookies[$"basket{username}"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return NotFound();


            double subtotal = 0;
            int basketCount = 0;

            if (dbproducts.ProductCount>1)
            {
                dbproducts.ProductCount--;
                Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(dbproducts));
            }
            else
            {
                products.Remove(dbproducts);


                List<BasketVM> productsNew = products.FindAll(p => p.Id != id);

                Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(productsNew));

                foreach (BasketVM pr in productsNew)
                {
                    subtotal += pr.Price * pr.ProductCount;
                    basketCount += pr.ProductCount;
                }

                var obje= new
                {
                    count = 0,
                    price = subtotal,
                    main = basketCount
                };

                return Ok(obje);
            }
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions 
            { 
                MaxAge = TimeSpan.FromDays(100)
            });


            foreach (var product in products)
            {
                subtotal += product.Price * product.ProductCount;
                basketCount += product.ProductCount;
            }

            var obj = new
            {
                Price = subtotal,
                Count = dbproducts.ProductCount,
                main = basketCount,
                itemTotal =dbproducts.Price*dbproducts.ProductCount
            };
            return Ok(obj);
        }
        public IActionResult Plus(int? id)
        {
            string username = "";
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return NotFound();
            string basket = Request.Cookies[$"basket{username}"];
            List<BasketVM> products;
            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return NotFound();
            dbproducts.ProductCount++;
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions 
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
                main = dbproducts.ProductCount,
                itemTotal = dbproducts.Price * dbproducts.ProductCount
            };

            return Ok(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sale()
        {
            string username="";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;   
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                Sale sale = new Sale();
                sale.SaleDate = DateTime.Now;
                sale.AppUserId = user.Id;
                List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[$"basket{username}"]);
                List<SalesProduct> salesProducts = new List<SalesProduct>();
                double Total = 0;
                foreach (var baskepProducts in basket)
                {
                    Product dbProduct = await _context.Products.FindAsync(baskepProducts.Id);
                    if (baskepProducts.ProductCount>dbProduct.Count)
                    {
                        TempData["Fail"] = "Purchase failed. Not enough product in storehouse left...";
                        return RedirectToAction("showitem");
                    }
                    SalesProduct salesProduct = new SalesProduct();
                    salesProduct.ProductId = dbProduct.Id;
                    salesProduct.Count = baskepProducts.ProductCount;
                    salesProduct.Id = sale.Id;
                    salesProduct.Price = dbProduct.Price;
                    salesProducts.Add(salesProduct);
                    Total += baskepProducts.ProductCount * dbProduct.Price;
                    dbProduct.Count -= baskepProducts.ProductCount;
                }
                sale.SalesProducts = salesProducts;
                sale.Total=Total;
                await _context.AddAsync(sale);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Purchase succesfull";
                return RedirectToAction("showitem");
            }
            else
            {
            return RedirectToAction("login", "account");
            }
        }
    }
}
