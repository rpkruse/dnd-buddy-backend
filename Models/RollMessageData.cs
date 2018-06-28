using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public class RollMessageData 
    {
        public string GroupName { get; set; }
        public int CharId { get; set; }
        public int Roll { get; set; }
        public int NumDice { get; set; }
        public int MaxRoll { get; set; }
    }
}
