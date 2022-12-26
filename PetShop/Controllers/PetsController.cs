using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PetShop.Interface;
using PetShop.Models;
using PetShop.Models.ViewModels;
using PetShop.Services;

namespace PetShop.Controllers
{
    public class PetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IPetService petService;

        public PetsController(ApplicationDbContext db, PetService petService)
        {
            this.db = db;
            this.petService = petService;
        }

        public ActionResult GetPetsByBreed(string breed)
        {
            petService = new PetService(db);
            return PartialView("_PetListPartial",petService.GetPetsByBreed(breed));
        }
        

        // GET: Pets
        [AllowAnonymous]
        public ActionResult Index()
        {
            petService = new PetService(db);
            IEnumerable<Pet> pets = petService.GetAllPets();

            List<string> breeds = new List<string>();

            foreach(Pet pet in pets)
            {
                if (!breeds.Contains(pet.Breed))
                {
                    breeds.Add(pet.Breed);
                }
            }

            return View(breeds);
        }

        // GET: Pets/Details/5
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // GET: Pets/Create
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public ActionResult Create()
        {
            petService = new PetService(db);
            IEnumerable<Pet> pets = petService.GetAllPets();

            List<string> breedsString = new List<string>();
            List<SelectListItem> breeds = new List<SelectListItem>();

            foreach (Pet pet in pets)
            {
                if (!breedsString.Contains(pet.Breed))
                {
                    breedsString.Add(pet.Breed);
                    breeds.Add(new SelectListItem { Text = pet.Breed, Value = pet.Breed ,Selected = false});
                }
            }

            CreatePetViewModel cpvm = new CreatePetViewModel
            {
                DropDownBreed = breeds
            };
            return View(cpvm);
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public JsonResult Create([Bind(Include = "Id,Name,age,IsMale,Breed")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return Json(new {text = "Success!"});
            }

            return Json(new {text = "Something went wrong!"});
        }

        // GET: Pets/Edit/5
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public ActionResult Edit(int? id)
        {
            petService = new PetService(db);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = petService.GetPetById(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Pet> pets = petService.GetAllPets();

            List<string> breedsString = new List<string>();
            List<SelectListItem> breeds = new List<SelectListItem>();

            foreach (Pet p in pets)
            {
                if (!breedsString.Contains(p.Breed))
                {
                    breedsString.Add(p.Breed);
                    if (pet.Breed == p.Breed)
                    {
                        breeds.Add(new SelectListItem { Text = p.Breed, Value = p.Breed, Selected = true });
                    }
                    else
                    {
                        breeds.Add(new SelectListItem { Text = p.Breed, Value = p.Breed, Selected = false });
                    }
                    
                }
            }

            EditPetViewModel epvm = new EditPetViewModel
            {
                DropDownBreed = breeds,
                Id = pet.Id,
                Name = pet.Name,
                age = pet.age,
                IsMale = pet.IsMale,
                Breed = pet.Breed
            };

            return View(epvm);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public JsonResult Edit([Bind(Include = "Id,Name,age,IsMale,Breed")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new {text = "Success!"});
            }
            return Json(new {text = "Something went wrong!"});
        }

        // GET: Pets/Delete/5
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // These methods can only be accessed by admin role users
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// ////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult MyPets()
        {
            return View("MyPets", db.Pets.Include(x => x.Owner)
                .Where(a => a.Owner.UserName == User.Identity.Name)
                .ToList());
        }

        [HttpGet]
        [Authorize]
        // Adopt method that looks up for a pet with Id and returns a view
        public ActionResult Adopt(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if(pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adopt(Pet pet)
        {
            var user = db.Users.Include(x => x.Pets)
                .Where(x => x.UserName == User.Identity.Name)
                .First();

            var p = db.Pets.Find(pet.Id);

            var claimUser = (ClaimsPrincipal)User;

            var dateOfBirth = Convert.ToDateTime(
                claimUser.Claims.Where(claim => claim.Type == ClaimTypes.DateOfBirth)
                .First()
                .Value);
            var age = DateTime.Now.Subtract(dateOfBirth);

            if(age.Days >= 365*18)
            {
                p.Owner = user;
                user.Pets.Add(p);
                db.SaveChanges();

            }

            return RedirectToAction("MyPets");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
