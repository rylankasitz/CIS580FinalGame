using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Scenes.Rooms
{
    public class Room
    {
        public string MapName { get { return Type + "_" + Name + "_" + Directions; } }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Directions { get; set; }
        public string DungeonName { get; set; }
        public bool Flip { get; set; }
        public bool HasKey { get; set; } = false;
        public bool HasBossKey { get; set; } = false;
    }
}
