using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetController
    {
        [Fact]
        public async Task Should_add_new_pet_to_system_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            //Method: POST
            //URI: /Api/addNewPet
            //body:
            //{
            //    "name": "Kitty",
            //    "type": "cat",
            //    "color": "white",
            //    "price": 1000
            //}

            var pet = new Pet(name: "Kittysss", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/addNewPet", postBody);

            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<Pet>(reponseBody);
            Assert.Equal(pet, savesPet);
        }

        [Fact]
        public async Task Should_get_all_pet_from_system_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("/api/getallPet");
            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<List<Pet>>(reponseBody);
            Assert.Contains(pet, savesPet);
        }
    }
}
