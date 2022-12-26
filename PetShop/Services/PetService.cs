using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetShop.Interface;
using PetShop.Models;

namespace PetShop.Services
{
    public class PetService : IPetService
    {
        private ApplicationDbContext context;

        public PetService()
        {
            context = new ApplicationDbContext();
        }

        public PetService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool OldEnoughToAdopt(DateTime dateTime)
        {
            TimeSpan difference = DateTime.Now.Subtract(dateTime);
            if (difference.Days > 365 * 18) return true;
            return false;
        }

        public IEnumerable<Pet> GetAllPets()
        {
            return context.Pets.ToList();
        }

        public Pet GetPetById(int? id)
        {
            return context.Pets.Where(p => p.Id == id).First();
        }

        public IEnumerable<Pet> GetPetsByBreed(string breed)
        {
            return context.Pets.Where(p => p.Breed == breed);
        }
    }
}