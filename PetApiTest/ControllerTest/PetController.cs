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
            //givn
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
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
        public async void Should_get_all_pets_to_system_successfully()
        {
            //givn
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
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
        public async void Should_find_pet_by_name_successfully()
        {
            //givn
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
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
            await httpClient.PostAsync("api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("api/findPetsByName?name=bob");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var pet1 = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal("bob", pet1.Name);
        }
    }
}
