using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using TaskManager.Data;

namespace TaskManager.Models
{
    public class Chat : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OrderId { get; set; }
        public List<Message> Messages { get; set; }
    }
}