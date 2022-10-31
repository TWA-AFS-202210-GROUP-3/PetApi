using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetController
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: api/addNewPet
             * Body:
             * {
             *   "name": "bob",
             *    "type":"cat",
             *   "color":"black",
             * "price": 2000,
             */
            var pet = new Pet(name: "bob", type: "cat", color: "black", price: 2000);
            var serializedObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            //Console.WriteLine(postBody);
            var response = await httpClient.PostAsync("api/addNewPet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
            //Assert.Equal(" ", responseBody);
        }

        [Fact]
        public async void Should_get_all_pets_to_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "bob", type: "cat", color: "black", price: 2000);
            var serializedObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("api/getAllPets");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async void Should_find_pet_by_name()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "bob", type: "cat", color: "black", price: 2000);
            var serializedObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("api/findPetByName?name=bob");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, actualPets);
        }

        [Fact]
        public async void Should_delete_pet_when_sold_out()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "bob", type: "cat", color: "black", price: 2000);
            var serializedObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.DeleteAsync("api/deleteSoldPet?name=bob");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Empty(actualPets);
        }

        [Fact]
        public async void Should_find_all_pets_have_the_same_type()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet("bob", "cat", "black", 2000),
                new Pet("Marry", "cat", "blue", 5000)
            };
            foreach (var pet in pets)
            {
                var serializedObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("api/addNewPet", postBody);
            }

            //when
            var response = await httpClient.GetAsync("api/findPetsByType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, actualPets.Count);
        }

        [Fact]
        public async void Should_find_all_pets_have_the_same_color()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet("bob", "cat", "black", 2000),
                new Pet("Marry", "cat", "blue", 5000),
                new Pet("sam", "dog", "blue", 1000),
            };
            foreach (var pet in pets)
            {
                var serializedObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("api/addNewPet", postBody);
            }

            //when
            var response = await httpClient.GetAsync("api/findPetsByColor?color=blue");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, actualPets.Count);
        }

        [Fact]
        public async void Should_find_all_pets_have_the_same_price_range()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pets = new List<Pet>
            {
                new Pet("bob", "cat", "black", 2000),
                new Pet("Marry", "cat", "blue", 5000),
                new Pet("sam", "dog", "blue", 1000),
                new Pet("Lily", "pig", "white", 3000),
            };
            foreach (var pet in pets)
            {
                var serializedObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("api/addNewPet", postBody);
            }

            //when
            var response = await httpClient.GetAsync("api/findPetsByPriceRange?min=2000&max=5000");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(3, actualPets.Count);
        }

        [Fact]
        public async void Should_get_one_pet_that_is_changed_price()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            var pet = new Pet(name: "bob", type: "cat", color: "black", price: 2000);
            var serializedObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            pet.Price = 1500;
            var serializedObjectNew = JsonConvert.SerializeObject(pet);
            var postBodyNew = new StringContent(serializedObjectNew, Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync("api/PetPriceModification", postBodyNew);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var priceModifiedPets = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(1500, priceModifiedPets.Price);
        }
    }
}
