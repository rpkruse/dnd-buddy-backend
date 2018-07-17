using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public class ChatMessageData
    {
        public string GroupName { get; set; } //Will either be the group or private message
        public string Username { get; set; }
        public string Message { get; set; }
        public string ConnectionID { get; set; }
        public bool IsPrivate { get; set; }
    }
}
