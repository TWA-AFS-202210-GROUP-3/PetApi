using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetControllerTest
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var reseponse = await httpClient.PostAsync("/api/addNewPet", postBody);

            //then
            reseponse.EnsureSuccessStatusCode();
            var reseponseBody = await reseponse.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(reseponseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: GET
             * URI: /api/getAllPets
             * Body

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            //when
            var response = await httpClient.GetAsync("/api/getAllPets");
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
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: GET
             * URI: /api/getPetByName?name=xxx
             * Body

             */
            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            //when
            var response = await httpClient.GetAsync("/api/getPetByName?name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petFound = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal("Kitty", petFound.Name);
        }

        [Fact]
        public async void Should_delete_pet_by_name()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: DELETE
             * URI: /api/delPetByName?name=xxx
             * Body

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            //when
            await httpClient.DeleteAsync("/api/delPetByName?name=Kitty");

            //then
            var allPetsResponse = await httpClient.GetAsync("/api/getAllPets");
            allPetsResponse.EnsureSuccessStatusCode();
            var allPetsBody = await allPetsResponse.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(allPetsBody);
            Assert.Empty(allPets);
        }

        [Fact]
        public async void Should_modify_pet_price()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: PATCH
             * URI: /api/modifyPetPrice
             * Body

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            pet.Price = 200;
            var newSerializeObject = JsonConvert.SerializeObject(pet);
            var newPostBody = new StringContent(newSerializeObject, Encoding.UTF8, "application/json");

            //when
            await httpClient.PatchAsync("/api/modifyPetPrice", newPostBody);

            //then
            var allPetsResponse = await httpClient.GetAsync("/api/getAllPets");
            allPetsResponse.EnsureSuccessStatusCode();
            var allPetsBody = await allPetsResponse.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(allPetsBody);
            Assert.Equal(200, allPets[0].Price);
        }

        [Fact]
        public async void Should_find_pet_by_type()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: GET
             * URI: /api/getPetByType?type=xxx

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            var cat2 = new Pet("Lili", "cat", "gold", 4000);
            await PostNewPet(httpClient, cat2);

            var dog = new Pet("Tom", "dog", "black", 2000);
            await PostNewPet(httpClient, dog);

            //when
            var response = await httpClient.GetAsync("/api/getPetByType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petsOfTypeCat = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, petsOfTypeCat.Count);
        }

        [Fact]
        public async void Should_find_pet_by_price_range()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: GET
             * URI: /api/getPetByPriceRange?lowestPrice=xxx&highestPrice=xxx

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            var cat2 = new Pet("Lili", "cat", "gold", 4000);
            await PostNewPet(httpClient, cat2);

            var dog = new Pet("Tom", "dog", "black", 2000);
            await PostNewPet(httpClient, dog);

            //when
            var response = await httpClient.GetAsync("/api/getPetByPriceRange?lowestPrice=999&highestPrice=3000");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petsInPriceRange = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, petsInPriceRange.Count);
        }

        [Fact]
        public async void Should_find_pet_by_color()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("api/delAllPets");

            /*
             * Method: GET
             * URI: /api/getPetByColor?color=xxx

             */

            var pet = new Pet("Kitty", "cat", "white", 1000);
            await PostNewPet(httpClient, pet);

            var cat2 = new Pet("Lili", "cat", "gold", 4000);
            await PostNewPet(httpClient, cat2);

            var dog = new Pet("Tom", "dog", "black", 2000);
            await PostNewPet(httpClient, dog);

            //when
            var response = await httpClient.GetAsync("/api/getPetByColor?color=gold");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petsColorGold = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Single(petsColorGold);
        }

        private static async Task PostNewPet(HttpClient httpClient, Pet pet)
        {
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
        }
    }
}
