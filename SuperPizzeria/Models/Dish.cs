using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
