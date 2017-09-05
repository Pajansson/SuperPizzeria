using System.Collections.Generic;
using SuperPizzeria.Models;

namespace SuperPizzeria.ViewModels
{
    public class DishCartViewModel
    {
        public List<Dish> Dishes { get; set; }
        public Cart Cart { get; set; }
    }
}
