using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgApp.Models
{
    internal class Character
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public int CharacterLevel { get; set; }
        public bool IsAlive { get; set; }
        public int? GuildId { get; set; }
    }
}
