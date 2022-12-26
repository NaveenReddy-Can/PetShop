using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetShop.Models;
using PetShop.Services;

namespace PetShop.Tests.Services
{
    [TestClass]
    public class PetServiceTests
    {
        [TestMethod]
        public void PetService_OldEnoughToAdopt_OldEnough()
        {
            PetService petService = new PetService(new ApplicationDbContext());
            DateTime oldEnough = DateTime.Now.AddYears(-18);
            bool expectedResult = true;

            bool actualResult = petService.OldEnoughToAdopt(oldEnough);

            Assert.AreEqual(expectedResult, actualResult);
            
        }

        [TestMethod]
        public void PetService_OldEnoughToAdopt_NotOldEnough()
        {
            PetService petService = new PetService(new ApplicationDbContext());
            DateTime oldEnough = DateTime.Now.AddYears(-14);
            bool expectedResult = false;

            bool actualResult = petService.OldEnoughToAdopt(oldEnough);

            Assert.AreEqual(expectedResult, actualResult);

        }

    }
}
