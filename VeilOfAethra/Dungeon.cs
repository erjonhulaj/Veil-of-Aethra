using System;
using System.Collections.Generic;

namespace VeilOfAethra
{
    public class Dungeon
    {
        public char[,] Grid { get; private set; }
        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }
        private Random rnd = new Random();
        public bool ExitReached { get; private set; } = false;

        private List<(int X, int Y)> MonsterPositions = new List<(int X, int Y)>();

        public Dungeon(int width = 20, int height = 15)
        {
            Grid = new char[height, width];
            
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Grid[y, x] = '.';

            // Create walls
            for (int i = 0; i < (width * height) / 4; i++)
            {
                int wx = rnd.Next(0, width);
                int wy = rnd.Next(0, height);
                Grid[wy, wx] = '#';
            }

            // Treasures
            for (int i = 0; i < 5; i++)
            {
                int tx = rnd.Next(0, width);
                int ty = rnd.Next(0, height);
                if (Grid[ty, tx] == '.')
                    Grid[ty, tx] = '$';
            }

            // More monsters
            for (int i = 0; i < 15; i++)
            {
                int ex = rnd.Next(0, width);
                int ey = rnd.Next(0, height);
                if (Grid[ey, ex] == '.')
                {
                    Grid[ey, ex] = 'E';
                    MonsterPositions.Add((ex, ey));
                }
            }

            // Exit
            int exitX = rnd.Next(0, width);
            int exitY = rnd.Next(0, height);
            Grid[exitY, exitX] = 'X';
            
            // Player start
            PlayerX = 0;
            PlayerY = 0;
            Grid[PlayerY, PlayerX] = '@';
        }

        public void Display()
        {
            Console.Clear();
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Console.Write(Grid[y, x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Find the Exit (X).");
        }

        
        // Handles player movement.
        // Returns: treasure, enemy, exit, moved, blocked, out-of-bounds, invalid
        public string Move(char direction)
        {
            int newX = PlayerX;
            int newY = PlayerY;

            switch (direction)
            {
                case 'w': newY--; break;
                case 's': newY++; break;
                case 'a': newX--; break;
                case 'd': newX++; break;
                default: return "invalid";
            }

            if (newX < 0 || newX >= Grid.GetLength(1) || newY < 0 || newY >= Grid.GetLength(0))
                return "out-of-bounds";

            if (Grid[newY, newX] == '#')
                return "blocked";

            string result = "moved";

            if (Grid[newY, newX] == '$')
            {
                result = "treasure";
                // Remove treasure
                Grid[newY, newX] = '.';
            }

            if (Grid[newY, newX] == 'E')
            {
                result = "enemy";
            }

            if (Grid[newY, newX] == 'X')
            {
                ExitReached = true;
                result = "exit";
            }

            // Move player
            Grid[PlayerY, PlayerX] = '.';
            PlayerX = newX;
            PlayerY = newY;
            Grid[PlayerY, PlayerX] = '@';

            // Move monster 
            MoveMonsters();

            return result;
        }

        
        // Moves monsters randomly one tile.
        private void MoveMonsters()
        {
            List<(int X, int Y)> newPositions = new List<(int X, int Y)>();

            foreach (var (x, y) in MonsterPositions)
            {
                int newX = x;
                int newY = y;

                int dir = rnd.Next(0, 4);
                switch (dir)
                {
                    case 0: newY--; break;
                    case 1: newY++; break;
                    case 2: newX--; break;
                    case 3: newX++; break;
                }

                if (newX < 0 || newX >= Grid.GetLength(1) || newY < 0 || newY >= Grid.GetLength(0))
                {
                    newPositions.Add((x, y));
                    continue;
                }

                if (Grid[newY, newX] == '#' || Grid[newY, newX] == '$' || Grid[newY, newX] == 'X' ||
                    Grid[newY, newX] == 'E' || Grid[newY, newX] == '@')
                {
                    newPositions.Add((x, y));
                    continue;
                }

                // Count monster
                int adjacentCount = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = newX + dx;
                        int ny = newY + dy;
                        if (nx >= 0 && ny >= 0 && nx < Grid.GetLength(1) && ny < Grid.GetLength(0))
                        {
                            if (Grid[ny, nx] == 'E')
                                adjacentCount++;
                        }
                    }
                }

                if (adjacentCount > 3)
                {
                    newPositions.Add((x, y));
                    continue;
                }

                // Monster bewegen
                Grid[y, x] = '.';
                Grid[newY, newX] = 'E';
                newPositions.Add((newX, newY));
            }

            MonsterPositions = newPositions;
        }
    }
}
