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

        [HttpGet("findPetbyName")]
        public Pet FindPetbyName([FromQuery] string name)
        {
            foreach (Pet pet in pets)
            {
                if (pet.Name == name)
                {
                    return pet;
                }
            }

            return null;
        }

        [HttpDelete("deletePetbyName")]
        public List<Pet> DeletePetbyName([FromQuery] string name)
        {
            foreach (Pet pet in pets)
            {
                if (pet.Name.Equals(name))
                {
                    pets.Remove(pet);
                    break;
                }
            }

            return pets;
        }

        [HttpDelete("deleteAllPets")]
        public List<Pet> Geleteallpets()
        {
            pets.Clear();
            return pets;
        }

        [HttpPatch("modifyPetbyName")]
        public Pet ModifyPetbyName(Pet petmodify)
        {
            foreach (Pet pet in pets)
            {
                if (pet.Name.Equals(pet.Name))
                {
                    pet.Price = petmodify.Price;
                }
            }

            return petmodify;
        }

        [HttpGet("findPetbyType")]
        public List<Pet> FindPetbyType([FromQuery] string type)
        {
            return pets.FindAll(x => x.Type == type);
        }

        [HttpGet("findPetbyPriceRange")]
        public List<Pet> FindPetbyPriceRange([FromQuery] int MaxPrice, int Minprice)
        {
            return pets.FindAll(x => x.Price >= Minprice && x.Price <= MaxPrice);
        }
    }
}
