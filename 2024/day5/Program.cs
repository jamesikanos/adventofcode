var allLines = File.ReadAllText("input.txt");

// Split into 2 sections, the first part (of lines) is the rules, the second is the inputs
var sections = allLines.Split("\n\n");

var rules = sections[0].Split("\n");
var inputs = sections[1].Split("\n");

var ruleDict = new List<(int, int)>();

foreach (var rule in rules)
{
    var parts = rule.Trim().Split("|");
    ruleDict.Add((int.Parse(parts[0]), int.Parse(parts[1])));
}

var validMiddleNumbersForPart1 = new List<int>();
var validMiddleNumbersForPart2 = new List<int>();

var comparer = new RuleComparer(ruleDict);

for (var i = 0; i < inputs.Length; i++)
{
    bool IsInOrder(List<int> numbers)
    {
        for (var j = 0; j < numbers.Count - 1; j++)
        {
            if (comparer.Compare(numbers[j], numbers[j + 1]) == 1)
            {
                return false;
            }
        }

        return true;
    }

    var input = inputs[i];

    var allNumbersInInput = input.Split(",").Select(int.Parse).ToList();

    var inOrder = IsInOrder(allNumbersInInput);

    if (inOrder)
    {
        var middleNumber = allNumbersInInput[(int)Math.Floor(allNumbersInInput.Count / 2d)];
        Console.WriteLine($"{input} is in order, adding middle number {middleNumber}");

        validMiddleNumbersForPart1.Add(middleNumber);
    }
    else
    {
        var cloneList = new List<int>(allNumbersInInput);

        // Reorder the list
        cloneList.Sort(comparer);

        var middleNumber = cloneList[(int)Math.Floor(allNumbersInInput.Count / 2d)];

        var newJoinedList = string.Join(",", cloneList.Select(j => j.ToString()));

        Console.WriteLine($"{input} is not in order, reordering to {newJoinedList}, adding middle number {middleNumber}");

        validMiddleNumbersForPart2.Add(middleNumber);
    }
}

// Sum all middle numbers
Console.WriteLine($"Part1 Answer: {validMiddleNumbersForPart1.Sum()}");
Console.WriteLine($"Part2 Answer: {validMiddleNumbersForPart2.Sum()}");

class RuleComparer: IComparer<int>
{
    private readonly List<(int, int)> _rules;

    public RuleComparer(List<(int, int)> rules)
    {
        _rules = rules;
    }

    public int Compare(int x, int y)
    {
        // If x is before y, return -1
        if (_rules.Any(r => r.Item1 == x && r.Item2 == y))
        {
            return -1;
        }

        // If y is before x, return 1
        if (_rules.Any(r => r.Item1 == y && r.Item2 == x))
        {
            return 1;
        }

        // If neither, return 0
        return 0;
    }
}