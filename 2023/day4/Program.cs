using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

// Pattern to divide the string into 3 sections, game id, draw and winning numbers
var pattern =  new Regex(@"Card\s+(?<gameId>\d+):(?<draw>[^|]*)\|(?<winner>.*)");

int total = 0;
int countOfScratchCards = 0;

// Store a dictionary of all cards that need to be repeated
// The key is the "Game Id" and the Value is the number of times it needs to repeat
var gameIdRepeats = new Dictionary<int, int>();

foreach (var line in lines)
{
    var match = pattern.Match(line);

    var gameId = int.Parse(match.Groups["gameId"].Value);
    var drawNumbers = match.Groups["draw"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    var winningNumbers = match.Groups["winner"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

    var repeatCount = 1;

    // If we need to repeat...
    if (gameIdRepeats.ContainsKey(gameId))
    {
        repeatCount += gameIdRepeats[gameId];
    }

    // Loop through based on the repeat count
    for (var i = 0; i < repeatCount; i++)
    {
        countOfScratchCards++; // Star 2 - The number of scratch cards we've been processing

        var score = 0;
        var winningDrawnCards = 0;

        // Go through each draw and match against the winner
        foreach (var number in drawNumbers)
        {
            if (!winningNumbers.Contains(number))
                continue;

            winningDrawnCards++; // Star 2 - The number of winning numbers

            // Star 1 - If the score is 0, make it 1
            if (score == 0)
            {
                score = 1;
                continue;
            }

            // Otherwise, double it
            score *= 2;
        }

        total += score;

        Console.WriteLine(match);
        Console.WriteLine("Score: " + score + " Running Total: " + total);
        Console.WriteLine();

        // Add in the repeats
        for (int j = 0; j < winningDrawnCards; j++)
        {
            // Calculate the game id from this game id +1 +j
            var gameIdToRepeat = j + 1 + gameId;

            // If we're not repeating this game id yet, add it to the dictionary
            if (!gameIdRepeats.ContainsKey(gameIdToRepeat))
            {
                gameIdRepeats[gameIdToRepeat] = 1;
                continue;
            }

            // Or, incremenet by 1
            gameIdRepeats[gameIdToRepeat]++;
        }
    }
}

Console.WriteLine("Total Score: " + total);
Console.WriteLine("Total Scratchcards: " + countOfScratchCards);