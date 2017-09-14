using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperPizzeria.Data;
using SuperPizzeria.Services;
using SuperPizzeria.ViewModels;

namespace SuperPizzeria.Controllers
{

    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartItemToCart(EditDishViewModel editDishViewModel)
        {
            var cartItem = _cartService.CreateCartItem(editDishViewModel);

            var currentCart = _cartService.GetCurrentCart(HttpContext);
           
            currentCart.CartItems.Add(cartItem);
            _cartService.SetCurrentCart(currentCart, HttpContext);

            return PartialView("_CartPartial", currentCart);
        }

        public IActionResult CustomizeDish(int id)
        {
            var customizeDishViewModel = _cartService.CustomizeDish(id);

            return PartialView("_CartItemPartial", customizeDishViewModel);
        }

        //// GET: Cart/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        // GET: Cart/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        public IActionResult RemoveCartItem(Guid id)
        {
            var currentCart = _cartService.GetCurrentCart(HttpContext);
            var cartItem = _cartService.GetCartItem(id, currentCart);
            currentCart.CartItems.Remove(cartItem);
            _cartService.SetCurrentCart(currentCart, HttpContext);

            return PartialView("_CartPartial", currentCart);
        }
    }
}