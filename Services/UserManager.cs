﻿using MongoDB.Driver;
using System.Threading.Tasks;
using TaskManager.Extensions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class UserManager
    {
        private readonly IMongoCollection<User> userDb;

        public UserManager()
        {
            IMongoDatabase mongoDatabase = DbExtensions.GetDatabase();

            userDb = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task CreateAsync(User user) => await userDb.InsertOneAsync(user);

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var filter = Builders<User>.Filter.Where(user => user.UserName == userName);

            return await userDb.Find(filter).FirstOrDefaultAsync();
        }
    }
}