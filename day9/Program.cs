using System.Drawing;

var lines = File.ReadAllLines("input.txt");

var hPosition = Point.Empty;
var tPositions = Enumerable.Range(0, 9).Select(_ => Point.Empty).ToArray();

var visitedLocations = new List<Point>();


foreach (var line in lines)
{
    (char direction, int steps) = ParseLine(line);

    for (int i = 0; i < steps; i++)
    {
        switch (direction)
        {
            case 'D':
                hPosition.Y++;
                break;
            case 'U':
                hPosition.Y--;
                break;
            case 'R':
                hPosition.X++;
                break;
            case 'L':
                hPosition.X--;
                break;
        }

        Console.WriteLine("===");

        Console.WriteLine("Head: {0} . {1}", direction, hPosition);

        // For each tail...
        for (int tailIndex = 0; tailIndex < tPositions.Length; tailIndex++)
        {
            // Chase the head
            if (tailIndex == 0)
            {
                CalculateNextTail(hPosition, ref tPositions[tailIndex]);
            }
            // Or; chase the previous tail
            else
            {
                CalculateNextTail(tPositions[tailIndex - 1], ref tPositions[tailIndex]);
            }

            Console.WriteLine("Tail{0}: {1}", tailIndex + 1, tPositions[tailIndex]);
        }

        if (!visitedLocations.Contains(tPositions.Last()))
        {
            visitedLocations.Add(tPositions.Last());

            Console.WriteLine("** New Tail Location **");
        }
    }
}

Console.WriteLine("Visited Locations: {0}", visitedLocations.Count);

(char Direction, int Steps) ParseLine(string line)
{
    var parts = line.Split(" ");

    return (parts[0][0], int.Parse(parts[1]));
}

void CalculateNextTail(Point topPosition, ref Point tailPosition)
{
    // Calculate the distance between the Head and the Tail
    var xDistance = topPosition.X - tailPosition.X;
    var yDistance = topPosition.Y - tailPosition.Y;

    // No distance, do nothing
    if (xDistance == 0 && yDistance == 0)
    {
        return;
    }

    // If we need to move diagonally, chase the head
    if (Math.Abs(xDistance) == 2 && Math.Abs(yDistance) == 2)
    {
        if (xDistance > 0)
            tailPosition.X++;
        else
            tailPosition.X--;

        if (yDistance > 0)
            tailPosition.Y++;
        else
            tailPosition.Y--;

        return;
    }

    if (yDistance == 2)
    {
        tailPosition.Y++;
        tailPosition.X = topPosition.X;
    }
    else if (yDistance == -2)
    {
        tailPosition.Y--;
        tailPosition.X = topPosition.X;
    }
    else if (xDistance == 2)
    {
        tailPosition.X++;
        tailPosition.Y = topPosition.Y;
    }
    else if (xDistance == -2)
    {
        tailPosition.X--;
        tailPosition.Y = topPosition.Y;
    }
}