using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using TaskManager.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class User : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string HashPassword { get; set; }
        public Roles Role { get; set; }
        public string Post { get; set; }
        public string Placement { get; set; }
        public List<string> Orders { get; set; }
    }
}