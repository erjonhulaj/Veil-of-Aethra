using System.Collections.Generic;

namespace VeilOfAethra
{
    public class SaveGame
    {
        public string Gender { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public List<string> Inventory { get; set; }
        public string CurrentZone { get; set; }
        public List<string> QuestFlags { get; set; }
        public List<string> Runes { get; set; }
    }
}