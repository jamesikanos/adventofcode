
var lines = File.ReadAllLines("input.txt");

var count = 0;

var groups = new List<string>();

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        continue;
    }

    Console.WriteLine("Adding: {0}", line);

    groups.Add(line);

    if (groups.Count == 3)
    {
        var groupLineA = groups[0];

        Console.WriteLine("Checking: {0}", groupLineA);

        foreach (var groupLineAChar in groupLineA)
        {
            if (groups[1].Contains(groupLineAChar) && groups[2].Contains(groupLineAChar))
            {
                count += CharValue(groupLineAChar);
                break;
            }
        }

        groups.Clear();
    }

    continue;

    // var compartmentA = line.Substring(0, line.Length / 2);
    // var compartmentB = line.Substring(line.Length / 2);

    // var duplicatedItems = new List<char>();

    // foreach (var itemA in compartmentA)
    // {
    //     if (compartmentB.Contains(itemA) && !duplicatedItems.Contains(itemA))
    //     {
    //         duplicatedItems.Add(itemA);
    //     }
    // }

    // Console.WriteLine("{0} .  {1}", compartmentA, compartmentB);

    // foreach (var d in duplicatedItems)
    // {
    //     Console.WriteLine("{0} - {1}", d, CharValue(d));
    //     count += CharValue(d);
    // }
}

Console.WriteLine(count);

int CharValue(char v)
{
    if (v >= 'a' && v <= 'z')
    {
        return ((int)v) - 96;
    }

    if (v >= 'A' && v <= 'Z')
    {
        return ((int)v) - 38;
    }

    return 0;
}
