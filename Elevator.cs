using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/// <summary>
/// Represents a single elevator with floor queues, direction, and movement logic.
/// </summary>
public class Elevator
{
    public int Id { get; }                        // Unique identifier for the elevator
    public int CurrentFloor { get; set; }         // Current floor the elevator is on
    public Direction Direction { get; set; }      // Current movement direction (Up, Down, Idle)

    public SortedSet<int> UpQueue { get; } = new SortedSet<int>();    // Floors to visit going up
    public SortedSet<int> DownQueue { get; } = new SortedSet<int>();  // Floors to visit going down

    /// <summary>
    /// Constructor to initialize elevator with an ID and set it to Idle at floor 0.
    /// </summary>
    public Elevator(int id)
    {
        Id = id;
        CurrentFloor = 0;
        Direction = Direction.Idle;
    }

    /// <summary>
    /// Adds a pickup request at a specific floor and sets direction if currently idle.
    /// </summary>
    /// <param name="floor">Pickup floor</param>
    /// <param name="direction">Requested direction</param>
    public void AddRequest(int floor, Direction direction)
    {
        if (direction == Direction.Up)
        {
            UpQueue.Add(floor);
            if (Direction == Direction.Idle) Direction = Direction.Up;
        }
        else
        {
            DownQueue.Add(floor);
            if (Direction == Direction.Idle) Direction = Direction.Down;
        }
    }

    /// <summary>
    /// Adds a destination floor for a passenger already inside the elevator.
    /// </summary>
    /// <param name="floor">Destination floor</param>
    public void AddDestination(int floor)
    {
        if (floor > CurrentFloor)
            UpQueue.Add(floor);
        else
            DownQueue.Add(floor);
    }

    /// <summary>
    /// Moves the elevator one step (one floor at a time) based on queue direction.
    /// Simulates 10-second travel delay per floor.
    /// </summary>
    public void Move()
    {
        // If elevator is idle, determine direction based on queues
        if (Direction == Direction.Idle)
        {
            if (UpQueue.Any())
                Direction = Direction.Up;
            else if (DownQueue.Any())
                Direction = Direction.Down;
            else
                return; // Nothing to do
        }

        if (Direction == Direction.Up)
        {
            // Get the closest upcoming stop
            int? nextStop = UpQueue.FirstOrDefault();

            // If UpQueue is empty but DownQueue isn't, change next stop
            if (!UpQueue.Any() && DownQueue.Any())
                nextStop = DownQueue.Max();

            if (nextStop != null)
            {
                // Move one floor toward the next stop
                if (CurrentFloor < nextStop)
                    CurrentFloor++;
                else if (CurrentFloor > nextStop)
                    CurrentFloor--;

                // Stop and clear any floors matching current floor
                if (UpQueue.Contains(CurrentFloor))
                {
                    Console.WriteLine($"Elevator {Id} stopping at floor {CurrentFloor}");
                    UpQueue.Remove(CurrentFloor);
                }

                if (DownQueue.Contains(CurrentFloor))
                {
                    Console.WriteLine($"Elevator {Id} stopping at floor {CurrentFloor}");
                    DownQueue.Remove(CurrentFloor);
                }
            }

            // Decide next direction
            if (!UpQueue.Any() && DownQueue.Any())
                Direction = Direction.Down;
            else if (!UpQueue.Any() && !DownQueue.Any())
                Direction = Direction.Idle;
        }
        else if (Direction == Direction.Down)
        {
            // Get the closest stop going down
            int? nextStop = DownQueue.LastOrDefault();

            // If DownQueue is empty but UpQueue isn't, change next stop
            if (!DownQueue.Any() && UpQueue.Any())
                nextStop = UpQueue.Min();

            if (nextStop != null)
            {
                // Move one floor toward next stop
                if (CurrentFloor < nextStop)
                    CurrentFloor++;
                else if (CurrentFloor > nextStop)
                    CurrentFloor--;

                // Stop and clear any floors matching current floor
                if (DownQueue.Contains(CurrentFloor))
                {
                    Console.WriteLine($"Elevator {Id} stopping at floor {CurrentFloor}");
                    DownQueue.Remove(CurrentFloor);
                }

                if (UpQueue.Contains(CurrentFloor))
                {
                    Console.WriteLine($"Elevator {Id} stopping at floor {CurrentFloor}");
                    UpQueue.Remove(CurrentFloor);
                }
            }

            // Decide next direction
            if (!DownQueue.Any() && UpQueue.Any())
                Direction = Direction.Up;
            else if (!DownQueue.Any() && !UpQueue.Any())
                Direction = Direction.Idle;
        }

        // Simulate time delay: 10 seconds per floor
        Thread.Sleep(10000);
    }

    /// <summary>
    /// Returns the status of the elevator including current floor, direction,
    /// next stop, and queue contents.
    /// </summary>
    /// <returns>Status string</returns>
    public string GetStatus()
    {
        string nextStop = "None";

        if (Direction == Direction.Up && UpQueue.Any())
            nextStop = UpQueue.Min().ToString();
        else if (Direction == Direction.Down && DownQueue.Any())
            nextStop = DownQueue.Max().ToString();

        return $"Elevator {Id} is at floor {CurrentFloor}, going {Direction}, " +
               $"Next stop: {nextStop}, UpQueue: [{string.Join(",", UpQueue)}], " +
               $"DownQueue: [{string.Join(",", DownQueue)}]";
    }
}
