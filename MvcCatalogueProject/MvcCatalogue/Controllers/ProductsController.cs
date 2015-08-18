namespace MvcCatalogue.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
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

                List<ImageGallery> list = de.ImageGalleries.ToList();
                ViewBag.Gallery = list.ToList();

                return View(products);
            }

            return View();
        }

        // GET: Products/Details
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

            IQueryable<ImageGallery> gallery = de.ImageGalleries.Where(m => m.ProductId == id);
            List<ImageGallery> list = gallery.ToList();

            IList<Like> likes = de.Likes.Where(m => m.ProductID == id).ToList();
            ViewBag.Likes = likes;

            IEnumerable<User> usernames = de.Users.ToList();
            ViewBag.Usernames = usernames;

            ViewBag.Gallery = list;
            ViewBag.Comments = comments;
            ViewBag.Users = commentNameList;
            ViewBag.Categories = de.Categories.ToList();

            var products = de.Products.Find(id);

            return View(products);
        }

        // POST: Products/Details
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

                IQueryable<ImageGallery> gallery = de.ImageGalleries.Where(m => m.ProductId == id);
                List<ImageGallery> list = gallery.ToList();

                IList<Like> likes = de.Likes.Where(m => m.ProductID == id).ToList();
                ViewBag.Likes = likes;

                IEnumerable<User> usernames = de.Users.ToList();
                ViewBag.Usernames = usernames;

                ViewBag.Gallery = list;
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

        // GET: Products/Edit
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

            IQueryable<ImageGallery> gallery = de.ImageGalleries.Where(m => m.ProductId == id);
            List<ImageGallery> list = gallery.ToList();

            ViewBag.Gallery = list;

            return View(product);
        }

        // POST: Products/Edit
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

        // GET: Products/Delete
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Product category = de.Products.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            IQueryable<ImageGallery> gallery = de.ImageGalleries.Where(m => m.ProductId == id);
            List<ImageGallery> list = gallery.ToList();

            ViewBag.Gallery = list;

            return View(category);
        }

        // POST: Products/Delete
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                IQueryable<ImageGallery> gallery = de.ImageGalleries.Where(m => m.ProductId == id);
                List<ImageGallery> list = gallery.ToList();

                foreach (var item in list)
                {
                    de.ImageGalleries.Remove(item);
                }

                IQueryable<Comment> comments = de.Comments.Where(m => m.ProductID == id);
                List<Comment> listComments = comments.ToList();

                foreach (var commentItem in listComments)
                {
                    de.Comments.Remove(commentItem);
                }

                IQueryable<Like> likes = de.Likes.Where(m => m.ProductID == id);
                List<Like> listLikes = likes.ToList();

                foreach (var item in listLikes)
                {
                    de.Likes.Remove(item);
                }

                Product product = de.Products.Find(id);
                de.Products.Remove(product);
                de.SaveChanges();

                return RedirectToAction("Index");
            }
            catch { }

            return View();
        }

        // GET: Products/Upload
        [Authorize(Roles = "Admin")]
        public ActionResult Upload(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ImageGallery gallery = de.ImageGalleries.Where(m => m.ProductId == id).FirstOrDefault();

            return View(gallery);
        }

        // POST: Products/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(int id, ImageGallery IG)
        {
            try
            {
                if (IG.File.ContentLength > (2 * 1024 * 1024))
                {
                    ModelState.AddModelError("CustomError", "File must NOT be higher than 2 MB");

                    return View();
                }
                if (IG.File.ContentType != "image/jpeg" && IG.File.ContentType != "image/png" &&
                    IG.File.ContentType != "image/jpg" && IG.File.ContentType != "image/gif")
                {
                    ModelState.AddModelError("CustomError", "File type should be jpeg, png, jpg or gif");

                    return View();
                }

                IG.FileName = IG.File.FileName;
                IG.ImageSize = IG.File.ContentLength;

                byte[] data = new byte[IG.File.ContentLength];
                IG.File.InputStream.Read(data, 0, IG.File.ContentLength);

                IG.ImageData = data;
                IG.ProductId = id;

                Product product = de.Products.Find(id);

                using (DatabaseEntities de = new DatabaseEntities())
                {
                    de.ImageGalleries.Add(IG);
                    de.SaveChanges();
                }
            }
            catch (Exception) { }
            
            return RedirectToAction("Edit", "Products", new { id = id });
        }

        // Search by category
        public ActionResult Search(int searchIndex)
        {
            List<Product> list = de.Products.Where(m => m.CategoryId == searchIndex).ToList();

            List<ImageGallery> listGallery = de.ImageGalleries.ToList();
            ViewBag.Gallery = listGallery.ToList();
            ViewBag.Categories = de.Categories.ToList();

            return View(list);
        }

        // Search by written text
        public ActionResult SearchText(string searchText)
        {
            List<Product> list = de.Products.ToList();
            List<Product> result = new List<Product>();

            foreach (var item in list)
            {
                if (CheckIt(item, searchText))
                {
                    result.Add(item);
                }
            }

            List<ImageGallery> listGallery = de.ImageGalleries.ToList();
            ViewBag.Gallery = listGallery.ToList();
            ViewBag.Categories = de.Categories.ToList();

            return View(result);
        }

        private static bool CheckIt(Product product, string search)
        {
            string name = product.ProductTitle.ToString().ToLower();

            if (name.StartsWith(search.ToLower()))
            {
                return true;
            }

            return false;
        }

        // POST: Products/Rate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Rate(int id, int rate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Like like = new Like();
                    like.ProductID = id;
                    User user = de.Users.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
                    like.UserID = user.UserId;
                    like.Likes = rate;

                    List<Like> likes = de.Likes.Where(m => m.ProductID == id).ToList();
                    Like findUser = likes.Where(m => m.UserID == user.UserId).FirstOrDefault();

                    if (findUser != null)
                    {
                        return RedirectToAction("Details", "Products", new { id = id });
                    }

                    de.Likes.Add(like);
                    de.SaveChanges();

                    return RedirectToAction("Details", "Products", new { id = id });
                }
                catch (Exception) { }
            }

            return RedirectToAction("Details", "Products", new { id = id });
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
