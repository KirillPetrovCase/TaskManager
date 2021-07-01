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
        public User Author { get; set; }
        public User Performer { get; set; }
        public bool Expired => DateTime.Compare(DateTime.Now, Deadline) == 1;
    }
}