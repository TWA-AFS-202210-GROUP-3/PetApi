using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        private static readonly List<Pet> pets = new List<Pet>();
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

        [HttpGet("findPetsByName")]
        public Pet FindPetsByName(string name)
        {
            return pets.First(pet => pet.Name.Equals(name));
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("deleteSoldPet")]
        public List<Pet> DeleteSoldPet(string name)
        {
            var soldPets = pets.First(_ => _.Name.Equals(name));
            pets.Remove(soldPets);
            return pets;
        }
    }
}
