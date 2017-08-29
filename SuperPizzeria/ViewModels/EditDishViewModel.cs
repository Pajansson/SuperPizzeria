using System.Collections.Generic;
using SuperPizzeria.Models;

namespace SuperPizzeria.ViewModels
{
    public class EditDishViewModel
    {
        public List<int> ingredientId { get; set; }
        public Dish Dish { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int Quantity { get; set; }
    }
}
