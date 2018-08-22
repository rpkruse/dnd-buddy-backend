using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class Item
    {
        public Item()
        {
           
        }

        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
        public string Cost_Type { get; set; }
        public bool CanEquip { get; set; }
        public int CharacterId { get; set; }

        public virtual Character Character { get; set; }
    }
}
