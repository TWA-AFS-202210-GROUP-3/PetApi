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

        [Fact]
        public async Task Should_find_pet_from_system_by_name_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("/api/findPetbyName?Name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<Pet>(reponseBody);
            Assert.Equal(pet.Name, savesPet.Name);
        }

        [Fact]
        public async Task Should_delete_pet_from_system_by_name_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.DeleteAsync("/api/deletePetbyName?name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<List<Pet>>(reponseBody);
            Assert.DoesNotContain(pet, savesPet);
        }

        [Fact]
        public async Task Should_modify_pet_price_from_system_by_name_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            pet.Price = 500;
            var serializeObjectmodify = JsonConvert.SerializeObject(pet);
            var postBodymodify = new StringContent(serializeObjectmodify, Encoding.UTF8, "application/json");

            await httpClient.GetAsync("/api/findPetbyName?Name=Kitty");
            var response = await httpClient.PatchAsync("/api/modifyPetbyName", postBodymodify);
            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<Pet>(reponseBody);
            Assert.Equal(500, pet.Price);
        }

        [Fact]
        public async Task Should_find_pets_from_system_by_type_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
                new Pet(name: "Mickey", type: "mouse", color: "white", price: 500),
            };
            foreach (Pet pet in pets)
            {
                var serialzeObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("/api/addNewPet", postBody);
            }
            //when
            var response = await httpClient.GetAsync("/api/findPetbyType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var reponseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<List<Pet>>(reponseBody);
            Assert.Equal(1, savesPet.Count);
        }

        [Fact]
        public async Task Should_find_pets_from_system_by_price_range_successfulAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pets = new List<Pet> 
            { new Pet("kitty", "cat", "white", 1000),
                new Pet("Bob", "cat", "white", 1500),
                new Pet("Ops", "cat", "white", 2000),
            };
            foreach (Pet pet in pets)
            {
                var serialzeObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("/api/addNewPet", postBody);
            }

            //when
            var response = await httpClient.GetAsync("/api/findPetbyPriceRange?Minprice=500&&MaxPrice=1700");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savesPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, savesPet.Count);
        }
    }
}
