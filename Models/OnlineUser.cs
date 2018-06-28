using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public class OnlineUser
    {
        public virtual UserMessageData UMD { get; set; }
        public virtual RollMessageData RMD { get; set; }
        public virtual ItemMessageData ITM { get; set; }
    }
}
