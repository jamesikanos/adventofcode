
using System.Diagnostics.CodeAnalysis;

public static class Program
{
    static string[] lines = Array.Empty<string>();

    static List<Point> partOfMap = new List<Point>();

    public static void Main()
    {
        // Convert into a grid
        lines = File.ReadAllLines("sample4.txt");

        Console.WriteLine(CharAtGrid(new Point(1, 1)));

        var currentPoint = FindStartingPoint(lines);
        partOfMap.Add(currentPoint);

        var currentChar = 'S';
        var direction = 'U';

        var steps = 0;

        do
        {
            Console.WriteLine($"At Point: {currentPoint} with char: {currentChar} moving {direction}");

            if (currentChar == 'S')
            {
                var r = FindStartingLocationNext(currentPoint);
                currentPoint = r.newPoint;
                partOfMap.Add(currentPoint);
                direction = r.direction;
                currentChar = CharAtGrid(currentPoint);
                continue;
            }

            switch (currentChar)
            {
                case '|':
                    currentPoint = direction == 'U' ? currentPoint.NavigateUp() : currentPoint.NavigateDown();
                    break;
                case '-':
                    currentPoint = direction == 'R' ? currentPoint.NavigateRight() : currentPoint.NavigateLeft();
                    break;
                case 'L':
                    if (direction == 'L')
                    {
                        currentPoint = currentPoint.NavigateUp();
                        direction = 'U';
                    }
                    else if (direction == 'D')
                    {
                        currentPoint = currentPoint.NavigateRight();
                        direction = 'R';
                    }

                    break;

                case 'J':
                    if (direction == 'R')
                    {
                        currentPoint = currentPoint.NavigateUp();
                        direction = 'U';
                    }
                    else if (direction == 'D')
                    {
                        currentPoint = currentPoint.NavigateLeft();
                        direction = 'L';
                    }

                    break;

                case '7':
                    if (direction == 'R')
                    {
                        currentPoint = currentPoint.NavigateDown();
                        direction = 'D';
                    }
                    else if (direction == 'U')
                    {
                        currentPoint = currentPoint.NavigateLeft();
                        direction = 'L';
                    }

                    break;

                case 'F':
                    if (direction == 'L')
                    {
                        currentPoint = currentPoint.NavigateDown();
                        direction = 'D';
                    }
                    else if (direction == 'U')
                    {
                        currentPoint = currentPoint.NavigateRight();
                        direction = 'R';
                    }

                    break;

                default:
                    throw new Exception();
            }

            currentChar = CharAtGrid(currentPoint);
            partOfMap.Add(currentPoint);

            steps++;
        }
        while (currentChar != 'S');

        Console.WriteLine((steps + 1) / 2);

        // Traverse the entire maze starting from the outside until you find a .
        // Then convert each . to an 0

        FindEnclosed();

        // Print the grid

        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }

        // Count all I
        Console.WriteLine(lines.Aggregate(0, (acc, val) => acc += val.Count(i => i =='I')));
    }

    static void FindEnclosed()
    {
        var SetToStar = (Point p) =>
        {
            var chars = lines[p.Y].ToCharArray();
            chars[p.X] = '*';
            lines[p.Y] = new string(chars);
        };

        var pointsToTest = new List<Point>();

        for (var y = 0; y < lines.Length; y++)
        {
            var isIn = false;

            for (var x = 0; x < lines[0].Length; x++)
            {
                var p = new Point(x, y);
                var isPartOfMap = partOfMap.Contains(p);
                var c = CharAtGrid(p);

                if (isPartOfMap)
                {
                    if (isIn)
                    {
                        if (c == '|' || c == 'J' || c == '7')
                        {
                            isIn = false;
                        }
                    }
                }

                if (isIn && !isPartOfMap)
                {
                    SetToStar(p);
                    pointsToTest.Add(p);
                }
            }
        }

        return;

        foreach (var pointToTest in pointsToTest)
        {
            Console.WriteLine($"Testing Point {pointToTest} with char {CharAtGrid(pointToTest)}");

            var hasReachedOutside = CanReachOutside(pointToTest, new List<Point>());

            var chars = lines[pointToTest.Y].ToCharArray();
            chars[pointToTest.X] = hasReachedOutside ? 'O' : 'I';
            lines[pointToTest.Y] = new string(chars);

            // Console.WriteLine($"Can Reach Outside: {hasReachedOutside}");
        }
    }

    static bool CanReachOutside(Point p, List<Point> triedPoints)
    {
        triedPoints.Add(p);

        // If the char is outside, return false
        if (CharAtGrid(p) == '=')
        {
            return true;
        }

        var points = new[]
        {
            p.NavigateLeft(),
            p.NavigateDown(),
            p.NavigateUp(),
            p.NavigateRight()
        };

        foreach (var newPoint in points)
        {
            // If the point is part of the map
            if (partOfMap.Contains(newPoint))
            {
                continue;
            }

            // If already tried this point
            if (triedPoints.Contains(newPoint))
            {
                continue;
            }

            if (CanReachOutside(newPoint, triedPoints))
            {
                return true;
            }
        }

        return false;
    }

    static char CharAtGrid(Point p)
    {
        try
        {
            return lines[p.Y][p.X];
        }
        catch (IndexOutOfRangeException)
        {
            return '=';
        }
    }

    static Point FindStartingPoint(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == 'S') return new Point(x, y);
            }
        }

        throw new Exception();
    }

    static (Point newPoint, char direction) FindStartingLocationNext(Point startingLocation)
    {
        // Go left
        var np = startingLocation.NavigateLeft();

        switch (CharAtGrid(np))
        {
            case '-':
            case 'F':
            case 'L':
                return (np, 'L');
        }

        // Go right
        np = startingLocation.NavigateRight();

        switch (CharAtGrid(np))
        {
            case '-':
            case '7':
            case 'J':
                return (np, 'R');
        }

        // Go Down
        np = startingLocation.NavigateDown();

        switch (CharAtGrid(np))
        {
            case '|':
            case 'J':
            case 'L':
                return (np, 'D');
        }

        // Must be up
        return (startingLocation.NavigateUp(), 'U');
    }
}

class Point : IEquatable<Point>, IEqualityComparer<Point>
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point NavigateLeft()
    {
        return new Point(X - 1, Y);
    }

    public Point NavigateRight()
    {
        return new Point(X + 1, Y);
    }

    public Point NavigateUp()
    {
        return new Point(X, Y - 1);
    }

    public Point NavigateDown()
    {
        return new Point(X, Y + 1);
    }

    public override string ToString()
    {
        return $"{X}:{Y}";
    }

    public bool Equals(Point? other)
    {
        return other?.X == this.X && other.Y == this.Y;
    }

    public bool Equals(Point? x, Point? y)
    {
        return x?.Equals(y) ?? false;
    }

    public int GetHashCode([DisallowNull] Point obj)
    {
        throw new NotImplementedException();
    }
}
