using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TaskManager.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class Order : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime Deadline { get; set; }
        public string Message { get; set; }
        public string AuthorId { get; set; }
        public string PerformerId { get; set; }
        public int LeftDays => (Deadline - DateTime.Now).Days;
        public bool Expired => LeftDays < 0;
    }
}