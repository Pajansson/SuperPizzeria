using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SuperPizzeria.Data;
using SuperPizzeria.Models;
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

        // GET: Cart
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Carts.ToListAsync());
        //}

        [HttpPost]
        public async Task<IActionResult> AddToCartItemToCart(EditDishViewModel editDishViewModel)
        {
            var cartItem = await _cartService.CreateCartItem(editDishViewModel);

            var currentCart = GetCurrentCart();
           
            currentCart.CartItems.Add(cartItem);
            SetCurrentCart(currentCart);

            return PartialView("_CartPartial", currentCart);
        }

        public IActionResult CustomizeDish(int id)
        {
            var dbdish = _context.Dishes.FirstOrDefault(x => x.Id == id);
            dbdish.DishIngredients = _context.DishIngredients.Where(x => x.DishId == dbdish.Id).ToList();

            var customizeDishViewModel = new EditDishViewModel
            {
                Dish = dbdish,
                IngredientId = new List<int>(),
                Ingredients = _context.Ingredients.ToList()
            };
            foreach (var dishIngredient in dbdish.DishIngredients)
            {
                customizeDishViewModel.IngredientId.Add(dishIngredient.IngredientId);
                dishIngredient.Ingredient.Price = 0;
            }

            return PartialView("_CartItemPartial", customizeDishViewModel);
        }

        public void SetCurrentCart(Cart cart)
        {
            var serializedValue = JsonConvert.SerializeObject(cart,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            HttpContext.Session.SetString("Cart", serializedValue);
        }//todo lyft ut till service

        public Cart GetCurrentCart()
        {
            Cart cart = null;

            if (HttpContext.Session.GetString("Cart") != null)
            {
                var temp = HttpContext.Session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            return cart ?? new Cart();
        } //todo lyft ut till service

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Cart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ApplicationUserId")] Cart cart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(cart);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        // GET: Cart/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts.SingleOrDefaultAsync(m => m.Id == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(cart);
        //}
        
        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId")] Cart cart)
        //{
        //    if (id != cart.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(cart);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CartExists(cart.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        public IActionResult RemoveCartItem(Guid id)
        {
            var currentCart = GetCurrentCart();
            var cartItem = currentCart.CartItems.FirstOrDefault(ci => ci.CartItemId == id);
            currentCart.CartItems.Remove(cartItem);
            SetCurrentCart(currentCart);

            return PartialView("_CartPartial", currentCart);
        }

        // POST: Cart/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var cart = await _context.Carts.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Carts.Remove(cart);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        
        //public IActionResult CheckOut(Cart cart)
        //{
        //    if (SignInManager.IsSignedIn(User))
        //    {
                
        //    }
        //}

        //private bool CartExists(int id)
        //{
        //    return _context.Carts.Any(e => e.Id == id);
        //}
    }
}