
var lines = File.ReadAllLines("input.txt");

var times = lines[0].Substring(10).Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
var distances = lines[1].Substring(10).Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

times = new long[] { long.Parse(times.Aggregate("", (acc, val) => acc + val.ToString())) };
distances = new long[] { long.Parse(distances.Aggregate("", (acc, val) => acc + val.ToString())) };

var winningCounts = new List<int>();

for (var t = 0; t < times.Length; t++)
{
    var time = times[t];

    var winningCount = 0;

    var previousRecordDistance = distances[t];

    Parallel.For(0, time, i =>
    {
        var speed = i;
        var durationOfMovement = time - i;

        var distance = speed * durationOfMovement;

        // Don't output console for part 2, it's too slow
        // Console.WriteLine($"Time: {time} - Button Press: {i} - Speed {speed} - Distance {distance}");

        if (distance > previousRecordDistance)
        {
            lock (times)
            {
                winningCount++;
            }
        }
    });

    winningCounts.Add(winningCount);
}

Console.WriteLine(winningCounts.Aggregate((acc, val) => acc * val));

// Part 2
Console.WriteLine(winningCounts[0]);

