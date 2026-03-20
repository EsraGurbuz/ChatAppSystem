using System;

namespace ChatApp.Shared
{
    // Encapsulation
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public ChatMessage() { }

        public ChatMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
}