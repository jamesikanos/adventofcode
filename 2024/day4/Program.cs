var fileLines = File.ReadAllLines("input.txt");

var findPart1Count = 0;
var findPart2Count = 0;

// Loop through each line and row
for (var y = 0; y < fileLines.Length; y++)
{
    for (var x = 0; x < fileLines[0].Length; x++)
    {
        var currentChar = fileLines[y][x];

        if (currentChar == 'A')
        {
            if (FindXmasPart2(x, y, "MSMS"))
            {
                Console.WriteLine($"Found X-MAS at {x},{y}");
                findPart2Count++;
                continue;
            }

            if (FindXmasPart2(x, y, "SMSM"))
            {
                Console.WriteLine($"Found X-MAS at {x},{y}");
                findPart2Count++;
                continue;
            }

            if (FindXmasPart2(x, y, "SSMM"))
            {
                Console.WriteLine($"Found X-MAS at {x},{y}");
                findPart2Count++;
                continue;
            }

            if (FindXmasPart2(x, y, "MMSS"))
            {
                Console.WriteLine($"Found X-MAS at {x},{y}");
                findPart2Count++;
                continue;
            }

            continue;
        }

        if (currentChar == 'X')
        {
                if (FindForwards(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going forwards");
                    findPart1Count++;
                }

                if (FindBackwards(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going backwards");
                    findPart1Count++;
                }

                if (FindDown(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going down");
                    findPart1Count++;
                }

                if (FindUp(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going up");
                    findPart1Count++;
                }

                if (FindDiagonalDownRight(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going diagonal down right");
                    findPart1Count++;
                }

                if (FindDiagonalDownLeft(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going diagonal down left");
                    findPart1Count++;
                }

                if (FindDiagonalUpRight(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going diagonal up right");
                    findPart1Count++;
                }

                if (FindDiagonalUpLeft(x, y))
                {
                    Console.WriteLine($"Found XMAS at {x},{y} going diagonal up left");
                    findPart1Count++;
                }
        }
    }
}

Console.WriteLine($"Found {findPart1Count} XMAS");
Console.WriteLine($"Found {findPart2Count} XMAS for Part 2");

bool FindXmasPart2(int x, int y, string wordToFind)
{
    bool checkCell(int x1, int y1, char c)
    {
        if (x1 < 0 || y1 < 0 || x1 >= fileLines[0].Length || y1 >= fileLines.Length)
        {
            return false;
        }

        return fileLines[y1][x1] == c;
    }

    if (!checkCell(x - 1, y - 1, wordToFind[0]))
    {
        return false;
    }

    if (!checkCell(x + 1, y - 1, wordToFind[1]))
    {
        return false;
    }

    if (!checkCell(x - 1, y + 1, wordToFind[2]))
    {
        return false;
    }

    if (!checkCell(x + 1, y + 1, wordToFind[3]))
    {
        return false;
    }

    return true;
}

bool FindWithFunction(int x, int y, Func<int, int, (int newX, int newY)> fn)
{
    var words = new List<char>();

    try
    {
        while (words.Count < 4)
        {
            words.Add(fileLines[y][x]);

            var (newX, newY) = fn(x, y);

            x = newX;
            y = newY;
        }
    }
    catch
    {
        // This is dirty, but supressing the exception is the easiest thing to do here
        return false;
    }

    return new string(words.ToArray()) == "XMAS";
}

bool FindForwards(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x + 1, y));
}

bool FindBackwards(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x - 1, y));
}

bool FindDown(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x, y + 1));
}

bool FindUp(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x, y - 1));
}

bool FindDiagonalDownRight(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x + 1, y + 1));
}

bool FindDiagonalDownLeft(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x - 1, y + 1));
}

bool FindDiagonalUpRight(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x + 1, y - 1));
}

bool FindDiagonalUpLeft(int x, int y)
{
    return FindWithFunction(x, y, (x, y) => (x - 1, y - 1));
}