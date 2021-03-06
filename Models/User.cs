﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class User
    {
        public User()
        {
            Character = new HashSet<Character>();
            Game = new HashSet<Game>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Character> Character { get; set; }
        public virtual ICollection<Game> Game { get; set; }
    }
}
