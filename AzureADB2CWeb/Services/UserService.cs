using AzureADB2CWeb.Data;
using AzureADB2CWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADB2CWeb.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public User Create(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetB2CTokenAsync()
        {
            throw new NotImplementedException();
        }

        public User GetById(string b2cObjectId)
        {
            throw new NotImplementedException();
        }

        public User GetUserFromSession()
        {
            throw new NotImplementedException();
        }
    }
}
