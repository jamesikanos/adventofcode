// Set part2 to false/true if you want to run part 1 or part 2
var part2 = true;

var lines = File.ReadAllLines("input.txt");

var safeRounds = 0;

for (var i = 0; i < lines.Length; i++)
{
    var levels = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

    if (TestLevel(levels))
    {
        safeRounds++;
        continue;
    }

    if (!part2)
    {
        continue;
    }

    var success = false;

    for (var j = 0; j < levels.Count; j++)
    {
        // Create a copy of the level but skip over the ith value
        var copy = new List<int>(levels);
        copy.RemoveAt(j);

        if (TestLevel(copy))
        {
            success = true;
            break;
        }
    }

    if (success)
    {
        safeRounds++;
        continue;
    }
}

Console.WriteLine($"Safe rounds: {safeRounds}");

static bool TestLevel(List<int> levels)
{
    var direction = 0; // 0 = no direction, 1 = up, -1 = down

    for (var j = 1; j < levels.Count; j++)
    {
        var previousLevel = levels[j - 1];
        var currentLevel = levels[j];

        if (currentLevel > previousLevel)
        {
            if (direction == 0)
            {
                direction = 1;
            }
            else if (direction == -1)
            {
                return false;
            }
        }
        else if (currentLevel < previousLevel)
        {
            if (direction == 0)
            {
                direction = -1;
            }
            else if (direction == 1)
            {
                return false;
            }
        }

        // Check the distance
        var distance = Math.Abs(currentLevel - previousLevel);

        if (distance < 1 || distance > 3)
        {
            return false;
        }
    }

    return true;
}