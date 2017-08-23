using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class Order
    {
        public Cart Cart { get; set; }
        public int Id { get; set; }
    }
}
