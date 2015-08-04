namespace MvcCatalogue
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Security;

    public class MyRoleProvider : RoleProvider
    {
        private int _cacheTimeoutInMinute = 20;

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            // Check cache
            var cacheKey = string.Format("{0}_role", username);

            if (HttpRuntime.Cache[cacheKey] != null)
            {
                return (string[])HttpRuntime.Cache[cacheKey];
            }

            string[] roles = new string[]{};

            using (DatabaseEntities de = new DatabaseEntities())
            {
                roles = (from a in de.Roles
                        join b in de.UserRoles on a.RoleId equals b.RoleId
                        join c in de.Users on b.UserId equals c.UserId
                        where c.Username.Equals(username)
                        select a.RoleName).ToArray<string>();

                if (roles.Count() > 0)
                {
                    HttpRuntime.Cache.Insert(cacheKey, roles, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinute), 
                        Cache.NoSlidingExpiration);
                }
            }

            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);

            return userRoles.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}