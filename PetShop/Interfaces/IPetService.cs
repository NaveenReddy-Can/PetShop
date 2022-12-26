using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetShop.Models;

namespace PetShop.Interface
{
    internal interface IPetService
    {
        
        bool OldEnoughToAdopt(DateTime dateTime);

        IEnumerable<Pet> GetAllPets();

        Pet GetPetById(int? id);

        IEnumerable<Pet> GetPetsByBreed(string breed);
    }
}