using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperPizzeria.Models;
using SuperPizzeria.ViewModels;

namespace SuperPizzeria.Services
{
    public interface ICartService
    {
        Task<CartItem> CreateCartItem(EditDishViewModel editDishViewModel);
    }
}
