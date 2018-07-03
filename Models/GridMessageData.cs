using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public class GridMessageData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
    }
}
