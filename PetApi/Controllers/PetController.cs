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

        [HttpGet("findPetByName")]
        public Pet FindPetByName(string name)
        {
            return pets.First(pet => pet.Name.Equals(name));
        }

        [HttpGet("findPetsByType")]
        public List<Pet> FindPetsByType(string type)
        {
            return pets.FindAll(pet => pet.Type.Equals(type));
        }

        [HttpGet("findPetsByColor")]
        public List<Pet> FindPetsByColor(string color)
        {
            return pets.FindAll(pet => pet.Color.Equals(color));
        }

        [HttpGet("findPetsByPriceRange")]
        public List<Pet> FindPetsByPriceRange(int min, int max)
        {
            return pets.FindAll(pet => pet.Price >= min && pet.Price <= max);
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
