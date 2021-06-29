﻿using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Extensions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class PlacementManager
    {
        private readonly IMongoCollection<Placement> placementDb;

        public PlacementManager()
        {
            IMongoDatabase mongoDatabase = DbExtensions.GetDatabase();

            placementDb = mongoDatabase.GetCollection<Placement>("Placements");
        }

        public async Task<IEnumerable<Placement>> GetPlacementAsync()
            => await placementDb.Find(FilterDefinition<Placement>.Empty).ToListAsync();
    }
}