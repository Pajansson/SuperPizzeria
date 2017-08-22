using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class Dish
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public List<DishIngredients> DishIngredients { get; set; }
    }
}
