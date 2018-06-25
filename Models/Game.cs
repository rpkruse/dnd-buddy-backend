using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class Game
    {
        public Game()
        {
            Character = new HashSet<Character>();
        }

        public int GameId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool GM { get; set; }

        public virtual ICollection<Character> Character { get; set; }
        public virtual User User { get; set; }
    }
}
