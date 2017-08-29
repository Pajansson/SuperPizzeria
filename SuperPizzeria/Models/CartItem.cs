using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class CartItem
    {
        public CartItem()
        {
            CartItemIngredients = new List<CartItemIngredient>();
            Quantity = 1;
        }

        public int Id { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public Dish Dish { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
        //public Guid IdGenerator { get { return Guid.NewGuid(); } }
    }
}
