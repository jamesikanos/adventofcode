using System.Text.RegularExpressions;

var numberPattern = new Regex("[0-9]+");

var lines = File.ReadAllLines("input.txt");

var monkeys = new List<Monkey>();

foreach (var line in lines)
{
    if (line.StartsWith("Monkey"))
    {
        monkeys.Add(new Monkey());
        continue;
    }

    if (line.Contains("Starting items:"))
    {
        var startingItems = new List<int>();
        foreach (var match in numberPattern.Matches(line))
        {
            if (match is Match mObj && mObj.Success)
            {
                startingItems.Add(int.Parse(mObj.Value));
            }
        }
        monkeys.Last().Items = startingItems;

        continue;
    }

    if (line.Contains("Operation:"))
    {
        var operationPattern = new Regex("old (\\*|\\+) (old|[0-9]+)");

        var match = operationPattern.Match(line);
        var operationType = match.Groups[1].ToString();
        var operationSecondPart = match.Groups[2].ToString();
        
        if (operationType == "+")
        {
            monkeys.Last().Operation =
                (old) => old + ((int.TryParse(operationSecondPart, out var sP)) ? sP : old);
        }

        if (operationType == "*")
        {
            monkeys.Last().Operation =
                (old) => {
                    var secondPart = int.TryParse(operationSecondPart, out var sP) ? sP : old;

                    return old * secondPart;
                };
        }
    }

    if (line.Contains("Test: divisible by"))
    {
        monkeys.Last().TestDivisibleBy = int.Parse(numberPattern.Match(line).Value);
        continue;
    }

    if (line.Contains("If true:"))
    {
        monkeys.Last().TrueMonkeyThrow = int.Parse(numberPattern.Match(line).Value);
        continue;
    }

    if (line.Contains("If false:"))
    {
        monkeys.Last().FalseMonkeyThrow = int.Parse(numberPattern.Match(line).Value);
        continue;
    }
}

var cycleLength = monkeys.Aggregate(1, (acc, val) => val.TestDivisibleBy * acc);

for (long i = 0; i < 10000; i++)
{
    foreach (var monkey in monkeys)
    {
        var startingItems = monkey.Items.ToArray();

        foreach (var item in startingItems)
        {
            var testResult = monkey.Operation(item) % cycleLength;

            monkey.Inspections++;

            if (testResult % monkey.TestDivisibleBy == 0)
            {
                monkeys[monkey.TrueMonkeyThrow].Items.Add(testResult);
            }
            else
            {
                monkeys[monkey.FalseMonkeyThrow].Items.Add(testResult);
            }
        }

        monkey.Items.Clear();
    }

    Console.WriteLine("== After Round {0} ==", i + 1);
}

for (int i = 0; i < monkeys.Count; i++)
{
    Console.WriteLine("Monkey {0} inspected items {1} times", i, monkeys[i].Inspections);
}

var highestInspections = monkeys.OrderByDescending(j => j.Inspections).Take(2).ToArray();

foreach (var m in highestInspections)
{
    Console.WriteLine(m.Inspections.ToString());
}

Console.WriteLine("Score: {0}", highestInspections[0].Inspections * highestInspections[1].Inspections);

class Monkey
{
    public List<int> Items { get; set; }

    public Func<int, int> Operation { get; set; }

    public int TestDivisibleBy { get; set; }

    public int TrueMonkeyThrow { get; set; }

    public int FalseMonkeyThrow { get; set; }

    public long Inspections { get; set; }
}
