using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using PetApiTest.Controller;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet addPet)
        {
            pets.Add(addPet);
            return addPet;
        }

        [HttpPost("addNewPets")]
        public List<Pet> AddNewPets(List<Pet> addPets)
        {
            pets.AddRange(addPets);
            return pets;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpDelete("deleteAllPets")]
        public List<Pet> DeleteAllPets()
        {
            pets.Clear();
            return pets;
        }

        [HttpGet("findPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return pets.Find(pet => pet.Name == name);
        }

        [HttpGet("findPetByType")]
        public List<Pet> FindPetsByType([FromQuery] string type)
        {
            var findResult = pets.FindAll(item => item.Type.Equals(type));
            return findResult;
        }

        [HttpGet("findPetsByPriceRange")]
        public List<Pet> FindPetsByPriceRange([FromQuery] string min, [FromQuery] string max)
        {
            var matchedPets = pets.FindAll(pet => pet.Price >= int.Parse(min) && pet.Price <= int.Parse(max));
            return matchedPets;
        }

        [HttpGet("findPetsByColor")]
        public List<Pet> FindPetsByColor([FromQuery] string color)
        {
            var findResult = pets.FindAll(item => item.Color.Equals(color));
            return findResult;
        }

        [HttpDelete("deletePetByName")]
        public List<Pet> DeletePetByName([FromQuery] string name)
        {
            pets.Remove(pets.Find(pet => pet.Name == name));
            return pets;
        }

        [HttpPatch("modifyPetPrice")]
        public List<Pet> ModifyPetPrice(Pet petNew)
        {
           pets.Find(pet => pet.Name == petNew.Name).Price = petNew.Price;
           return pets;
        }
    }
}
