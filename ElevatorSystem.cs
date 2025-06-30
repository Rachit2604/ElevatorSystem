using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the elevator system including all elevators, 
/// assigning requests, and updating elevator movement.
/// </summary>
public class ElevatorSystem
{
    public List<Elevator> Elevators { get; }

    /// <summary>
    /// Initializes the system with a given number of elevators.
    /// </summary>
    /// <param name="numberOfElevators">Number of elevators to create and manage.</param>
    public ElevatorSystem(int numberOfElevators)
    {
        Elevators = new List<Elevator>();
        for (int i = 1; i <= numberOfElevators; i++)
        {
            Elevators.Add(new Elevator(i));
        }
    }

    /// <summary>
    /// Simulates a single step where each elevator may move.
    /// </summary>
    public void Step()
    {
        foreach (var elevator in Elevators)
        {
            elevator.Move();
        }
    }

    /// <summary>
    /// Prints the status of all elevators and calls Step to simulate progress.
    /// </summary>
    public void PrintStatus()
    {
        Step(); // Update movement before showing status
        foreach (var elevator in Elevators)
        {
            Console.WriteLine(elevator.GetStatus());
        }
        Console.WriteLine("--------------------------------------------------");
    }

    /// <summary>
    /// Handles an elevator request from a floor, assigns the best elevator,
    /// and adds destination floors for the passenger.
    /// </summary>
    /// <param name="floor">Requesting floor</param>
    /// <param name="direction">Direction of travel (Up or Down)</param>
    /// <param name="destinations">List of destination floors</param>
    public void HandleRequest(int floor, Direction direction, List<int> destinations)
    {
        var elevator = FindBestElevator(floor, direction);
        elevator.AddRequest(floor, direction);

        foreach (int dest in destinations)
        {
            elevator.AddDestination(dest);
        }

        Console.WriteLine($"? Request added: Floor {floor} going {direction}");
    }

    /// <summary>
    /// Finds the best elevator to handle a request based on current state, 
    /// direction, and proximity to the requesting floor.
    /// </summary>
    /// <param name="requestFloor">Floor where request originated</param>
    /// <param name="direction">Requested direction</param>
    /// <returns>Best-suited Elevator instance</returns>
    private Elevator FindBestElevator(int requestFloor, Direction direction)
    {
        Elevator bestElevator = null;
        int minDistance = int.MaxValue;

        foreach (var elevator in Elevators)
        {
            int distance = Math.Abs(elevator.CurrentFloor - requestFloor);

            // Prefer idle elevators that are closest
            if (elevator.Direction == Direction.Idle)
            {
                if (distance < minDistance)
                {
                    bestElevator = elevator;
                    minDistance = distance;
                }
            }
            // Prefer elevators already moving in the same direction
            else if (elevator.Direction == direction)
            {
                bool willPassFloor =
                    (direction == Direction.Up && elevator.CurrentFloor <= requestFloor) ||
                    (direction == Direction.Down && elevator.CurrentFloor >= requestFloor);

                if (willPassFloor && distance < minDistance)
                {
                    bestElevator = elevator;
                    minDistance = distance;
                }
            }
        }

        // Fallback: choose the least busy elevator (smallest queue load)
        if (bestElevator == null)
        {
            bestElevator = Elevators
                .OrderBy(e => e.UpQueue.Count + e.DownQueue.Count)
                .First();
        }

        return bestElevator;
    }
}
