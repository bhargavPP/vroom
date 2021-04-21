using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.AppDbContext;
using vroom.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using vroom.Models;
using cloudscribe.Pagination.Models;

namespace vroom.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }

        public BikeController(VroomDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new Models.Bike()
            };
        }
        public IActionResult Index2()
        {
            var Bike = _db.Bikes.Include(m => m.Make).Include(m => m.Model);
            return View(Bike.ToList());
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 3)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;
            var Bike = _db.Bikes.Include(m => m.Make).Include(m => m.Model).Skip(ExcludeRecords).Take(pageSize);
            var result = new PagedResult<Bike>
            {
                Data = Bike.AsNoTracking().ToList(),
                TotalItems = _db.Bikes.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View(result);
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            _db.Bikes.Add(BikeVM.Bike);
            //_db.SaveChanges();

            //save bike Logic
            //get bike id we have saved in database
            var BikeID = BikeVM.Bike.Id;

            //get wwwroot path to save file in folder
            string wwrootpath = _hostingEnvironment.WebRootPath;

            //get uploaded files
            var files = HttpContext.Request.Form.Files;

            //get reference of DBset for bike just have saved in database
            var SavedBike = _db.Bikes.Find(BikeID);

            //upload the file on server and save image  pathe of user
            if (files.Count != 0)
            {
                var ImagePath = @"Image\bike\";
                var Extension = Path.GetExtension(files[0].FileName);
                var relativeImagePath = ImagePath + BikeID + Extension;
                var AbsImagePath = Path.Combine(wwrootpath, relativeImagePath);

                //upload file on server
                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                //set Image path to database
                SavedBike.ImagePath = relativeImagePath;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            BikeVM.Bike = _db.Bikes.Include(m => m.Make).SingleOrDefault(m => m.Id == Id);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }
        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(int Id)
        {

            if (!ModelState.IsValid)
            {
                return View(BikeVM);
            }
            _db.Update(BikeVM.Bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Bike bike = _db.Bikes.Find(Id);
            if (bike == null)
            {
                return NotFound();
            }
            _db.Bikes.Remove(bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}