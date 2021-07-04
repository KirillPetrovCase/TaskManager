using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TaskManager.Data;

namespace TaskManager.Models
{
    public class Placement : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}