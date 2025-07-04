using System;

namespace VeilOfAethra
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowTitleScreen();
        }

        private static CancellationTokenSource manaRegenTokenSource = new CancellationTokenSource();

        static void ShowTitleScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine("             VEIL OF AETHRA               ");
            Console.WriteLine("══════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("The world trembles at the edge of dissolution.");
            Console.WriteLine("You, a Seeker touched by the Veil, must find");
            Console.WriteLine("the three lost fragments before all memory fades.");
            Console.WriteLine();

            Console.WriteLine("[1] New Game");
            Console.WriteLine("[2] Load Game");
            Console.WriteLine("[3] Quit");
            Console.WriteLine();

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartNewGame();
                    break;
                case "2":
                    LoadGame();
                    break;
                case "3":
                    QuitGame();
                    break;
                default:
                    Console.WriteLine("Invalid input. Press any key to return.");
                    Console.ReadKey();
                    ShowTitleScreen();
                    break;
            }
        }

        static void StartNewGame()
        {
            Player player = CharacterSelection();

            Console.Clear();
            Console.WriteLine("You have chosen:");
            Console.WriteLine();
            player.Display();
            Console.WriteLine();
            Console.WriteLine("Press any key to begin your journey...");
            Console.ReadKey();

            StartGameLoop(player);
        }

        static void LoadGame()
        {
            Console.Clear();
            Console.WriteLine("Loading game...");
            var (player, currentZone) = LoadPlayer();

            if (player == null)
            {
                Console.WriteLine("No saved game found.");
                Console.WriteLine("Press any key to return.");
                Console.ReadKey();
                ShowTitleScreen();
            }
            else
            {
                Console.WriteLine("Game loaded successfully!");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                StartGameLoop(player, currentZone);
            }
        }

        static void QuitGame()
        {
            Console.Clear();
            Console.WriteLine("Thank you for playing Veil of Aethra.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static Player CharacterSelection()
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine(" Choose your identity:");
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine();
            Console.WriteLine("[1] Male Seeker");
            Console.WriteLine("[2] Female Seeker");
            Console.WriteLine();

            Console.Write("Your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    return new Player("Male");
                case "2":
                    return new Player("Female");
                default:
                    Console.WriteLine("Invalid input. Press any key to try again.");
                    Console.ReadKey();
                    return CharacterSelection();
            }
        }

        static void StartGameLoop(Player player, Zone currentZone = null)
        {
            // Zones
            Zone village = new Zone(
                "The Village of the Last Flame",
                "A crumbling village shrouded in the Veil.",
                new string[]
                {
                    "     [] [] []",
                    "    [  _   _ ]",
                    "     | |_| |",
                    "      /   \\"
                });

            Zone crypt = new Zone(
                "The Decayed Crypt",
                "Ancient runes cover the cracked walls.",
                new string[]
                {
                    "     ____",
                    "    / ___|",
                    "   | |",
                    "   | |___",
                    "    \\____|"
                });

            Zone forest = new Zone(
                "The Misty Forest",
                "Shadows twist between trees wrapped in fog.",
                new string[]
                {
                    "    &&& &&  & &&",
                    "  && &\\/&\\|& ()|/ @, &&",
                    "  &\\/(/&/&||/& /_/)_&/_&",
                    "&&() &\\/&|()|/&\\/ '% &"
                });

            Zone cavern = new Zone(
                "Cavern of Whispers",
                "An eerie silence pervades the twisting tunnels.",
                new string[]
                {
                    "   ~ ~ ~ ~ ~ ~",
                    "  ~  ~  ~ ~  ~",
                    " ~   ~ ~   ~ ~"
                });

            Zone spire = new Zone(
                "Obsidian Spire",
                "Black stone rises into endless darkness.",
                new string[]
                {
                    "     ||||",
                    "     ||||",
                    "     ||||",
                    "     ||||"
                });

            Zone sanctum = new Zone(
                "Veiled Sanctum",
                "The final bastion before oblivion.",
                new string[]
                {
                    "  ~~~   ~~~",
                    "  ~ Veil ~",
                    "  ~~~   ~~~"
                });

            Npc flameWarden = new Npc(
                "Flame Warden",
                "The last keeper of the Conclave.",
                "Seek the fragments, Seeker. Without them, Aethra will fall."
            );

            Npc runePriest = new Npc(
                "Rune Priest",
                "A scholar of forgotten magics.",
                "Beware the Crypt. Its runes test both mind and flesh."
            );

            Npc brokenKnight = new Npc(
                "Broken Knight",
                "A warrior consumed by the Veil.",
                "I tried to resist... and failed. Will you fare better?"
            );

            if (currentZone == null)
                currentZone = village;

            bool playing = true;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var regenTask = StartManaRegenerationAsync(player, tokenSource.Token);

            while (playing)
            {
                Console.Clear();
                currentZone.Display();
                player.Display();
                ShowPlayerStatus(player);

                Console.WriteLine("What will you do?");
                Console.WriteLine("[1] Travel to Village");
                Console.WriteLine("[2] Travel to Crypt");
                Console.WriteLine("[3] Travel to Forest");
                Console.WriteLine("[4] Travel to Cavern");
                Console.WriteLine("[5] Travel to Spire");
                Console.WriteLine("[6] Travel to Sanctum (Final)");
                Console.WriteLine("[7] Enter the Dungeon");
                Console.WriteLine("[8] Talk to an NPC");
                Console.WriteLine("[9] View Inventory");
                Console.WriteLine("[10] Save Game");
                Console.WriteLine("[11] Quit Game");
                Console.WriteLine();

                Console.Write("Choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        currentZone = village;
                        break;

                    case "2":
                        currentZone = crypt;
                        if (!player.Runes.Contains("Crypt Rune"))
                        {
                            bool solved = SolveCryptRiddle();
                            if (solved)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("The runes glow. You claim the Crypt Rune!");
                                player.Runes.Add("Crypt Rune");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("You failed to solve the riddle.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            TriggerRandomBattle(player);
                        }
                        break;

                    case "3":
                        currentZone = forest;
                        if (!player.Runes.Contains("Forest Rune"))
                        {
                            bool found = SearchForestAltar(player);
                            if (found)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("You find the hidden altar and claim the Forest Rune!");
                                player.Runes.Add("Forest Rune");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("You could not find the altar.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            TriggerRandomBattle(player);
                        }
                        break;

                    case "4":
                        currentZone = cavern;
                        Enemy specter = new Enemy(
                            "Specter of Echoes",
                            50,
                            12,
                            new[] { "  .-.", " (o o)", "  |=|" });
                        StartBattle(player, specter);
                        if (player.HP > 0 && !player.Runes.Contains("Echo Rune"))
                        {
                            Console.WriteLine("You claim the Echo Rune!");
                            player.Runes.Add("Echo Rune");
                            Console.ReadKey();
                        }
                        break;

                    case "5":
                        currentZone = spire;

                        if (!player.Runes.Contains("Obsidian Rune"))
                        {
                            Enemy golem = new Enemy(
                                "Obsidian Golem",
                                80,
                                16,
                                new[]{ "  [][][]", " [ 0 0 ]", "   | |" });
                            StartBattle(player, golem);

                            if (player.HP > 0)
                            {
                                Console.WriteLine("You claim the Obsidian Rune!");
                                player.Runes.Add("Obsidian Rune");

                                Console.WriteLine();
                                Console.WriteLine("You notice a faint glow behind the altar.");
                                if (SolveSpireRiddle(player))
                                {
                                    player.Inventory.Add("Ancient Key");
                                    Console.WriteLine("Ancient Key added to your inventory!");
                                }
                                else
                                {
                                    Console.WriteLine("The glow fades...");
                                }
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("You already claimed the Obsidian Rune.");
                            Console.ReadKey();
                        }
                        break;

                    case "6":
                        currentZone = sanctum;

                        if (!player.Runes.Contains("Crypt Rune") ||
                            !player.Runes.Contains("Forest Rune") ||
                            !player.Runes.Contains("Echo Rune") ||
                            !player.Runes.Contains("Obsidian Rune"))
                        {
                            Console.WriteLine("You lack the runes required to enter the Sanctum.");
                            Console.ReadKey();
                        }
                        else if (!player.Inventory.Contains("Ancient Key"))
                        {
                            Console.WriteLine("The door remains sealed. An Ancient Key seems to be required.");
                            Console.ReadKey();
                        }
                        else if (!player.Inventory.Contains("Veil Sigil"))
                        {
                            Console.WriteLine("The Sanctum trembles. A hidden chamber opens...");
                            bool defeated = StartSecretBossBattle(player);
                            if (!defeated)
                            {
                                Console.WriteLine("You must grow stronger to face this foe.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You step into the final arena. The Archshade Prime awaits.");
                            Enemy archshadePrime = new Enemy(
                                "Archshade Prime",
                                300,
                                35,
                                new[] {
                                    "   █████████",
                                    " ██         ██",
                                    "██  (X) (X)  ██",
                                    "██           ██",
                                    " ██         ██",
                                    "   █████████"
                                });
                            StartBattle(player, archshadePrime);

                            if (player.HP > 0)
                            {
                                FinalConfrontation(player);
                            }
                        }
                        break;
                    case "7":
                        ExploreDungeon(player);
                        break;

                    case "8":
                        HandleNpcInteraction(currentZone, player, flameWarden, runePriest, brokenKnight);
                        break;

                    case "9":
                        ShowInventory(player);
                        break;

                    case "10":
                        SavePlayer(player, currentZone);
                        break;

                    case "11":
                        Console.WriteLine("Farewell, Seeker...");
                        tokenSource.Cancel();
                        playing = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Press any key.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        static bool SolveCryptRiddle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You approach the ancient rune-inscribed wall...");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("A voice whispers:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\"I am not alive, but I grow.");
            Console.WriteLine("I have no lungs, but I need air.");
            Console.WriteLine("What am I?\"");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer == "fire")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The runes flare with light, acknowledging your wisdom.");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The runes fade. You have failed the test.");
                Console.ResetColor();
                return false;
            }
        }
        
        static bool SearchForestAltar(Player player)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You begin searching the Misty Forest for the hidden altar...");
            Console.ResetColor();
            Thread.Sleep(1500);

            if (player.Intelligence < 6)
            {
                Console.WriteLine("Your mind is not sharp enough to decipher the signs.");
                return false;
            }

            Console.WriteLine("You discover an ancient altar covered in moss.");
            Console.WriteLine("It whispers a question:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\"I speak without a mouth and hear without ears. I have nobody, but I come alive with wind. What am I?\"");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer.Contains("echo"))
            {
                Console.WriteLine("The altar glows with emerald light. You claim the Verdant Rune!");
                player.Runes.Add("Verdant Rune");
                return true;
            }
            else
            {
                Console.WriteLine("The altar falls silent. You answered incorrectly.");
                return false;
            }
        }

                static void HandleNpcInteraction(Zone currentZone, Player player, Npc flameWarden, Npc runePriest, Npc brokenKnight)
                {
                    Console.Clear();
                    Console.WriteLine($"You look around {currentZone.Name}.");

                    if (currentZone.Name.Contains("Village"))
                    {
                        Console.WriteLine("[1] Talk to the Flame Warden");
                        Console.WriteLine("[2] Talk to the Rune Priest");
                        Console.WriteLine("[3] Search the Shrine for Runes");
                        Console.WriteLine("[4] Return");
                        Console.Write("Choice: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                flameWarden.Talk();
                                Console.WriteLine("Press any key to return.");
                                Console.ReadKey();
                                break;

                            case "2":
                                runePriest.Talk();
                                Console.WriteLine("Press any key to return.");
                                Console.ReadKey();
                                break;

                            case "3":
                                if (player.Runes.Contains("Dust Rune"))
                                {
                                    Console.WriteLine("The shrine is empty. You already claimed the Dust Rune.");
                                }
                                else
                                {
                                    Console.WriteLine("You find the Dust Rune and feel vitality flow through you!");
                                    player.HP += 10;
                                    player.Runes.Add("Dust Rune");
                                    Console.WriteLine("Dust Rune added to your runes.");
                                }
                                Console.WriteLine("Press any key to return.");
                                Console.ReadKey();
                                break;

                            default:
                                return;
                        }
                    }
                    else if (currentZone.Name.Contains("Crypt"))
                    {
                        Console.WriteLine("[1] Speak to the Broken Knight");
                        Console.WriteLine("[2] Return");
                        Console.Write("Choice: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                brokenKnight.Talk();
                                Console.WriteLine("Press any key to return.");
                                Console.ReadKey();
                                break;

                            default:
                                return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is no one here.");
                        Console.WriteLine("Press any key to return.");
                        Console.ReadKey();
                    }
                }


               public static void StartBattle(Player player, Enemy enemy)
                {
                    Random rnd = new Random();
                    bool fighting = true;
                    bool bossDefeated = false;
                    bool fled = false;

                    while (fighting)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("══════════════════════════════════════════");
                        Console.WriteLine($" BATTLE: {enemy.Name}");
                        Console.WriteLine("══════════════════════════════════════════");
                        Console.ResetColor();

                        enemy.Display();
                        player.Display();
                        ShowPlayerStatus(player);

                        Console.WriteLine();
                        Console.WriteLine("Choose your action:");
                        Console.WriteLine("[1] Attack");
                        Console.WriteLine("[2] Cast Spell");
                        Console.WriteLine("[3] Use Item");
                        Console.WriteLine("[4] Flee");
                        Console.WriteLine();
                        Console.Write("Choice: ");
                        string input = Console.ReadLine();

                        switch (input)
                        {
                            case "1":
                                int damage = rnd.Next(5, 5 + player.Strength);
                                enemy.HP -= damage;
                                Console.WriteLine($"You strike the {enemy.Name} for {damage} damage!");
                                break;

                            case "2":
                                if (player.Mana >= 5)
                                {
                                    int spellDamage = rnd.Next(10, 10 + player.Intelligence);
                                    enemy.HP -= spellDamage;
                                    player.Mana -= 5;
                                    Console.WriteLine($"You cast Aether Bolt for {spellDamage} damage!");
                                }
                                else
                                {
                                    Console.WriteLine("Not enough mana!");
                                }
                                break;

                            case "3":
                                UseItem(player);
                                break;

                            case "4":
                                Console.WriteLine("You flee back to safety...");
                                fled = true;
                                fighting = false;
                                Console.WriteLine("Press any key to continue.");
                                Console.ReadKey();
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }

                        if (!fled && enemy.HP <= 0)
                        {
                            Console.WriteLine($"The {enemy.Name} has been defeated!");
                            player.XP += 10;
                            CheckLevelUp(player);

                            // Drop loot
                            Random lootRnd = new Random();
                            int loot = lootRnd.Next(0, 3);
                            if (loot == 0)
                            {
                                Console.WriteLine("You find an Essence of Healing!");
                                player.Inventory.Add("Essence of Healing");
                            }
                            else if (loot == 1)
                            {
                                Console.WriteLine("You find an Elixir of Power!");
                                player.Inventory.Add("Elixir of Power");
                            }
                            else
                            {
                                Console.WriteLine("You find a Mana Crystal!");
                                player.Inventory.Add("Mana Crystal");
                            }

                            // Boss Runes
                            if (enemy.Name.Contains("Archshade") && !player.Runes.Contains("Veil Rune"))
                            {
                                Console.WriteLine("You claim the Veil Rune!");
                                player.Runes.Add("Veil Rune");
                            }
                            else if (enemy.Name.Contains("Obsidian Golem") && !player.Runes.Contains("Obsidian Rune"))
                            {
                                Console.WriteLine("You claim the Obsidian Rune!");
                                player.Runes.Add("Obsidian Rune");
                            }
                            else if (enemy.Name.Contains("Specter of Echoes") && !player.Runes.Contains("Echo Rune"))
                            {
                                Console.WriteLine("You claim the Echo Rune!");
                                player.Runes.Add("Echo Rune");
                            }

                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                            return;
                        }

                        int enemyDamage = rnd.Next(3, enemy.AttackPower + 1);
                        player.HP -= enemyDamage;
                        Console.WriteLine($"The {enemy.Name} attacks you for {enemyDamage} damage!");

                        if (player.HP <= 0)
                        {
                            Console.WriteLine("You have been slain by the Veil...");
                            Console.WriteLine("Game Over.");
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }

                        Console.WriteLine("Press any key for the next turn...");
                        Console.ReadKey();
                    }
                }

        static void ShowInventory(Player player)
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine(" INVENTORY");
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine();

            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {player.Inventory[i]}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
        }

        static void UseItem(Player player)
        {
            Console.Clear();
            Console.WriteLine("Which item do you want to use?");
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("You have no items.");
                Console.WriteLine("Press any key to return.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {player.Inventory[i]}");
            }

            Console.WriteLine("[0] Cancel");
            Console.Write("Choice: ");
            string input = Console.ReadLine();

            if (input == "0")
                return;

            if (int.TryParse(input, out int index) && index > 0 && index <= player.Inventory.Count)
            {
                string selectedItem = player.Inventory[index - 1];

                if (selectedItem == "Essence of Healing")
                {
                    player.HP += 20;
                    if (player.HP > player.MaxHP)
                        player.HP = player.MaxHP;
                    Console.WriteLine("You feel rejuvenated. +20 HP.");
                }
                else if (selectedItem == "Elixir of Power")
                {
                    player.Strength += 5;
                    Console.WriteLine("Your muscles surge with power! +5 Strength for this battle.");
                }
                else if (selectedItem == "Mana Crystal")
                {
                    player.Mana += 10;
                    if (player.Mana > player.MaxMana)
                        player.Mana = player.MaxMana;
                    Console.WriteLine("Your mana replenishes by +10.");
                }
                else
                {
                    Console.WriteLine("Nothing happens...");
                }

                player.Inventory.RemoveAt(index - 1);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        
               static void SavePlayer(Player player, Zone currentZone)
        {
            var save = new SaveGame
            {
                Gender = player.Gender,
                HP = player.HP,
                MaxHP = player.MaxHP,
                Mana = player.Mana,
                MaxMana = player.MaxMana,
                XP = player.XP,
                Level = player.Level,
                Strength = player.Strength,
                Intelligence = player.Intelligence,
                Inventory = player.Inventory,
                CurrentZone = currentZone.Name,
                QuestFlags = player.QuestFlags,
                Runes = player.Runes
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(save, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText("savegame.json", json);

            Console.WriteLine("Game saved successfully!");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static (Player, Zone) LoadPlayer()
        {
            if (!System.IO.File.Exists("savegame.json"))
            {
                Console.WriteLine("No savegame found. Press any key.");
                Console.ReadKey();
                return (null, null);
            }

            string json = System.IO.File.ReadAllText("savegame.json");
            var save = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveGame>(json);

            Player player = new Player(save.Gender)
            {
                HP = save.HP,
                MaxHP = save.MaxHP,
                Mana = save.Mana,
                MaxMana = save.MaxMana,
                XP = save.XP,
                Level = save.Level,
                Strength = save.Strength,
                Intelligence = save.Intelligence,
                Inventory = save.Inventory,
                QuestFlags = save.QuestFlags,
                Runes = save.Runes
            };

            // Recreate zones
            Zone village = new Zone(
                "The Village of the Last Flame",
                "A crumbling village shrouded in the Veil.",
                new string[]
                {
                    "     [] [] []",
                    "    [  _   _ ]",
                    "     | |_| |",
                    "      /   \\"
                });

            Zone crypt = new Zone(
                "The Decayed Crypt",
                "Ancient runes cover the cracked walls.",
                new string[]
                {
                    "     ____",
                    "    / ___|",
                    "   | |",
                    "   | |___",
                    "    \\____|"
                });

            Zone forest = new Zone(
                "The Misty Forest",
                "Shadows twist between trees wrapped in fog.",
                new string[]
                {
                    "    &&& &&  & &&",
                    "  && &\\/&\\|& ()|/ @, &&",
                    "  &\\/(/&/&||/& /_/)_&/_&",
                    "&&() &\\/&|()|/&\\/ '% &"
                });

            Zone currentZone = village;
            if (save.CurrentZone.Contains("Crypt"))
                currentZone = crypt;
            else if (save.CurrentZone.Contains("Forest"))
                currentZone = forest;

            return (player, currentZone);
        }
        
        static void ExploreDungeon(Player player)
        {
            Dungeon dungeon = new Dungeon();
            bool inDungeon = true;

            while (inDungeon && !dungeon.ExitReached)
            {
                dungeon.Display();

                Console.Write("Move (W/A/S/D): ");
                char input = Console.ReadKey(true).KeyChar;

                string result = dungeon.Move(input);

                dungeon.Display();

                switch (result)
                {
                    case "treasure":
                        Random lootRnd = new Random();
                        int loot = lootRnd.Next(0, 3);
                        if (loot == 0)
                        {
                            Console.WriteLine("\nYou found an Essence of Healing!");
                            player.Inventory.Add("Essence of Healing");
                        }
                        else if (loot == 1)
                        {
                            Console.WriteLine("\nYou found an Elixir of Power!");
                            player.Inventory.Add("Elixir of Power");
                        }
                        else
                        {
                            Console.WriteLine("\nYou found a Mana Crystal!");
                            player.Inventory.Add("Mana Crystal");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        break;

                    case "enemy":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nAn enemy appears!");
                        Console.ResetColor();
                        Enemy enemy = new Enemy(
                            "Crypt Fiend",
                            30,
                            8,
                            new[]
                            {
                                @"(\(\ ",
                                @"( -.- )",
                                @"o_("")(",
                            });
                        StartBattle(player, enemy);
                        break;

                    case "exit":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nYou found the exit and escape the dungeon!");
                        Console.ResetColor();
                        inDungeon = false;
                        break;

                    case "blocked":
                        Console.WriteLine("\nA wall blocks your path.");
                        break;

                    case "out-of-bounds":
                        Console.WriteLine("\nYou cannot go further that way.");
                        break;

                    case "invalid":
                        Console.WriteLine("\nInvalid key.");
                        break;
                }
            }
        }

        static void TriggerRandomBattle(Player player)
        {
            Random rnd = new Random();
            int chance = rnd.Next(0, 100);

            if (chance < 50)
            {
                Enemy shade = new Enemy(
                    "Shade of Forgotten Dreams",
                    30,
                    8,
                    new string[]
                    {
                        "        .-.",
                        "       (o o)",
                        "       |=|",
                        "      __|__",
                        "   //.=|=.\\\\",
                        "  // .=|=. \\\\",
                        "  \\\\ .=|=. //",
                        "   \\\\(_=_)//",
                        "    (:| |:)",
                        "     || ||",
                        "     () ()",
                        "     || ||",
                        "     || ||",
                        "    ==' '=="
                    });

                StartBattle(player, shade);
            }
        }

        public static void CheckLevelUp(Player player)
        {
            int xpForNextLevel = player.Level * 20;

            if (player.XP >= xpForNextLevel)
            {
                player.Level++;
                player.XP = 0;
                player.Strength += 2;
                player.Intelligence += 1;
                player.MaxHP += 10;
                player.HP = player.MaxHP;
                player.MaxMana += 5;
                player.Mana = player.MaxMana;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Congratulations! You reached Level {player.Level}!");
                Console.WriteLine("+2 Strength, +1 Intelligence, +10 Max HP, +5 Max Mana");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        static void ShowPlayerStatus(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine($" Level {player.Level}   XP: {player.XP}");
            Console.WriteLine($" HP: {player.HP}/{player.MaxHP}   Mana: {player.Mana}/{player.MaxMana}");
            Console.WriteLine($" Strength: {player.Strength}   Intelligence: {player.Intelligence}");
            Console.WriteLine("Runes: " + (player.Runes.Count > 0 ? string.Join(", ", player.Runes) : "None"));
            Console.WriteLine("══════════════════════════════════════════");
            Console.ResetColor();
        }

        static void FinalConfrontation(Player player)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You stand before the Altar of Aethra...");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("You place your runes upon the ancient stone.");
            Console.WriteLine();

            int runeCount = player.Runes.Count;

            if (runeCount == 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All three runes resonate in unison.");
                Console.WriteLine("A pure white flame erupts, cleansing the Veil from the land.");
                Console.WriteLine("Aethra is saved. Your name will never be forgotten.");
                Console.WriteLine("=== GOOD ENDING ===");
                Console.ResetColor();
            }
            else if (runeCount == 2)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("The runes pulse weakly.");
                Console.WriteLine("You channel the last of your life into the flame.");
                Console.WriteLine("The Veil retreats, but your body fades into the ether.");
                Console.WriteLine("Aethra endures, though your sacrifice is eternal.");
                Console.WriteLine("=== NEUTRAL ENDING ===");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("The runes crack and fall to dust.");
                Console.WriteLine("The Veil roars triumphant as darkness devours the land.");
                Console.WriteLine("All is lost.");
                Console.WriteLine("=== DARK ENDING ===");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit the game.");
            Console.ReadKey();
            Environment.Exit(0);
        }
        public static async Task StartManaRegenerationAsync(Player player, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), token);

                if (player.Mana < player.MaxMana)
                {
                    player.Mana += 1;
                    if (player.Mana > player.MaxMana)
                        player.Mana = player.MaxMana;
                }
            }
        }
        
        static bool SolveSpireRiddle(Player player)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("An ancient inscription glows faintly on the obsidian wall...");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("\"What type of cheese is made backward?");
            Console.WriteLine();
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer == "Edam")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The wall slides open, revealing an Ancient Key!");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The runes fade into darkness.");
                Console.ResetColor();
                return false;
            }
        }
        static bool StartSecretBossBattle(Player player)
        {
            Enemy secretBoss = new Enemy(
                "Warden of the Veil",
                200,
                25,
                new[]
                {
                    "   ██████",
                    "  ██    ██",
                    " ██  ▓▓  ██",
                    "██        ██",
                    " ██  ▓▓  ██",
                    "  ██    ██",
                    "   ██████"
                });

            StartBattle(player, secretBoss);

            if (player.HP > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Warden falls. You grasp the Veil Sigil, pulsing with ancient power.");
                Console.ResetColor();
                player.Inventory.Add("Veil Sigil");
                Console.WriteLine("+200 Max HP, +60 Max Mana!");

                player.MaxHP += 200;
                player.HP = player.MaxHP;
                player.MaxMana += 60;
                player.Mana = player.MaxMana;

                Console.ReadKey();
                return true;
            }
            else
            {
                Console.WriteLine("You were defeated.");
                Console.ReadKey();
                return false;
            }
        }
    }
}

