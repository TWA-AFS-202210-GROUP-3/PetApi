using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
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

        [HttpGet("findPetByName")]
        public Pet FindPetByName([FromQuery]string name)
        {
            return pets.Find(pet => pet.Name.Equals(name));
        }

        [HttpPatch("removeOnePet")]
        public List<Pet> RemoveOnePet(Pet pet)
        {
            var matchedPet = pets.Find(item => item.Name.Equals(pet.Name));
            pets.Remove(matchedPet);
            return pets;
        }

        [HttpPatch("editPetPrice")]
        public Pet EditPetPrice(Pet pet)
        {
            var matchedPet = pets.Find(item => item.Name.Equals(pet.Name));
            matchedPet.Price = pet.Price;
            return matchedPet;
        }

        [HttpDelete("deleteAllPets")]
        public List<Pet> DeleteAllPets()
        {
            pets.Clear();
            return pets;
        }
    }
}
