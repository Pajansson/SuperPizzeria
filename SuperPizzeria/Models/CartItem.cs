using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace SuperPizzeria.Models
{
    public class CartItem
    {
        public CartItem()
        {
            CartItemId = Guid.NewGuid();
            CartItemIngredients = new List<CartItemIngredient>();
            Quantity = 1;
        }
        
        public Guid CartItemId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public Dish Dish { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
