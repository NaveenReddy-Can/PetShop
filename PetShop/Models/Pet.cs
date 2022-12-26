using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetShop.Attributes;

namespace PetShop.Models
{
    // Pet model class with 5 properties
    public class Pet
    {
        public int Id { get; set; }

        [NoDigits]
        public string Name { get; set; }

        [NonNegative]
        public int age { get; set; }

        public bool IsMale { get; set; }
        public string Breed { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}