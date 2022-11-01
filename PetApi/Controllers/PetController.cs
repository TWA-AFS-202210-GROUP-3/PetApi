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
            return pets.FindAll(_ => _.Type.Equals(type));
        }

        [HttpGet("findPetsByColor")]
        public List<Pet> FindPetsByColor(string color)
        {
            return pets.FindAll(_ => _.Color.Equals(color));
        }

        [HttpGet("findPetsByPriceRange")]
        public List<Pet> FindPetsByPriceRange(int min, int max)
        {
            return pets.FindAll(_ => _.Price >= min && _.Price <= max);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpPatch("PetPriceModification")]
        public Pet petPriceModification(Pet pet)
        {
            var newpet = pets.First(_ => _.Name.Equals(_.Name));
            newpet.Price = pet.Price;
            return newpet;
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
