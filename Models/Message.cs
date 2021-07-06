using System;

namespace TaskManager.Models
{
    public class Message
    {
        public string MessageText { get; set; }
        public string SenderId { get; set; }
        public DateTime Time { get; set; }
    }
}