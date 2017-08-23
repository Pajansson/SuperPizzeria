using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperPizzeria.Models;

namespace SuperPizzeria.ViewModels
{
    public class EditDishViewModel
    {
        public Dish Dish { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
