namespace VeilOfAethra
{
    public class Zone
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] AsciiArt { get; set; }

        public Zone(string name, string description, string[] asciiArt)
        {
            Name = name;
            Description = description;
            AsciiArt = asciiArt;
        }

        public void Display()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            System.Console.WriteLine("══════════════════════════════════════════");
            System.Console.WriteLine($" {Name}");
            System.Console.WriteLine("══════════════════════════════════════════");
            System.Console.ResetColor();

            foreach (var line in AsciiArt)
            {
                System.Console.WriteLine(line);
            }

            System.Console.WriteLine();
            System.Console.WriteLine(Description);
            System.Console.WriteLine();
        }
    }
}