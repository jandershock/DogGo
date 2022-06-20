using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownersRepository;
        private readonly IDogRepository _dogRepository;

        public OwnersController(IOwnerRepository ownersRepository, IDogRepository dogRepository)
        {
            _ownersRepository = ownersRepository;
            _dogRepository = dogRepository;
        }

        // GET: OwnersController
        public ActionResult Index()
        {
            List<Owner> allOwners = _ownersRepository.GetAllOwners();
            return View(allOwners);
        }

        // GET: OwnersController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownersRepository.GetOwnerById(id);
            List<Dog> ownersDogs = _dogRepository.GetDogsByOwnerId(id);
            ViewData["dogs"] = ownersDogs;
            return View(owner);
        }

        // GET: OwnersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OwnersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
