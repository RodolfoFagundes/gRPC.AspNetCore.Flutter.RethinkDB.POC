using System;

namespace GrpcServer.Db.Abstractions
{
    public class ChatModel
    {
        public string User { get; set; }

        public string Content { get; set; }

        public DateTime DateTimeChange { get; set; }

        public override string ToString()
        {
            return $"IdChat: {User}, Content: {Content}, DateTimeChange: {DateTimeChange}";
        }
    }
}
