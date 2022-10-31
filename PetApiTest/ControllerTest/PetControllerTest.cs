using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetControllerTest
    {
        [Fact]
        public async void Should_add_newPet_to_system_successfully()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
                Method: POST
                URI: /api/addNewPet
                Body: {
                       "name": "Kitty",
                       "type": "cat",
                       "color": "white",
                       "price": 1000
                       }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/addNewPet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(resonseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets_from_system_successfully()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            var response = await httpClient.GetAsync("/api/getAllPets");
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(resonseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async void Should_get_one_pet_when_get_pet_by_name()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            var response = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var matchedPet = JsonConvert.DeserializeObject<Pet>(resonseBody);
            Assert.Equal(pet, matchedPet);
        }

        [Fact]
        public async void Should_delete_one_pet_when_one_pet_was_purchased()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            var response = await httpClient.PatchAsync("/api/removeOnePet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var matchedPets = JsonConvert.DeserializeObject<List<Pet>>(resonseBody);
            Assert.Equal(new List<Pet>(), matchedPets);
        }

        [Fact]
        public async void Should_return_one_pet_when_change_one_pet_price()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            pet.Price = 200;
            var newSerializeObject = JsonConvert.SerializeObject(pet);
            var newPostBody = new StringContent(newSerializeObject, Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync("/api/editPetPrice", newPostBody);
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var matchedPet = JsonConvert.DeserializeObject<Pet>(resonseBody);
            Assert.Equal(pet, matchedPet);
        }

        [Fact]
        public async void Should_get_pets_when_get_pets_by_type()
        {
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");

            var pets = new List<Pet>
            {
                    new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
                    new Pet(name: "Kite", type: "cat", color: "white", price: 123),
                    new Pet(name: "Obama", type: "cat", color: "white", price: 2333),
            };
            pets.ForEach(async pet =>
            {
                var serializeObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("/api/addNewPet", postBody);
            });

            var response = await httpClient.GetAsync("/api/getPetsByType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var resonseBody = await response.Content.ReadAsStringAsync();
            var matchedPets = JsonConvert.DeserializeObject<List<Pet>>(resonseBody);
            Assert.Equal(pets.Count, matchedPets.Count);
        }
    }
}

