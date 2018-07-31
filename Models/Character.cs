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
            Item = new HashSet<Item>();
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

        public string Armor { get; set; }
        public string Weapon { get; set; }
        public string Shield { get; set; }
        public string Neck { get; set; }
        public string Ring_1 { get; set; }
        public string Ring_2 { get; set; }

        public int Xp { get; set; }

        public int UserId { get; set; }

        public int GameId { get; set; }

        public virtual User User { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<Item> Item { get; set; }

    }
}
