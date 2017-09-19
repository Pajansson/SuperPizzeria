using System;
using System.ComponentModel.DataAnnotations;

namespace SuperPizzeria.Models
{
    public class CartItemIngredient
    {
        public CartItemIngredient()
        {
            CartItemIngredientId = Guid.NewGuid();
        }
        [Key]
        public Guid CartItemIngredientId { get; set; }
        public Guid CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public bool Enabled { get; set; }
    }
}
