using System.Text.RegularExpressions;

var fileLines = File.ReadAllLines("input.txt");

// Read everything that is not a dot
var pattern = new Regex(@"(\d+|[^\.\n])");

var symbolsOrNumbers = new List<SymbolOrNumber>();

var lineCount = 0;

// Read all lines, extract the numbers and symbols
foreach (var line in fileLines)
{
    var matches = pattern.Matches(line);

    foreach (var match in matches.Cast<Match>())
    {
        symbolsOrNumbers.Add(new SymbolOrNumber
        {
            Value = match.ToString(),
            X = match.Index,
            Y = lineCount
        });
    }

    lineCount++;
}

// Find all numbers (not symbols)
var allNumbers = symbolsOrNumbers.Where(x => x.IsNumber).ToList();

var matchedNumbers = new List<int>();

foreach (var number in allNumbers)
{
    var isMatch = false;

    // Loop through surrounding co-ordinates. Including diagonal
    for (int y = number.Y - 1; y < number.Y + 2; y++)
    {
        for (int index = -1; index < number.Value.Length + 1; index++)
        {
            int x = index + number.X;

            try
            {
                var matchedChar = fileLines[y][x];

                // Print out the number char for visual representation
                if (number.MatchesCoordinate(x, y))
                {
                    Console.Write(matchedChar);
                    continue;
                }

                // If the char at the co-ordinate isn't a '.' then it should be included
                if (matchedChar != '.')
                {
                    isMatch = true;
                }

                Console.Write(matchedChar);
            }
            catch (IndexOutOfRangeException)
            {
                Console.Write('.');
            }
        }

        Console.WriteLine();
    }

    Console.WriteLine("Matched: {0} - {1}", number.Value, isMatch);
    Console.WriteLine();

    if (isMatch)
    {
        matchedNumbers.Add(number.AsNumber);
    }
}

// Sum of all matched numbers
Console.WriteLine("Sum of all matched numbers: {0}", matchedNumbers.Sum());
Console.WriteLine();

// Find all * symbols
var allStar = symbolsOrNumbers.Where(x => x.Value == "*").ToList();

// Total Gear Ratio
int gearRatio = 0;

foreach (var starSymbol in allStar)
{

    Console.WriteLine("=======");

    var foundNumbers = new List<SymbolOrNumber>();

    // Loop through surrounding co-ordinates. Including diagonal
    for (int y = starSymbol.Y - 1; y < starSymbol.Y + 2; y++)
    {
        Console.Write("| ");

        for (int index = -1; index < 2; index++)
        {
            int x = index + starSymbol.X;

            try
            {
                var matchedChar = fileLines[y][x];
                Console.Write(matchedChar);

                // If the surrounding char can be parsed to int, add it to the list
                // Find it should the MatchesCoordinate method
                if (int.TryParse(matchedChar.ToString(), out _))
                {
                    foundNumbers.Add(allNumbers.Single(num => num.MatchesCoordinate(x, y)));
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.Write('.');
            }
        }

        Console.WriteLine(" |");
    }

    Console.WriteLine("=======");

    // Distint by id to ensure that we don't match the same number twice
    foundNumbers = foundNumbers.DistinctBy(x => x.Id).ToList();

    // Only increase the gear ratio if there are exactly 2 matched numbers
    if (foundNumbers.Count == 2)
    {
        gearRatio += foundNumbers[0].AsNumber * foundNumbers[1].AsNumber;

        Console.WriteLine("Found {0} numbers, increasing gear ratio by {1}x{2}={3}", foundNumbers.Count, foundNumbers[0].AsNumber, foundNumbers[1].AsNumber, gearRatio);
    }
    else
    {
        Console.WriteLine("Found {0} numbers, skipping", foundNumbers.Count);
    }

    Console.WriteLine();
}

Console.WriteLine("Gear ratio: {0}", gearRatio);

class SymbolOrNumber
{
    // Unique ID, useful for Part 2 to ensure that we don't match the same number twice
    public Guid Id { get; set; }

    public SymbolOrNumber()
    {
        Id = Guid.NewGuid();
    }

    public string? Value { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public bool IsNumber
    {
        get
        {
            return int.TryParse(Value, out _);
        }
    }

    public int AsNumber
    {
        get
        {
            return int.Parse(Value);
        }
    }

    // Tests the incoming co-ordinate against the current symbol or number
    public bool MatchesCoordinate(int x, int y)
    {
        if (Y != y)
        {
            return false;
        }

        if (x >= X && x < X + Value.Length)
        {
            return true;
        }

        return false;
    }
}
