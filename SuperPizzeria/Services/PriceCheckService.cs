using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperPizzeria.Data;
using SuperPizzeria.Models;

namespace SuperPizzeria.Services
{
    public class PriceCheckService : IPriceCheckService
    {
        public int CalculateCartItemPrice(CartItem cartItem)
        {
            var cartItemPrice = cartItem.Dish.Price;

            foreach (var dishIngredient in cartItem.Dish.DishIngredients)
            {
                dishIngredient.Ingredient.Price = 0;
            }

            foreach (var cII in cartItem.CartItemIngredients)
            {
                var cartItemIngredient =
                    new CartItemIngredient
                    {
                        Ingredient = cII.Ingredient,
                        CartItem = cartItem,
                    };
                cartItemPrice += cartItemIngredient.Ingredient.Price;
            }

            return cartItemPrice;
        }

        public int CalulateCartPrice(Cart cart)
        {
            foreach (var cartItem in cart.CartItems)
            {
                cartItem.Price = CalculateCartItemPrice(cartItem);
            }
            return cart.CartItems.Sum(cartItem => cartItem.Price);
        }
    }
}
