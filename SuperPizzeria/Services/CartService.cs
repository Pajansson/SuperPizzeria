using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SuperPizzeria.Data;
using SuperPizzeria.Models;
using SuperPizzeria.ViewModels;

namespace SuperPizzeria.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> CreateCartItem(EditDishViewModel editDishViewModel)
        {
            var dbIngredients = _context.Ingredients.ToList();
            var dbdish = _context.Dishes.Include(i => i.DishIngredients).ThenInclude(di => di.Dish)
                .FirstOrDefault(x => x.Id == editDishViewModel.Dish.Id);
            var cartItem = new CartItem
            {
                Dish = dbdish,
                DishId = dbdish.Id,
                Quantity = editDishViewModel.Quantity,
                Price = dbdish.Price
            };
            foreach (var dishIngredient in cartItem.Dish.DishIngredients)
            {
                dishIngredient.Ingredient.Price = 0;
            }

            foreach (var cartItemIngredientId in editDishViewModel.IngredientId)
            {
                var cartItemIngredient =
                    new CartItemIngredient
                    {
                        Ingredient = dbIngredients.Find(i => i.Id == cartItemIngredientId),
                        CartItem = cartItem,
                        IngredientId = cartItemIngredientId
                    };
                cartItem.Price += cartItemIngredient.Ingredient.Price;
                cartItem.CartItemIngredients.Add(cartItemIngredient);
            }

            return cartItem;
        }


    }
}