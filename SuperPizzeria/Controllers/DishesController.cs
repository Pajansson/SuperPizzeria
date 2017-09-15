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
            var dishes = await _context.Dishes.ToListAsync();
            foreach (var dish in dishes)
            {
                await Details(dish.Id);
            }
            var dishVm = new DishCartViewModel
            {
                Dishes = dishes,
                Cart = GetCurrentCart(),
            };
            return View(dishVm);
        }

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

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(c => c.Category)
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
            var createDishVm = new EditDishViewModel
            {
                Categories = _context.Categories.ToList(),
                Ingredients = _context.Ingredients.ToList(),
            };

            return View(createDishVm);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Id,Price")] Dish dish)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(dish);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(dish);
        //}
        [HttpPost]
        public IActionResult Create(EditDishViewModel editDishViewModel)
        {
            var newDish = new Dish
            {
                Category = _context.Categories.FirstOrDefault(c => c.Id == editDishViewModel.Dish.CategoryId),
                Name = editDishViewModel.Dish.Name,
                Price = editDishViewModel.Dish.Price,
                DishIngredients = new List<DishIngredient>()
            };

            var dbIngredients = _context.Ingredients.ToList();
            if (editDishViewModel.IngredientId != null)
            {
                foreach (var dishIngredientId in editDishViewModel.IngredientId)
                {
                    var dishIngredient =
                        new DishIngredient
                        {
                            Ingredient = dbIngredients.Find(i => i.Id == dishIngredientId),
                            Dish = newDish,
                            IngredientId = dishIngredientId
                        };
                    newDish.DishIngredients.Add(dishIngredient);
                }
            }
            
            _context.Dishes.Add(newDish);
            _context.SaveChanges();
            var dishes = new DishCartViewModel();
            dishes.Dishes = _context.Dishes.ToList();
            return RedirectToAction("Index", "Dishes");
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dbdish = _context.Dishes.FirstOrDefault(x => x.Id == id);
            dbdish.DishIngredients = _context.DishIngredients.Where(x => x.DishId == dbdish.Id).ToList();

            var customizeDishViewModel = new EditDishViewModel
            {
                Dish = dbdish,
                IngredientId = new List<int>(),
                Ingredients = _context.Ingredients.ToList(),
                Categories = _context.Categories.ToList()
            };

            foreach (var dishIngredient in dbdish.DishIngredients)
            {
                customizeDishViewModel.IngredientId.Add(dishIngredient.IngredientId);
            }

            return View(customizeDishViewModel);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Name,Id,Price")] Dish dish)
        public async Task<IActionResult> Edit(EditDishViewModel editDishViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dbdish = _context.Dishes.FirstOrDefault(x => x.Id == editDishViewModel.Dish.Id);

                    var tempDishIng = new List<DishIngredient>();
                    tempDishIng = _context.DishIngredients.Where(di => di.DishId == editDishViewModel.Dish.Id).ToList();

                    foreach (var item in tempDishIng)
                    {
                        _context.DishIngredients.Remove(item);
                    }
                    _context.SaveChanges();

                    var dbIngredients = _context.Ingredients.Where(x => editDishViewModel.IngredientId.Contains(x.Id))
                        .ToList();
                    dbdish.Category =
                        _context.Categories.FirstOrDefault(c => c.Id == editDishViewModel.Dish.CategoryId);
                    dbdish.CategoryId = editDishViewModel.Dish.CategoryId;
                    dbdish.Name = editDishViewModel.Dish.Name;
                    dbdish.Price = editDishViewModel.Dish.Price;

                    foreach (var ingredient in dbIngredients)
                    {
                        var dishIngredient =
                            new DishIngredient {Ingredient = ingredient, Dish = dbdish, IngredientId = ingredient.Id};
                        _context.DishIngredients.Add(dishIngredient);
                    }
                    _context.Update(dbdish);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(editDishViewModel.Dish.Id))
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
            return View(editDishViewModel);
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
            var dishIngredients = await _context.DishIngredients.ToArrayAsync();

            foreach (var dishIngredient in dishIngredients.Where(dii => dii.DishId == dish.Id))
            {
                _context.DishIngredients.Remove(dishIngredient);
            }
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}