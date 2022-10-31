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
        public Pet FindPetByName([FromQuery] string Name)
        {
            return pets.Find(pet => pet.Name == Name);
        }

        [HttpDelete("deletePetByName")]
        public List<Pet> DeletePetByName([FromQuery] string Name)
        {
            pets.Remove(pets.Find(pet => pet.Name == Name));
            return pets;
        }

        [HttpPatch("modifyPetPrice")]
        public List<Pet> ModifyPetPrice(Pet petNew)
        {
           pets.Find(pet => pet.Name == petNew.Name).Price = petNew.Price;
           return pets;
        }

        //[HttpPatch("modifyPetPrice")]
        //public List<Pet> ModifyPetPrice([FromQuery] string Name, int price)
        //{
        //    pets.Find(pet => pet.Name == Name).Price = price;
        //    return pets;
        //}
    }
}
