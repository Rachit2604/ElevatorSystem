using System;
using System.Collections.Generic;
using System.Linq;

public class ElevatorSystem
{
    public List<Elevator> Elevators { get; }

    public ElevatorSystem(int numberOfElevators)
    {
        Elevators = new List<Elevator>();
        for (int i = 1; i <= numberOfElevators; i++)
        {
            Elevators.Add(new Elevator(i));
        }
    }

    public void Step()
    {
        foreach (var elevator in Elevators)
        {
            elevator.Move();
        }
    }

    public void PrintStatus()
    {
        Step();
        foreach (var elevator in Elevators)
        {
            Console.WriteLine(elevator.GetStatus());
        }
        Console.WriteLine("--------------------------------------------------");
    }

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

    private Elevator FindBestElevator(int requestFloor, Direction direction)
    {
        Elevator bestElevator = null;
        int minDistance = int.MaxValue;

        foreach (var elevator in Elevators)
        {
            int distance = Math.Abs(elevator.CurrentFloor - requestFloor);

            if (elevator.Direction == Direction.Idle)
            {
                if (distance < minDistance)
                {
                    bestElevator = elevator;
                    minDistance = distance;
                }
            }
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

        if (bestElevator == null)
        {
            bestElevator = Elevators.OrderBy(e => e.UpQueue.Count + e.DownQueue.Count).First();
        }

        return bestElevator;
    }

}