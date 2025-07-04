namespace VeilOfAethra
{
    public class Npc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dialogue { get; set; }

        public Npc(string name, string description, string dialogue)
        {
            Name = name;
            Description = description;
            Dialogue = dialogue;
        }

        public void Talk()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Magenta;
            System.Console.WriteLine($"{Name} says:");
            System.Console.ResetColor();
            System.Console.WriteLine();
            System.Console.WriteLine($"\"{Dialogue}\"");
            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadKey();
        }
    }
}