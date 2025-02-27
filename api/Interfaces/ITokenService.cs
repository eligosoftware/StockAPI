using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserManager<AppUser> userManager,AppUser user);
    }
}