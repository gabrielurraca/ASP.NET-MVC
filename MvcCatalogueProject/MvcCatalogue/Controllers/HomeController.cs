namespace MvcCatalogue.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(de.Products.ToList());
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

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

        [Authorize(Roles="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}