using System;

namespace ChatApp.Shared
{
    // BASE CLASS (The Parent): Contains common properties for any type of message
    public class MessageBase
    {
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string MessageType { get; set; } // Can be "Standard", "System", or "Image"

        public MessageBase() { }
    }

    // DERIVED CLASS (The Child): Inherits from MessageBase and adds specific content
    // This is where INHERITANCE happens ( : MessageBase )
    public class ChatMessage : MessageBase
    {
        public string Message { get; set; }

        public ChatMessage()
        {
            MessageType = "Standard";
        }

        public ChatMessage(string sender, string content)
        {
            Sender = sender;
            Message = content;
            MessageType = "Standard";
        }
    }
}