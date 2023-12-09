
var lines = File.ReadAllLines("input.txt");

var totalLast = 0;
var totalFirst = 0;

foreach (var line in lines)
{
    Console.WriteLine(line);

    var numbers = line.Split(" ").Select(int.Parse).ToArray();

    var stacks = new Stack<List<int>>();
    stacks.Push(numbers.ToList());

    List<int> currentSet() => stacks.Peek();

    do
    {
        var newSet = new List<int>();

        for (int i = 0; i < currentSet().Count - 1; i++)
        {
            var diff = currentSet()[i + 1] - currentSet()[i];
            newSet.Add(diff);
        }

        Console.WriteLine(string.Join(" ", newSet));

        stacks.Push(newSet);
    }
    while (!currentSet().All(i => i == 0));

    Console.WriteLine("-----");

    var nextNumber = 0;
    var firstNumber = 0;

    while (stacks.Count > 1)
    {
        // Get the last number of the final row
        var lastRow = stacks.Pop();
        var previousRow = stacks.Peek();

        // Calculate first and last numbers
        nextNumber = previousRow.Last() + lastRow.Last();
        firstNumber = previousRow.First() - lastRow.First();

        previousRow.Add(nextNumber);
        previousRow.Insert(0, firstNumber);
    }

    totalLast += stacks.Peek().Last();
    totalFirst += stacks.Peek().First();

    Console.WriteLine($"NUMBER TO ADD: [{stacks.Peek().First()},...,{stacks.Peek().Last()}]");

    Console.WriteLine("====");
}

Console.WriteLine("Part 1: " + totalLast);
Console.WriteLine("Part 2: " + totalFirst);
