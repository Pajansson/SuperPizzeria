using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperPizzeria.Models;

namespace SuperPizzeria.Services
{
    public interface IPriceCheckService
    {
        int CalculateCartItemPrice(CartItem cartItem);

        int CalulateCartPrice(Cart cart);
    }
}
