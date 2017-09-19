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
        private readonly IPriceCheckService _priceCheckService;

        public CartService(ApplicationDbContext context, IPriceCheckService priceCheckService)
        {
            _context = context;
            _priceCheckService = priceCheckService;
        }

        public CartItem CreateCartItem(EditDishViewModel editDishViewModel)
        {
            var dbIngredients = _context.Ingredients.ToList();
            var dbdish = _context.Dishes.Include(i => i.DishIngredients)
                .FirstOrDefault(x => x.Id == editDishViewModel.Dish.Id);
            var cartItem = new CartItem
            {
                Dish = dbdish,
                //DishId = dbdish.Id,
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
                        //CartItemIngredientId = Guid.NewGuid(),//dubblett?
                        Ingredient = dbIngredients.Find(i => i.Id == cartItemIngredientId),
                        CartItem = cartItem,
                        CartItemId = cartItem.CartItemId
                        //IngredientId = cartItemIngredientId
                    };
                cartItem.CartItemIngredients.Add(cartItemIngredient);
            }
            cartItem.Price = _priceCheckService.CalculateCartItemPrice(cartItem);
            return cartItem;
        }

        public Cart GetCurrentCart(HttpContext context)
        {
            Cart cart = null;

            if (context.Session.GetString("Cart") != null)
            {
                var temp = context.Session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            return cart ?? new Cart();
        }

        public void SetCurrentCart(Cart cart, HttpContext context)
        {
            var serializedValue = JsonConvert.SerializeObject(cart,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            context.Session.SetString("Cart", serializedValue);
        }

        public EditDishViewModel CustomizeDish(int id)
        {
            var dbdish = _context.Dishes.FirstOrDefault(x => x.Id == id);
            dbdish.DishIngredients = _context.DishIngredients.Where(x => x.DishId == dbdish.Id).ToList();

            var customizeDishViewModel = new EditDishViewModel
            {
                Dish = dbdish,
                IngredientId = new List<int>(),
                Ingredients = _context.Ingredients.ToList()
            };
            foreach (var dishIngredient in dbdish.DishIngredients)
            {
                customizeDishViewModel.IngredientId.Add(dishIngredient.IngredientId);
                dishIngredient.Ingredient.Price = 0;
            }
            return customizeDishViewModel;
        }

        public CartItem GetCartItem(Guid id, Cart currentCart)
        {
            return currentCart.CartItems.FirstOrDefault(ci => ci.CartItemId == id);
        }
    }
}