using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace arabahw.Controllers
{
    public class ModelsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ModelsController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Models
        public async Task<IActionResult> Index(int? brandId)
        {
            ViewBag.NoModels = false;

            IQueryable<Model> modelQuery = db.Models.Include(m => m.Brand);

            if (brandId.HasValue)
            {
                modelQuery = modelQuery.Where(m => m.BrandId == brandId.Value);
                ViewBag.BrandName = (await db.Brands.FindAsync(brandId.Value))?.Name;
            }

            var models = await modelQuery.ToListAsync();

            if (brandId.HasValue && !models.Any())
            {
                // No models found for the specified brand
                ViewBag.NoModels = true;
                // BrandId and BrandName are already set above if brandId has value.
            }

            return View(models);
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(db.Brands, "BrandId", "Name");
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ModelId, Name, BrandId")] Model model)
        {
            if (ModelState.IsValid)
            {
                db.Models.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", new { brandId = model.BrandId });
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "Name", model.BrandId);
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(db.Brands, "BrandId", "BrandId", model.BrandId);
            return View(model);
        }

        // POST: Models/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,Name,BrandId")] Model model)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(model);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
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
            ViewData["BrandId"] = new SelectList(db.Brands, "BrandId", "BrandId", model.BrandId);
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await db.Models.FindAsync(id);
            if (model != null)
            {
                db.Models.Remove(model);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
            return db.Models.Any(e => e.ModelId == id);
        }

    }
}
