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
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var dishes = new List<Dish>();
            dishes = await _context.Dishes.ToListAsync();
            foreach (var dish in dishes)
            {
                await Details(dish.Id);
            }
            return View(dishes);
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,Price")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ingredientsList = await _context.Ingredients.ToListAsync();
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            var editDish = new EditDishViewModel();
            editDish.Dish = dish;
            editDish.Ingredients = ingredientsList;
            if (dish == null)
            {
                return NotFound();
            }
            return View(editDish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,Price")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }

        public IActionResult AddToCart(int id)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.Id == id);

            Cart cart;

            if (HttpContext.Session.GetString("AddedDishes") == null)
            {
                cart = new Cart() { CartItems = new List<CartItem>() };
            }
            else
            {
                var temp = HttpContext.Session.GetString("AddedDishes");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            CartItem cartItem = new CartItem
            {
                Dish = dish,
                Quantity = 1,
                DishId = dish.Id
            };

            if (cart.CartItems.Any(x => x.DishId == dish.Id))
            {
                cart.CartItems.First(x => x.DishId == dish.Id).Quantity++;
            }
            else
            {
                cart.CartItems.Add(cartItem);
            }

            var serializedValue = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("AddedDishes", serializedValue);

            return PartialView("_CartPartial", cart.CartItems);
        }
    }
}
