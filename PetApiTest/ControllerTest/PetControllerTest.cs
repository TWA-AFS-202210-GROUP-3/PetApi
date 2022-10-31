using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApiTest.Controller;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetController
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            /*
             *Method:post
             *URI: /api/addNewPet
             *Body:
             * {
             *"name":"kitty",
             *"type":"cat",
             *"color":"white",
             *"price":1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync("/api/addNewPet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            /*
             *Method:post
             *URI: /api/addNewPet
             *Body:
             * {
             *"name":"kitty",
             *"type":"cat",
             *"color":"white",
             *"price":1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpclient.GetAsync("/api/getAllPet");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }
    }
}
