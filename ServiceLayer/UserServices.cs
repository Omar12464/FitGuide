using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserServices
    {
        private readonly UserManager<User> _userManager;

        public UserServices(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

    }
}
