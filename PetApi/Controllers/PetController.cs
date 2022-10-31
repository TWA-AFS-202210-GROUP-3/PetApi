using Microsoft.AspNetCore.Mvc;
using PetApiTest.Controller;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            return pet;
        }
    }
}
