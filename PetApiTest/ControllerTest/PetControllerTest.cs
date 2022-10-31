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

        [Fact]
        public async void Should_find_the_pet_by_name_successfully()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpclient = application.CreateClient();
            await httpclient.DeleteAsync("/api/deleteAllPets");
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
            var pet1 = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject1 = JsonConvert.SerializeObject(pet1);
            var postBody1 = new StringContent(serializeObject1, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody1);
            var pet2 = new Pet(name: "Amy", type: "bog", color: "white", price: 1000);
            var serializeObject2 = JsonConvert.SerializeObject(pet2);
            var postBody2 = new StringContent(serializeObject2, Encoding.UTF8, "application/json");
            await httpclient.PostAsync("/api/addNewPet", postBody2);
            //when
            var response = await httpclient.DeleteAsync("/api/deletePetByName?Name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var pets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.True(pets.Find(pet => pet.Name == "Kitty") == null);
        }

        /*
 *AC3: I can find pet by its name. findPetByName  / get

AC4: When a pet was bought by customer, I can get it off the shelf. / delete

AC5: I can modify the price of a pet. / put

AC6: I can find pets by type. / 

AC7: I can find pets by price range.

AC8: I can find pets by color.
 *
 *
 */
    }
}
