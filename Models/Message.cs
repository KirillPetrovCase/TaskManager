using System;

namespace TaskManager.Models
{
    public class Message
    {
        public string MessageText { get; set; }
        public string SenderName { get; set; }
        public DateTime Time { get; set; }
    }
}