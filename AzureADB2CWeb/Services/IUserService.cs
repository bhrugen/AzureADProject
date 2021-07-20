using AzureADB2CWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADB2CWeb.Services
{
    public interface IUserService
    {
        User Create(User user);
        User GetById(string b2cObjectId);
        Task<string> GetB2CTokenAsync();
        User GetUserFromSession();
    }
}
