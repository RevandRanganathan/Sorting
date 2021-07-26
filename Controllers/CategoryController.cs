using M2.Data;
using M2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace M2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            Configuration = configuration;
        }
        private readonly IConfiguration Configuration;



        public async Task<IActionResult> Index(string sortOrder, string SearchString, int pageNumber = 1)
        {
            var Categ = _db.Categories.AsQueryable();



            if (!String.IsNullOrEmpty(SearchString))
            {
                Categ = Categ.Where(s => s.Name.Contains(SearchString));
            }



            ViewData["NameOrder"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : " ";



            switch (sortOrder)
            {
                case "name_desc":
                    Categ = Categ.OrderByDescending(u => u.Name);
                    break;
                default:
                    Categ = Categ.OrderBy(u => u.Name);
                    break;
            }



            return View(await PaginatedList<Category>.CreateAsync(Categ.AsNoTracking(), pageNumber, 5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
