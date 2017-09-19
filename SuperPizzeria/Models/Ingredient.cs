using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class Ingredient
    {
        public string Name { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public int Price { get; set; }
    }
}
