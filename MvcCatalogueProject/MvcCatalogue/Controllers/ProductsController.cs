namespace MvcCatalogue.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ProductsController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();

        // GET: Products
        public ActionResult Index()
        {            
            if (de.Products != null)
            {
                var products = de.Products.ToList();
                ViewBag.Categories = de.Categories.ToList();

                return View(products);
            }

            return View();
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var comments = de.Comments.Where(m => m.ProductID == id);
            var users = de.Users.ToList();
            var commentNameList = new List<string>();

            foreach (var commentsItem in comments)
            {
                foreach (var usersItem in users)
                {
                    if (commentsItem.UserID == usersItem.UserId)
                    {
                        commentNameList.Add(usersItem.Username);

                        break;
                    }
                }
            }

            ViewBag.Comments = comments;
            ViewBag.Users = commentNameList;
            ViewBag.Categories = de.Categories.ToList();

            var products = de.Products.Find(id);

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "Comment1")]int id, Comment collection)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Product> products = de.Products.ToList();
                IEnumerable<User> users = de.Users.ToList();

                collection.ProductID = id;

                //Console.WriteLine(users.FirstOrDefault(m => m.UserId.Equals(User.Identity.Name)));
                //collection.UserID = users.FirstOrDefault(m => m.UserId.Equals(User.Identity.Name));

                foreach (var item in users)
                {
                    if (item.Username == User.Identity.Name)
                    {
                        collection.UserID = item.UserId;

                        break;
                    }
                }

                de.Comments.Add(collection);
                de.SaveChanges();

                //return RedirectToAction("Index");

                var comments = de.Comments.Where(m => m.ProductID == id);
                var commentNameList = new List<string>();

                foreach (var commentsItem in comments)
                {
                    foreach (var usersItem in users)
                    {
                        if (commentsItem.UserID == usersItem.UserId)
                        {
                            commentNameList.Add(usersItem.Username);

                            break;
                        }
                    }
                }

                ViewBag.Comments = comments;
                ViewBag.Users = commentNameList;
                ViewBag.Categories = de.Categories.ToList();
            }

            var product = de.Products.Find(id);

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            int selectedItem = 0;
            ViewBag.GetCategories = new SelectList(de.Categories.ToList(), "CategoryId", "CategoryName", selectedItem);

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductTitle,ProductContent,ProductPrice,CategoryId")] Product collection)
        {
            int selectedItem = 0;
            ViewBag.GetCategories = new SelectList(de.Categories.ToList(), "CategoryId", "CategoryName", selectedItem);

            if (ModelState.IsValid)
            {
                try
                {                        
                    de.Products.Add(collection);
                    de.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception) { }
            }

            return View(collection);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Product product = de.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            int selectedItem = product.CategoryId;

            ViewBag.GetCategories = new SelectList(de.Categories.ToList(), "CategoryId", "CategoryName", selectedItem);

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductTitle,ProductContent,ProductPrice,CatalogueId")] int id, Product collection)
        {
            if (ModelState.IsValid)
            {
                de.Entry(collection).State = EntityState.Modified;
                de.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(collection);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Product category = de.Products.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Product product = de.Products.Find(id);
                de.Products.Remove(product);
                de.SaveChanges();

                return RedirectToAction("Index");
            }
            catch { }

            return View();
        }

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
