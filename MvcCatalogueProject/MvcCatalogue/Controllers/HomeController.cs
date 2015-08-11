namespace MvcCatalogue.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();

        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<ImageGallery> list = de.ImageGalleries.ToList();
            ViewBag.Gallery = list.ToList();
            ViewBag.Categories = de.Categories.ToList();

            return View(de.Products.ToList());
        }

        // GET: Home/Contact
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        // GET: Home/MyProfile
        [Authorize]
        public ActionResult MyProfile()
        {
            var users = de.Users.ToList();
            User user = new User();

            foreach (var userItem in users)
            {
                if (userItem.Username == User.Identity.Name)
                {
                    user = userItem;
                }
            }

            return View(user);
        }

        // GET: Home/AdminIndex
        [Authorize(Roles="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }

        // GET: Home/Error
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}