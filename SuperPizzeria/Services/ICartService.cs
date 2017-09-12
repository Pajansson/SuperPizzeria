using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SuperPizzeria.Models;
using SuperPizzeria.ViewModels;

namespace SuperPizzeria.Services
{
    public interface ICartService
    {
        Task<CartItem> CreateCartItem(EditDishViewModel editDishViewModel);
        Cart GetCurrentCart(HttpContext context);
        void SetCurrentCart(Cart cart, HttpContext context);
        EditDishViewModel CustomizeDish(int id);
        CartItem GetCartItem(Guid id, Cart currentCart);
    }
}
