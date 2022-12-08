using MoreLinq;

var lines = File.ReadAllLines("input.txt");

var counts = new Dictionary<int, int>();

var currentCount = 0;

var currentElf = 0;

for (var i = 0; i < lines.Length; i++)
{
    var currentLine = lines[i]?.Trim();

    if (string.IsNullOrEmpty(currentLine))
    {
        Console.WriteLine("Counting Elf {0} - Item Count {1}", counts.Count, currentCount);

        if (currentCount > 0)
        {
            counts.Add(currentElf++, currentCount);
        }

        currentCount = 0;
        continue;
    }

    if (int.TryParse(currentLine, out var newCount))
    {
        currentCount += newCount;
    }
}

Console.WriteLine("Counting Elf {0} - Item Count {1}", counts.Count, currentCount);

if (currentCount > 0)
{
    counts.Add(currentElf++, currentCount);
}

var topItems = counts.OrderByDescending(j => j.Value).Take(3);

var topCount = 0;

foreach (var topItem in topItems)
{
    topCount += topItem.Value;
    Console.WriteLine("Elf {0} with {1} calories", topItem.Key, topItem.Value);
}

Console.WriteLine(topCount);