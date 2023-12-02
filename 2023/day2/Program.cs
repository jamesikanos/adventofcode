using System.Text.RegularExpressions;

var gamePattern = @"(Game (?<gameId>\d+): )|(?<count>\d+) (?<color>red|blue|green)";

var games = File.ReadAllLines("input.txt");

int totalPower = 0;

var successfulGameIds = new List<int>();

foreach (var game in games)
{
    var matches = Regex.Matches(game, gamePattern);

    var gameId = int.Parse(matches[0].Groups["gameId"].Value);

    var drawCollection = new List<(string color, int count)>();

    // Loop through each draw (match + 1)
    for (var i = 1; i < matches.Count; i++)
    {
        var count = int.Parse(matches[i].Groups["count"].Value);
        var color = matches[i].Groups["color"].Value;

        drawCollection.Add((color, count));
    }

    // Calculate the max values for each color in this game
    var redMax = drawCollection.Where(x => x.color == "red").Max(x => x.count);
    var greenMax = drawCollection.Where(x => x.color == "green").Max(x => x.count);
    var blueMax = drawCollection.Where(x => x.color == "blue").Max(x => x.count);

    // Calculate the power for this game
    var power = redMax * greenMax * blueMax;
    totalPower += power;

    Console.WriteLine($"Game: {gameId} - Power: {power} ({redMax} * {greenMax} * {blueMax})");

    // Draw has more than 12 red cubes
    if (drawCollection.Any(x => x.color == "red" && x.count > 12))
    {
        continue;
    }

    // Draw has more than 13 green cubes
    if (drawCollection.Any(x => x.color == "green" && x.count > 13))
    {
        continue;
    }

    // Draw has more than 14 blue cubes
    if (drawCollection.Any(x => x.color == "blue" && x.count > 14))
    {
        continue;
    }

    Console.WriteLine("Success: " + gameId);
    successfulGameIds.Add(gameId);
}

Console.WriteLine("Total GameIds: " + successfulGameIds.Aggregate(0, (acc, x) => acc + x));
Console.WriteLine("Total Power: " + totalPower);
