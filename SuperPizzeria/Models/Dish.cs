using System.Collections.Generic;
using System.Linq;

namespace SuperPizzeria.Models
{
    public class Dish
    {
        public Dish()
        {
            DishIngredients = new List<DishIngredient>();
        }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
