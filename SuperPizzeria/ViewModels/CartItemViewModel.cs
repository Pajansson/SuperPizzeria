using System.Collections.Generic;
using SuperPizzeria.Models;

namespace SuperPizzeria.ViewModels
{
    public class CartItemViewModel
    {
        public CartItem CartItem { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
