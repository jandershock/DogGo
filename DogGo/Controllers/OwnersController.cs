using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownersRepository;
        private readonly IDogRepository _dogRepository;
        private readonly IWalkerRepository _walkerRepository;
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public OwnersController
            (
            IOwnerRepository ownersRepository, 
            IDogRepository dogRepository, 
            IWalkerRepository walkerRepository, 
            INeighborhoodRepository neighborhoodRepository
            )
        {
            _ownersRepository = ownersRepository;
            _dogRepository = dogRepository;
            _walkerRepository = walkerRepository;
            _neighborhoodRepository = neighborhoodRepository;
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

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = _dogRepository.GetDogsByOwnerId(id),
                Walkers = _walkerRepository.GetWalkersByNeighborhood(owner.NeighborhoodId)
            };

            return View(vm);
        }

        // GET: OwnersController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };
            return View(vm);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownersRepository.AddOwner(owner);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            Owner owner = _ownersRepository.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = owner,
                Neighborhoods = _neighborhoodRepository.GetAll()
            };

            return View(vm);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownersRepository.UpdateOwner(owner);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownersRepository.GetOwnerById(id);

            return View(owner);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownersRepository.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }
    }
}
