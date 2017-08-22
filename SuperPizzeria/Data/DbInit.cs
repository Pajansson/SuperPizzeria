using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SuperPizzeria.Models;

namespace SuperPizzeria.Data
{
    public class DbInit
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "Student@test.com";
            aUser.Email = "Student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$word1").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingredient { Name = "Cheese" };
                var tomato = new Ingredient { Name = "Tomato" };
                var ham = new Ingredient { Name = "Ham" };

                var capriciosa = new Dish { Name = "Capricoiusa", Price = 79 };
                var margarita = new Dish { Name = "Margarita", Price = 69 };
                var hawaii = new Dish { Name = "Hawaii", Price = 85 };

                var capriciosaCheese = new DishIngredients { Dish = capriciosa, Ingredient = cheese };
                var capriciosaTomato = new DishIngredients { Dish = capriciosa, Ingredient = tomato };
                var capriciosaHam = new DishIngredients { Dish = capriciosa, Ingredient = ham };

                capriciosa.DishIngredients = new List<DishIngredients>();
                capriciosa.DishIngredients.Add(capriciosaCheese);
                capriciosa.DishIngredients.Add(capriciosaHam);
                capriciosa.DishIngredients.Add(capriciosaTomato);

                context.AddRange(ham, tomato, cheese);
                context.AddRange(capriciosa, margarita, hawaii);
                context.SaveChanges();
            }
        }
    }
}
