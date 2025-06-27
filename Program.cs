using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        ElevatorSystem system = new ElevatorSystem(4);

        while (true)
        {
            Console.WriteLine("Enter a request in the format: [floor] [up/down] or type 'status' or 'exit'");
            Console.Write("Your input: ");
            string input = Console.ReadLine()?.ToLower()?.Trim();

            if (input == "exit") break;

            if (input == "status")
            {
                system.PrintStatus();
                continue;
            }

            string[] parts = input.Split(' ');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int floor) || 
                (parts[1] != "up" && parts[1] != "down"))
            {
                Console.WriteLine("❌ Invalid input. Example: '3 up'");
                continue;
            }

            Direction direction = parts[1] == "up" ? Direction.Up : Direction.Down;

            Console.Write("Enter destination floor for passenger: ");
            string destInput = Console.ReadLine()?.Trim() ?? "";
            List<int> destinations = destInput.Split(',')
                                              .Select(s => int.TryParse(s.Trim(), out int f) ? f : -1)
                                              .Where(f => f >= 0)
                                              .ToList();

            if (!destinations.Any())
            {
                Console.WriteLine("❌ No valid destination floors entered.");
                continue;
            }

            system.HandleRequest(floor, direction, destinations);
            system.PrintStatus();
        }
    }
}