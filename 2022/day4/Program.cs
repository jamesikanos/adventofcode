
var lines = File.ReadAllLines("input.txt");

var count = 0;

foreach (var line in lines)
{
    var splits = line.Split(",");

    var pair1 = ExtractFromTo(splits[0]);
    var pair2 = ExtractFromTo(splits[1]);

    if (pair1.from >= pair2.from && pair1.from <= pair2.to)
    {
        Console.WriteLine("Match: {0}", line);
        count++;
        continue;
    }

    if (pair2.from >= pair1.from && pair2.from <= pair1.to)
    {
        Console.WriteLine("Match: {0}", line);
        count++;
        continue;
    }

    // if (pair1.from >= pair2.from && pair1.to <= pair2.to)
    // {
    //     Console.WriteLine("Match: {0}", line);

    //     count++;
    //     continue;
    // }

    // if (pair2.from >= pair1.from && pair2.to <= pair1.to)
    // {
    //     Console.WriteLine("Match: {0}", line);

    //     count++;
    //     continue;
    // }
}

Console.WriteLine(count);

(int from, int to) ExtractFromTo(string input)
{
    var parts = input.Split("-");

    return (int.Parse(parts[0]), int.Parse(parts[1]));
}
