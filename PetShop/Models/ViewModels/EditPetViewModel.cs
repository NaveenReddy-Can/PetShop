using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetShop.Attributes;

namespace PetShop.Models.ViewModels
{
    public class EditPetViewModel
    {
        public int Id { get; set; }

        [NoDigits]
        public string Name { get; set; }

        [NonNegative]
        public int age { get; set; }

        public bool IsMale { get; set; }
        public string Breed { get; set; }
        public List<SelectListItem> DropDownBreed { get; set; }
    }
}