using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles user input and assigns elevator requests to the ElevatorSystem.
/// </summary>
class Program
{
    /// <summary>
    /// Main method that runs the interactive elevator system loop.
    /// </summary>
    static void Main()
    {
        // Initialize elevator system with 4 elevators
        ElevatorSystem system = new ElevatorSystem(4);

        while (true)
        {
            Console.WriteLine("Enter a request in the format: [floor] [up/down] or type 'status' or 'exit'");
            Console.Write("Your input: ");
            string input = Console.ReadLine()?.ToLower()?.Trim();

            // Exit the loop if user types 'exit'
            if (input == "exit") break;

            // Print status of all elevators if requested
            if (input == "status")
            {
                system.PrintStatus();
                continue;
            }

            // Validate user input format
            string[] parts = input.Split(' ');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int floor) ||
                (parts[1] != "up" && parts[1] != "down"))
            {
                Console.WriteLine("❌ Invalid input. Example: '3 up'");
                continue;
            }

            // Determine direction from input
            Direction direction = parts[1] == "up" ? Direction.Up : Direction.Down;

            // Ask user for destination floor(s)
            Console.Write("Enter destination floor for passenger: ");
            string destInput = Console.ReadLine()?.Trim() ?? "";

            // Parse destination floors into a list
            List<int> destinations = destInput.Split(',')
                                              .Select(s => int.TryParse(s.Trim(), out int f) ? f : -1)
                                              .Where(f => f >= 0)
                                              .ToList();

            if (!destinations.Any())
            {
                Console.WriteLine("❌ No valid destination floors entered.");
                continue;
            }

            // Handle request by assigning elevator and adding destinations
            system.HandleRequest(floor, direction, destinations);

            // Optionally print status after each request
            system.PrintStatus();
        }
    }
}