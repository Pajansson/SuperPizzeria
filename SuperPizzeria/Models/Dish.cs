using System.Collections.Generic;

namespace SuperPizzeria.Models
{
    public class Dish
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
