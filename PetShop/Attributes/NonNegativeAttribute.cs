using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetShop.Attributes
{
    public class NonNegativeAttribute : ValidationAttribute
    {
        public NonNegativeAttribute() : base("Number cannot be negative") { }
    
        public override bool IsValid(object value)
        {
            try
            {
                int intValue = Convert.ToInt32(value);
                return intValue >= 0;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
    
}