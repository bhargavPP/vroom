﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using vroom.AppDbContext;
using vroom.Controllers.Resources;
using vroom.Models;
using vroom.Models.ViewModels;

namespace vroom.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly IMapper _mapper;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; }

        public ModelController(VroomDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };
        }
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.Models.Add(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == Id);
            if (ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }
        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(int Id)
        {

            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.Update(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Model model = _db.Models.Find(Id);
            if (model == null)
            {
                return NotFound();
            }
            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]
        public IEnumerable<ModelResources> Models(int MakeID)
        {
            //return _db.Models.ToList().Where(m=>m.MakeID==MakeID);
            //////////////Autommaper user/////////////
            //create mapper configuration
            var models = _db.Models.ToList().Where(m => m.MakeID == MakeID).ToList();
            //var config = new MapperConfiguration(mc => mc.CreateMap<Model, ModelResources>());
            //var mapper = new Mapper(config);

            var modelResources = _mapper.Map<List<Model>, List<ModelResources>>(models);

            return modelResources;
            //////////////Autommaper user/////////////
            //second method to map 
            //var models = _db.Models.ToList();
            //var modelResources = models.Select(m => new ModelResources { Id = m.Id, Name = m.Name }).ToList();

            //return modelResources;
        }
    }
}