using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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
    }
}
