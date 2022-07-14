﻿using FrontToBack.Extensions;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index(int page=1, int take=5)
        {

            List<Product> product = _context.Products.Include(p => p.Category)
                .Skip((page-1)*take).Take(take).ToList();
            PaginationVM<Product> paginationVM = new PaginationVM<Product>(product, PageCount(take), page);
            return View(paginationVM);
        }

        private int PageCount(int take)
        {
            List<Product> products = _context.Products.ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
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

            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (product.Photo.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }
            if (!(product.CategoryId>0))
            {
                ModelState.AddModelError("CategoryId", "Choose a category");
                return View();
            }

            

            Product NewProduct = new Product
            {
                Price = product.Price,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Count = product.Count,
                ImageUrl = product.Photo.SaveImage(_env, "img")
            };

            _context.Products.Add(NewProduct);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            if (id == null) return NotFound();
            Product product= await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
         
            if (product.Photo == null)
            {
                dbProduct.ImageUrl= dbProduct.ImageUrl;
            }
            else
            {
                if (!product.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Choose images only");
                    return View();
                }
                if (product.Photo.ValidSize(20000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                string oldImg = dbProduct.ImageUrl;
                string path = Path.Combine(_env.WebRootPath, "img", oldImg);
                dbProduct.ImageUrl = product.Photo.SaveImage(_env, "img");
                Helper.Helper.DeleteImage(path);

            }




            dbProduct.Name = product.Name;
            dbProduct.Price= product.Price;
            dbProduct.Count = product.Count;
            dbProduct.CategoryId= product.CategoryId;
           

            //await _context.AddAsync(dbProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Product product= await _context.Products.FindAsync(id);
            if (product== null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", product.ImageUrl);
            Helper.Helper.DeleteImage(path);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
