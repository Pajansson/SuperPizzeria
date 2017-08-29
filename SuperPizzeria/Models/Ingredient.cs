﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class Ingredient
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        //public bool Included { get; set; }
    }
}
