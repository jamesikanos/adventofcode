
using System.Drawing;

var lines = File.ReadAllLines("input.txt");

var rows = new List<List<char>>();

var startingPoint = Point.Empty;
var finalPoint = Point.Empty;

foreach (var line in lines)
{
    var newRow = new List<char>();
    rows.Add(newRow);

    foreach (var c in line)
    {
        newRow.Add(c);

        if (c == 'S')
        {
            startingPoint = new Point(newRow.Count - 1, rows.Count - 1);
        }

        if (c == 'E')
        {
            finalPoint = new Point(newRow.Count - 1, rows.Count - 1);
        }
    }
}

Console.WriteLine("Starting Point: {0}", startingPoint);
Console.WriteLine("Final Point: {0}", finalPoint);

var allStartingPoints = new List<Point>();

for (int r = 0; r < rows.Count; r++)
{
    for (int c = 0; c < rows[0].Count; c++)
    {
        if (PointToChar(new Point(c, r)) == 'a')
        {
            allStartingPoints.Add(new Point(c, r));
        }
    }
}

var shortestStep = int.MaxValue;

foreach (var newStartingPoint in allStartingPoints)
{
    var solution = FindSolution(newStartingPoint);

    if (solution == null)
    {
        continue;
    }

    var solutionPoints = new List<Point>();

    var stepCount = 0;
    while (solution.Parent != null)
    {
        solutionPoints.Add(solution.ThisPoint);

        stepCount++;
        solution = solution.Parent;
    }

    shortestStep = Math.Min(shortestStep, stepCount);
}

Console.WriteLine("Shortest Step: {0}", shortestStep);

char PointToChar(Point p)
{
    return rows[p.Y][p.X];
}

Point? LookDown(Point thisPoint)
{
    if (thisPoint.Y >= (rows.Count - 1))
    {
        return null;
    }

    return new Point(thisPoint.X, thisPoint.Y + 1);
}

Point? LookRight(Point thisPoint)
{
    if (thisPoint.X >= rows[0].Count - 1)
    {
        return null;
    }

    return new Point(thisPoint.X + 1, thisPoint.Y);
}

Point? LookLeft(Point thisPoint)
{
    if (thisPoint.X <= 0)
    {
        return null;
    }

    return new Point(thisPoint.X - 1, thisPoint.Y);
}

Point? LookUp(Point thisPoint)
{
    if (thisPoint.Y <= 0)
    {
        return null;
    }

    return new Point(thisPoint.X, thisPoint.Y - 1);
}

void MarkPoint(Point p)
{
    Console.SetCursorPosition(p.X, p.Y);
    Console.Write('*');
}

void DrawGrid(List<Point> points)
{
    for (int r = 0; r < rows.Count; r++)
    {
        for (int c = 0; c < rows[0].Count; c++)
        {
            var p = new Point(c, r);

            if (points.Contains(p))
            {
                Console.Write("*");
            }
            else
            {
                Console.Write(PointToChar(p));
            }
        }

        Console.WriteLine();
    }
}

LinkedPoint? FindSolution(Point startingPoint)
{
    var queue = new Queue<LinkedPoint>();
    queue.Enqueue(new LinkedPoint { ThisPoint = startingPoint, Parent = null });

    var explored = new List<Point>();

    LinkedPoint? solution = null;

    while (queue.Any())
    {
        var item = queue.Dequeue();

        if (item.ThisPoint == finalPoint)
        {
            // GOAL
            solution = item;
            break;
        }

        var directions = new[] {
            () => LookDown(item.ThisPoint),
            () => LookLeft(item.ThisPoint),
            () => LookUp(item.ThisPoint),
            () => LookRight(item.ThisPoint)
        };

        foreach (var direction in directions)
        {
            var result = direction();

            if (result == null) continue;

            var src = PointToChar(item.ThisPoint);
            var dst = PointToChar(result.Value);

            if (src == 'S') src = 'a';
            if (dst == 'E') dst = 'z';

            var diff = (dst - src);

            // Console.WriteLine("From: {0} to: {1} - Diff: {2}", src, dst, diff);

            if (diff > 1) continue;

            if (explored.Contains(result.Value)) continue;

            explored.Add(result.Value);

            var linkedPoint = new LinkedPoint
            {
                Parent = item,
                ThisPoint = result.Value
            };

            queue.Enqueue(linkedPoint);
        }
    }

    return solution;
}

class LinkedPoint
{
    public LinkedPoint? Parent { get; set; }

    public Point ThisPoint { get; set; }
}
