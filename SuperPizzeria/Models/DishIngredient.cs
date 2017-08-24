namespace SuperPizzeria.Models
{
    public class DishIngredient
    {
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        public Dish Dish { get; set; }
        public int DishId { get; set; }
    }
}
