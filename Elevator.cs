using System;
using System.Collections.Generic;
using System.Linq;

public class Elevator
{
    public int Id { get; }
    public int CurrentFloor { get; set; }
    public Direction Direction { get; set; }
    public SortedSet<int> UpQueue { get; } = new SortedSet<int>();
    public SortedSet<int> DownQueue { get; } = new SortedSet<int>();

    public Elevator(int id)
    {
        Id = id;
        CurrentFloor = 0;
        Direction = Direction.Idle;
    }

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

    public void AddDestination(int floor)
    {
        if (floor > CurrentFloor)
            UpQueue.Add(floor);
        else
            DownQueue.Add(floor);
    }

    public void Move()
    {
        // If the elevator is idle but has some request, set direction
        if (Direction == Direction.Idle)
        {
            if (UpQueue.Any())
                Direction = Direction.Up;
            else if (DownQueue.Any())
                Direction = Direction.Down;
            else
                return;
        }

        if (Direction == Direction.Up)
        {
            int? nextStop = UpQueue.FirstOrDefault();
            if (!UpQueue.Any() && DownQueue.Any())
                nextStop = DownQueue.Max(); 

            if (nextStop != null)
            {
                if (CurrentFloor < nextStop)
                    CurrentFloor++;
                else if (CurrentFloor > nextStop)
                    CurrentFloor--; 

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

            // Update direction after reaching target
            if (!UpQueue.Any() && DownQueue.Any())
                Direction = Direction.Down;
            else if (!UpQueue.Any() && !DownQueue.Any())
                Direction = Direction.Idle;
        }
        else if (Direction == Direction.Down)
        {
            int? nextStop = DownQueue.LastOrDefault();
            if (!DownQueue.Any() && UpQueue.Any())
                nextStop = UpQueue.Min();

            if (nextStop != null)
            {
                if (CurrentFloor < nextStop)
                    CurrentFloor++;
                else if (CurrentFloor > nextStop)
                    CurrentFloor--;

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

            if (!DownQueue.Any() && UpQueue.Any())
                Direction = Direction.Up;
            else if (!DownQueue.Any() && !UpQueue.Any())
                Direction = Direction.Idle;
        }
    }

    public string GetStatus()
    {
        string nextStop = "None";
        if (Direction == Direction.Up && UpQueue.Any())
            nextStop = UpQueue.Min().ToString();
        else if (Direction == Direction.Down && DownQueue.Any())
            nextStop = DownQueue.Max().ToString();

        return $"Elevator {Id} is at floor {CurrentFloor}, going {Direction}, Next stop: {nextStop}, UpQueue: [{string.Join(",", UpQueue)}], DownQueue: [{string.Join(",", DownQueue)}]";
    }
}