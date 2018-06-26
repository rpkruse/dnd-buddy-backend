using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class Character
    {
        public Character()
        {

        }

        public int CharacterId { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }

        public int Abil_Score_Str { get; set; }
        public int Abil_Score_Dex { get; set; }
        public int Abil_Score_Con { get; set; }
        public int Abil_Score_Int { get; set; }
        public int Abil_Score_Wis { get; set; }
        public int Abil_Score_Cha { get; set; }

        public int Level { get; set; }

        public int UserId { get; set; }

        public int GameId { get; set; }

        public virtual User User { get; set; }
        public virtual Game Game { get; set; }
    }
}
