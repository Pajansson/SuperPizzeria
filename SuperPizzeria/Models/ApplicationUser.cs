using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace SuperPizzeria.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Adress { get; set; }
    }
    public static class IdentityExt
    {
        public static Task<T> GetCurrentUser<T>(this UserManager<T> manager, HttpContext httpContext) where T : class
        {
            return manager.GetUserAsync(httpContext.User);
        }
    }
}
