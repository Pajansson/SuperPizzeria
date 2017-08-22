using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class DishIngredients
    {
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        public Dish Dish { get; set; }
        public int DishId { get; set; }
    }
}
