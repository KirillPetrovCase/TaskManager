using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TaskManager.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class User : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Login { get; set; }
        public string Name { get; set; }
        public string HashPassword { get; set; }
        public string Placement { get; set; }
        public Roles Role { get; set; }
    }
}