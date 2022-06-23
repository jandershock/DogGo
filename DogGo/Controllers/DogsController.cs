using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepository;

        public DogsController(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }


        // GET: DogController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepository.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.Owner = new Owner() { Id = GetCurrentUserId()};
                _dogRepository.AddDog(dog);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            if (dog.Owner.Id != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                if (dog.Owner.Id != GetCurrentUserId())
                {
                    return NotFound();
                }
                _dogRepository.UpdateDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            if (dog.Owner.Id != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                if (dog.Owner.Id != GetCurrentUserId())
                {
                    return NotFound();
                }
                _dogRepository.DeleteDogEntry(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
