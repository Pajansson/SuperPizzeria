using System.Collections.Generic;

namespace SuperPizzeria.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }
        public int Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
