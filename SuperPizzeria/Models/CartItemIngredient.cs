namespace SuperPizzeria.Models
{
    public class CartItemIngredient
    {
        //public CartItemIngredient(Ingredient ingredient)
        //{
        //    Ingredient = ingredient;
        //}
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public bool Enabled { get; set; }
    }
}
