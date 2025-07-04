using System;

namespace VeilOfAethra
{
    public class Enemy
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int AttackPower { get; set; }
        public string[] AsciiSprite { get; set; }

        public Enemy(string name, int hp, int attackPower, string[] asciiSprite)
        {
            Name = name;
            HP = hp;
            AttackPower = attackPower;
            AsciiSprite = asciiSprite;
        }

        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Enemy: {Name}]");
            Console.ResetColor();

            foreach (var line in AsciiSprite)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine($"HP: {HP}");
            Console.WriteLine();
        }
    }
}