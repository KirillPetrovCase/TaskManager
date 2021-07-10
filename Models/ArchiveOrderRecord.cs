using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TaskManager.Data;

namespace TaskManager.Models
{
    public class ArchiveOrderRecord : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public string PerformerId { get; set; }
        public string ChatId { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public DateTime ArchivedTime { get; set; }
        public DateTime Deadline { get; set; }
    }
}