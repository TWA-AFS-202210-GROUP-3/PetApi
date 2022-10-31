using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;
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
        public async void Should_add_new_pets_to_system_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "dog", color: "white", price: 500),
                new Pet(name: "Amy", type: "cat", color: "white", price: 2000),
                new Pet(name: "Bob", type: "cat", color: "black", price: 1000)
            };
            var serializeObject = JsonConvert.SerializeObject(pets);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            // when
            var response = await httpclient.PostAsync("/api/addNewPets", postBody);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pets, savedPets);
        }

        [Fact]
        public async void Should_get_all_pets_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpclient.GetAsync("/api/getAllPets");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async void Should_find_the_pet_by_name_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpclient.GetAsync("/api/findPetByName?Name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var findPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet.Name, findPet.Name);
        }

        [Fact]
        public async void Should_delete_the_pet_when_it_was_bought_by_customer_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "dog", color: "white", price: 500),
                new Pet(name: "Amy", type: "cat", color: "white", price: 2000),
                new Pet(name: "Bob", type: "cat", color: "black", price: 1000)
            };
            var serializeObject = JsonConvert.SerializeObject(pets);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //when
            var response = await httpclient.DeleteAsync("/api/deletePetByName?Name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var restPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.True(restPets.Find(pet => pet.Name == "Kitty") == null);
        }

        [Fact]
        public async void Should_modifiy_the_price_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody);
            pet.Price = 500;
            var serializeObjectNew = JsonConvert.SerializeObject(pet);
            var patchBody = new StringContent(serializeObjectNew, Encoding.UTF8, "application/json");
            //when
            var response = await httpclient.PatchAsync("/api/modifyPetPrice", patchBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petsNew = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            var priceNew = petsNew.Find(pet => pet.Name == "Kitty").Price;
            Assert.Equal(500, priceNew);
        }

        [Fact]
        public async void Should_find_pets_by_type_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "dog", color: "white", price: 500),
                new Pet(name: "Amy", type: "dog", color: "white", price: 2000),
                new Pet(name: "Bob", type: "cat", color: "black", price: 1000)
            };
            var serializeObject = JsonConvert.SerializeObject(pets);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPets", postBody);
            //when
            var response = await httpclient.GetAsync("/api/findPetsByType?type=dog");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var matchedpPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, matchedpPets.Count);
        }

        [Fact]
        public async void Should_find_pets_by_price_range_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "dog", color: "white", price: 510),
                new Pet(name: "Amy", type: "dog", color: "white", price: 2000),
                new Pet(name: "Bob", type: "cat", color: "black", price: 900)
            };
            var serializeObject = JsonConvert.SerializeObject(pets);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPets", postBody);
            //when
            var response = await httpclient.GetAsync("/api/findPetsByPriceRange?min=500&max=1000");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var matchedpPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, matchedpPets.Count);
        }

        [Fact]
        public async void Should_find_pets_by_color_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet(name: "Kitty", type: "dog", color: "white", price: 510),
                new Pet(name: "Amy", type: "dog", color: "white", price: 2000),
                new Pet(name: "Bob", type: "cat", color: "black", price: 900)
            };
            var serializeObject = JsonConvert.SerializeObject(pets);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPets", postBody);
            //when
            var response = await httpclient.GetAsync("/api/findPetsByColor?color=white");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var matchedpPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, matchedpPets.Count);
        }
    }
}
