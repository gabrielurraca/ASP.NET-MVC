namespace MvcCatalogue.Controllers
{
    using MvcCatalogue.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    public class AccountController : Controller
    {
        private DatabaseEntities de = new DatabaseEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnURL = "")
        {
            #region Existing Code 01
            //using (DatabaseEntities de = new DatabaseEntities())
            //{
            //    var user = de.Users.Where(a=>a.Username.Equals(l.Username) && a.Password.Equals(l.Password)).FirstOrDefault();

            //    if (user != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(user.Username, l.RememberMe);

            //        if (Url.IsLocalUrl(ReturnURL))
            //        {
            //            return Redirect(ReturnURL);
            //        }
            //        else
            //        {
            //            return RedirectToAction("MyProfile", "Home");
            //        }
            //    }
            //}
            #endregion

            if (ModelState.IsValid)
            {
                var isValidUser = Membership.ValidateUser(l.Username, l.Password);

                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);

                    if (Url.IsLocalUrl(ReturnURL))
                    {
                        return Redirect(ReturnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }          

            ModelState.Remove("Password");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,FullName,EmailID")] User user)
        {
            IEnumerable<User> users = de.Users;

            if (de.Users.Any(u => u.Username == user.Username))
            {
                return RedirectToAction("Error", "Home");
            }

            //foreach (var userItem in users)
            //{
            //    if (userItem.Username == user.Username)
            //    {
            //        return RedirectToAction("Error", "Home");
            //    }
            //}

            if (ModelState.IsValid)
            {
                de.Users.Add(user);
                de.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordValuesModel model)
        {
            if (ModelState.IsValid)
            {
                var password = de.Users.Where(m => m.Username == User.Identity.Name)
                    .Select(m => m.Password)
                    .Single();

                if (model.OldPassword == password && model.Password == model.RepeatPassword)
                {
                    var users = de.Users.ToList();

                    foreach (var userItem in users)
                    {
                        if (userItem.Username == User.Identity.Name)
                        {
                            userItem.Password = model.Password;

                            de.Entry(userItem).State = EntityState.Modified;
                            de.SaveChanges();

                            break;
                        }
                    }

                    return RedirectToAction("MyProfile", "Home");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var users = de.Users.ToList();

                foreach (var userItem in users)
                {
                    if (userItem.Username == User.Identity.Name)
                    {
                        userItem.FullName = model.FullName;
                        userItem.EmailID = model.Email;

                        de.Entry(userItem).State = EntityState.Modified;
                        de.SaveChanges();

                        break;
                    }
                }

                return RedirectToAction("MyProfile", "Home");
            }

            return View(model);
        }
    }
}