
using System.Drawing;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var rockPoints = new List<Point>();
var sandPoints = new List<Point>();

var pattern = new Regex("(?<x>[0-9]+)\\,(?<y>[0-9]+)");

foreach (var line in lines)
{
    Console.WriteLine(line);

    var match = pattern.Matches(line);

    Point? previousPoint = null;

    foreach (var m in match.Cast<Match>())
    {
        var x = int.Parse(m.Groups["x"].Value);
        var y = int.Parse(m.Groups["y"].Value);
        var thisPoint = new Point(x, y);


        AddRockPoint(thisPoint);

        if (previousPoint == null)
        {
            previousPoint = thisPoint;
            continue;
        }

        // Measure the drawing of the line
        Point drawingPoint = previousPoint.Value;

        // Console.WriteLine("Drawing from {0} to {1}", drawingPoint, thisPoint);

        // Measure towards the new point
        while (drawingPoint != thisPoint)
        {
            var diffX = drawingPoint.X - thisPoint.X;
            var diffY = drawingPoint.Y - thisPoint.Y;

            if (diffX < 0)
            {
                drawingPoint.X += 1;
            }
            else if (diffX > 0)
            {
                drawingPoint.X -= 1;
            }

            if (diffY < 0)
            {
                drawingPoint.Y += 1;
            }
            else if (diffY > 0)
            {
                drawingPoint.Y -= 1;
            }

            AddRockPoint(drawingPoint);

            // Console.WriteLine(" . {0}", drawingPoint);
        }

        previousPoint = thisPoint;
    }
}

// Draw the Rock Floor
var minX = rockPoints.OrderBy(j => j.X).First().X - 150;
var maxX = rockPoints.OrderBy(j => j.X).Last().X + 150;
var floorY = rockPoints.OrderBy(j => j.Y).Last().Y + 2;

for (int floorX = minX; floorX < maxX; floorX++)
{
    rockPoints.Add(new Point(floorX, floorY));
}

// Work out the minimum y position
// (if it goes below this then it's falling into the abyss)
var isFallingIndefinitely = false;

// Play the Game
while (!isFallingIndefinitely)
{
    var maxY = rockPoints.Concat(sandPoints).OrderByDescending(j => j.Y).First().Y;
    // Console.WriteLine("New Sand: Max Y {0}", maxY);

    var sandPoint = new Point(500, 0);

    while (!isFallingIndefinitely)
    {
        isFallingIndefinitely = sandPoint.Y >= maxY;
        if (isFallingIndefinitely) break;

        // Can fall down?
        var dirDown = new Point(sandPoint.X, sandPoint.Y + 1);
        if (!rockPoints.Contains(dirDown) && !sandPoints.Contains(dirDown))
        {
            // Console.WriteLine("Falling Down");

            sandPoint.Y += 1;
            continue;
        }

        // Can go left?
        var dirLeft = new Point(sandPoint.X - 1, sandPoint.Y + 1);
        if (!rockPoints.Contains(dirLeft) && !sandPoints.Contains(dirLeft))
        {
            // Console.WriteLine("Going Left");

            sandPoint.X -= 1;
            sandPoint.Y += 1;
            continue;
        }

        // Can go right?
        var dirRight = new Point(sandPoint.X + 1, sandPoint.Y + 1);
        if (!rockPoints.Contains(dirRight) && !sandPoints.Contains(dirRight))
        {
            // Console.WriteLine("Going Right");

            sandPoint.X += 1;
            sandPoint.Y += 1;
            continue;
        }

        // Console.WriteLine("Settling at: {0}", sandPoint);

        if (sandPoints.Contains(sandPoint))
        {
            isFallingIndefinitely = true;
            break;
        }

        sandPoints.Add(sandPoint);
        break;
    }
}

DrawCurrentGrid();

Console.WriteLine("Answer: {0}", sandPoints.Count);

void AddRockPoint(Point p)
{
    if (!rockPoints.Contains(p))
        rockPoints.Add(p);
}

void DrawCurrentGrid()
{
    var allPoints = rockPoints.Concat(sandPoints).ToList();

    var minX = allPoints.OrderBy(j => j.X).First().X - 2;
    var minY = allPoints.OrderBy(j => j.Y).First().Y - 2;

    var maxX = allPoints.OrderBy(j => j.X).Last().X + 2;
    var maxY = allPoints.OrderBy(j => j.Y).Last().Y + 2;

    for (int y = minY; y < maxY; y++)
    {
        for (int x = minX; x < maxX; x++)
        {
            var here = new Point(x, y);

            if (rockPoints.Contains(here))
            {
                Console.Write("#");
            }
            else if (sandPoints.Contains(here))
            {
                Console.Write("O");
            }
            else
            {
                Console.Write(".");
            }
        }

        Console.WriteLine();
    }
}
