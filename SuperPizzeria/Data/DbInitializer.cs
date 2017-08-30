using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SuperPizzeria.Models;

namespace SuperPizzeria.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var vegetarian = new Category {Name = "Vegetarian"};
                var notVegetarian = new Category { Name = "Not Vegetarian" };
                var cheese = new Ingredient { Name = "Cheese" };
                var pineapple = new Ingredient { Name = "Pineapple" };
                var kebab = new Ingredient { Name = "Kebab" };
                var banana = new Ingredient { Name = "Banan" };
                var tomatoe = new Ingredient { Name = "Tomatoe" };
                var ham = new Ingredient { Name = "Ham" };
                var capricciosa = new Dish { Name = "Capricciosa", Price = 7, Category = notVegetarian};
                var margaritha = new Dish { Name = "Margaritha", Price = 6, Category = vegetarian };
                var hawaii = new Dish { Name = "Hawaii", Price = 8, Category = notVegetarian };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomatoe = new DishIngredient { Dish = capricciosa, Ingredient = tomatoe };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };

                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };
                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                
                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                var margarithaTomatoe = new DishIngredient { Dish = margaritha, Ingredient = tomatoe };

                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaTomatoe);
                capricciosa.DishIngredients.Add(capricciosaCheese);
                capricciosa.DishIngredients.Add(capricciosaHam);

                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiHam);
                hawaii.DishIngredients.Add(hawaiiCheese);
                hawaii.DishIngredients.Add(hawaiiPineapple);

                margaritha.DishIngredients = new List<DishIngredient>();
                margaritha.DishIngredients.Add(margarithaTomatoe);
                margaritha.DishIngredients.Add(margarithaCheese);

                context.Ingredients.Add(pineapple);
                context.Ingredients.Add(kebab);
                context.Ingredients.Add(banana);
                context.Dishes.Add(capricciosa);
                context.Dishes.Add(margaritha);
                context.Dishes.Add(hawaii);
                context.SaveChanges();
            }
        }
    }
}
