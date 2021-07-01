using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TaskManager.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class Order : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }
        public OrderStatus Status { get; set; }
        public string Message { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime Deadline { get; set; }
        public string AuthorId { get; set; }
        public string PerformerId { get; set; }
        public bool Expired => DateTime.Compare(DateTime.Now, Deadline) == 1;
        public int LeftDays => (DateTime.Now - Deadline).Days;
    }
}