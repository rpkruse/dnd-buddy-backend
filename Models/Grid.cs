using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class Grid
    {
        public int GridId { get; set; }
        public string Name { get; set; }
        public string GridData { get; set; }
    }
}
