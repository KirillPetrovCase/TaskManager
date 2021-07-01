using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TaskManager.Data;

namespace TaskManager.Models
{
    public class Placement : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}