using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperPizzeria.Models;

namespace SuperPizzeria.ViewModels
{
    public class DishCartViewModel
    {
        public List<Dish> Dishes { get; set; }
        public Cart Cart { get; set; }
    }
}
