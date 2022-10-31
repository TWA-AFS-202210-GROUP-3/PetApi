using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : Controller
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getallPet")]
        public List<Pet> Getallpets()
        {
            return pets;
        }

        //[Http("findPetbyName")]
        //public Pet FindPetbyName(string name)
        //{
        //    return pets;
        //}

        [HttpDelete("eleteAllPets")]
        public List<Pet> Geleteallpets()
        {
            pets.Clear();
            return pets;
        }
    }
}
