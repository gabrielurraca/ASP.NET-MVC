namespace MvcCatalogue.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using MvcCatalogue;

    [Authorize(Roles="Admin")]
    public class CategoriesController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View(de.Categories.ToList());
        }

        // GET: Categories/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Categories/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = de.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (de.Categories.Any(u => u.CategoryName == category.CategoryName))
            {
                return RedirectToAction("Error", "Categories");
            }

            if (ModelState.IsValid)
            {
                try 
	            {
                    de.Categories.Add(category);
                    de.SaveChanges();

                    return RedirectToAction("Index");
	            }
	            catch (Exception) { }
            }

            return View(category);
        }

        // GET: Categories/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = de.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                de.Entry(category).State = EntityState.Modified;
                de.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = de.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = de.Categories.Find(id);
            de.Categories.Remove(category);
            de.SaveChanges();

            return RedirectToAction("Index");
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                de.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
