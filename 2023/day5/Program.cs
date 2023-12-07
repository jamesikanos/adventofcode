
using System.Text.RegularExpressions;

var text = File.ReadAllText("input.txt");

// Break up everything by double newline
var sections = text.Split("\n\n");

var pattern = @"(?<source>[a-z]+)-to-(?<destination>[a-z]+)";

long[] startingPoints = new long[0];

foreach (var section in sections)
{
    if (section.StartsWith("seeds: "))
    {
        var points = section.Substring(7).Split(" ").Select(long.Parse).ToArray();

        var p = new List<long>();

        for (var i = 0; i < points.Length; i+= 2)
        {
            var start = points[i];
            var length = points[i + 1];

            Console.WriteLine("Seed Range: {0} to {1}", start, start + length);

            for (var n = 0; n < length; n++)
            {
                p.Add(start + n);
            }

            startingPoints = p.ToArray();
        }

        // Order the starting points
        startingPoints = startingPoints.OrderBy(i => i).ToArray();

        continue;
    }

    var sectionLines = section.Split("\n");

    var match = Regex.Match(sectionLines[0], pattern);
    var sourceName = match.Groups["source"].Value;
    var destinationName = match.Groups["destination"].Value;

    var mappings = new List<MappingRange>();

    Console.WriteLine("Calculating: " + sectionLines[0]);

    for (var i = 1; i < sectionLines.Length; i++)
    {
        var numbers = sectionLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        mappings.Add(new MappingRange
        {
            Source = numbers[1],
            Destination = numbers[0],
            Length = numbers[2]
        });
    }

    // Order the list
    mappings = mappings.OrderBy(i => i.Source).ToList();

    Console.WriteLine("======");
    Console.WriteLine("Mapping to: " + destinationName);
    Console.WriteLine("Total Seeds: " + startingPoints.Length);

    // Loop through each number in the starting points
    // for (int i = 0; i < startingPoints.Count; i++)
    Parallel.For(0, startingPoints.Length, i =>
    {
        var source = startingPoints[i];

        long destination = 0;

        // Console.WriteLine("Starting with: " + source);

        var selectedMapping = mappings.FirstOrDefault(k => k.IsWithinRange(source));

        if (selectedMapping != null)
        {
            destination = selectedMapping.MapToDestination(source);
            // Console.WriteLine("Mapping To (found): " + destination);
        }
        else
        {
            destination = source;
            // Console.WriteLine("Mapping To (not-found): " + destination);
        }

        startingPoints[i] = destination;
    });

    Console.WriteLine();
}

Console.WriteLine("Lowest Location: " + startingPoints.Min());

class MappingRange
{
    public long Source { get; set; }

    public long Destination { get; set; }

    public long Length { get; set; }

    public long MaxSource => Source + Length;

    public bool IsWithinRange(long value)
    {
        return value >= Source && value < MaxSource;
    }

    public long MapToDestination(long source)
    {
        return source - Source + Destination;
    }
}

