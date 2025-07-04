using System;
using System.Collections.Generic;

namespace VeilOfAethra
{
    public class Player
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public List<string> Inventory { get; set; }
        public List<string> QuestFlags { get; set; }
        public List<string> Runes { get; set; }

        public string[] AsciiSprite { get; set; }

        public Player(string gender)
        {
            Gender = gender;
            Name = "Seeker";
            MaxHP = 100;
            HP = 100;
            MaxMana = 30;
            Mana = 30;
            Strength = 5;
            Intelligence = 5;
            XP = 0;
            Level = 1;
            Inventory = new List<string>();
            QuestFlags = new List<string>();
            Runes = new List<string>();

            if (gender == "Male")
            {
                AsciiSprite = new string[]
                {
                    "   O",
                    "  /|\\",
                    "  / \\"
                };
            }
            else
            {
                AsciiSprite = new string[]
                {
                    "   O",
                    "  /|\\",
                    "  / \\",
                    " /   \\"
                };
            }
        }

        public void Display()
        {
            foreach (var line in AsciiSprite)
            {
                Console.WriteLine(line);
            }
        }
    }
}