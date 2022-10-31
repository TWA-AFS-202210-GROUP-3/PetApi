using Microsoft.AspNetCore.Mvc;
using PetApiTest.ControllerTest;
using System.Collections.Generic;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("getPetByName")]
        public Pet GetPetByName([FromQuery]string name)
        {
            return pets.Find(pet => pet.Name == name);
        }

        [HttpDelete("delAllPets")]
        public void DelAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("delPetByName")]
        public void DelPetByName([FromQuery] string name)
        {
            var petFound = pets.Find(pet => pet.Name == name);
            pets.Remove(petFound);
        }

        [HttpPatch("modifyPetPrice")]
        public void ModifyPetPrice(Pet petModified)
        {
            pets.Find(pet => pet.Name == petModified.Name).Price = petModified.Price;

        }
    }
}
