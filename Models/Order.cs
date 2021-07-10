using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TaskManager.Data;
using TaskManager.Data.Contracts;

namespace TaskManager.Models
{
    public class Order : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string PerformerId { get; set; }
        public string ChatId { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public DateTime Deadline { get; set; }
        public OrderStatus Status { get; set; }
        public bool InWork => Status == OrderStatus.InWork;
        public bool NewMessageForUser { get; set; }
        public bool NewMessageForAdmin { get; set; }
    }
}