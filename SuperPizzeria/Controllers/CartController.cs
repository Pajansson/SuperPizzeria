using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SuperPizzeria.Data;
using SuperPizzeria.Models;
using SuperPizzeria.ViewModels; 

namespace SuperPizzeria.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carts.ToListAsync());
        }

        [HttpPost]
        public IActionResult AddToCartItemToCart(EditDishViewModel editDishViewModel)
        {
            var dbIngredients = _context.Ingredients.ToList();
            var dbdish = _context.Dishes.Include(i => i.DishIngredients).ThenInclude(di => di.Dish).FirstOrDefault(x => x.Id == editDishViewModel.Dish.Id);
            var cartItem = new CartItem {Dish = dbdish, DishId = dbdish.Id, Quantity = editDishViewModel.Quantity};

            foreach (var dishIngredientId in editDishViewModel.ingredientId)
            {
                var cartItemIngredient =
                    new CartItemIngredient {Ingredient = dbIngredients.Find(i => i.Id == dishIngredientId), CartItem = cartItem, IngredientId = dishIngredientId};
                cartItem.CartItemIngredients.Add(cartItemIngredient);
            }
            
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
                ingredientId = new List<int>(),
                Ingredients = _context.Ingredients.ToList()
            };
            foreach (var dishIngredient in dbdish.DishIngredients)
            {
                customizeDishViewModel.ingredientId.Add(dishIngredient.IngredientId);
            }

            return PartialView("_CartItemPartial", customizeDishViewModel);
        }

        public void SetCurrentCart(Cart cart)
        {
            var serializedValue = JsonConvert.SerializeObject(cart, new JsonSerializerSettings{ ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            HttpContext.Session.SetString("Cart", serializedValue);
        }

        public Cart GetCurrentCart()
        {
            Cart cart = null;

            if (HttpContext.Session.GetString("Cart") != null)
            {
                var temp = HttpContext.Session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            return cart??new Cart();
        }//todo lyft ut till service

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.SingleOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

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

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}