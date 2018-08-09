using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class Monster
    {
        public Monster()
        {
        }
        public int MonsterId { get; set; }

        public string Name { get; set; }

        public int Max_HP { get; set; }
        public int HP { get; set; }

        public int GameId { get; set; }

        public string Url { get; set; }

        public virtual Game Game { get; set; }
    }
}
