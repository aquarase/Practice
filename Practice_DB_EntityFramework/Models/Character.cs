using System.ComponentModel.DataAnnotations;

namespace Practice_DB_EntityFramework.Models
{
    public class Character
    {
        [Key]
        public int CharacterID { get; set; }

        [MaxLength(30)]
        public string? CharacterName { get; set; }

        public int? CharacterLevel { get; set; }

        public bool? IsAlive { get; set; }
    }
}