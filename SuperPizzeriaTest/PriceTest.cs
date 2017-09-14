using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using SuperPizzeria.Data;
using SuperPizzeria.Models;
using SuperPizzeria.Services;
using SuperPizzeria.ViewModels;
using Xunit;

namespace SuperPizzeriaTest
{
    public class PriceTest
    {
        private readonly IServiceProvider _serviceProvider;

        public PriceTest()
        {
            var services = new ServiceCollection();
            services
                .AddTransient<IPriceCheckService, PriceCheckService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void CartItemPriceTest()
        {
            var cartItem = CreateTestCartItem();
            var priceCheckService = new PriceCheckService();
            _serviceProvider.GetService<IPriceCheckService>();
            var result = priceCheckService.CalculateCartItemPrice(cartItem);
            Assert.Equal(8, result);
        }

        private static CartItem CreateTestCartItem()
        {
            var testDishHawaii = new Dish
            {
                Price = 8
            };

            var testHamIngredient = new Ingredient
            {
                Name = "Ham",
                Id = 1,
                Price = 1
            };

            var testCheeseIngredient = new Ingredient
            {
                Name = "Cheese",
                Id = 2,
                Price = 2
            };

            var testPineappleIngredient = new Ingredient
            {
                Name = "Pineapple",
                Id = 3,
                Price = 3
            };

            var testHawaiiCheeseDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testCheeseIngredient

            };

            var testHawaiiHamDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testHamIngredient

            };

            var testHawaiiPineappleDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testPineappleIngredient

            };

            var testDishIngredients = new List<DishIngredient>
            {
                testHawaiiCheeseDishIngredient,
                testHawaiiHamDishIngredient,
                testHawaiiPineappleDishIngredient
            };

            testDishHawaii.DishIngredients = testDishIngredients;

            var testCartItem = new CartItem
            {
                Dish = testDishHawaii,
            };
            return testCartItem;
        }

        private static EditDishViewModel CreateTestEditDishViewModel()
        {
            var testDishHawaii = new Dish
            {
                Price = 8
            };

            var testHamIngredient = new Ingredient
            {
                Name = "Ham",
                Id = 1,
                Price = 1
            };

            var testCheeseIngredient = new Ingredient
            {
                Name = "Cheese",
                Id = 2,
                Price = 2
            };

            var testPineappleIngredient = new Ingredient
            {
                Name = "Pineapple",
                Id = 3,
                Price = 3
            };

            var testHawaiiCheeseDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testCheeseIngredient

            };

            var testHawaiiHamDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testHamIngredient

            };

            var testHawaiiPineappleDishIngredient = new DishIngredient
            {
                Dish = testDishHawaii,
                Ingredient = testPineappleIngredient

            };

            var testDishIngredients = new List<DishIngredient>
            {
                testHawaiiCheeseDishIngredient,
                testHawaiiHamDishIngredient,
                testHawaiiPineappleDishIngredient
            };

            testDishHawaii.DishIngredients = testDishIngredients;

            var editDishViewModel = new EditDishViewModel
            {
                Dish = testDishHawaii,
            };

            return editDishViewModel;
        }
    }
}
