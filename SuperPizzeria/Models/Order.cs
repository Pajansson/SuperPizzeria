using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPizzeria.Models
{
    public class Order
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public int OrderId { get; set; }
    }
}
