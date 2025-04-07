using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Interfaces
{
    public interface IAuthService
    {
        Task<String> CreateTokenAsync(User user, UserManager<User> userManager);
    }
}
