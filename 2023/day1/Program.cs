
using System.Text.RegularExpressions;

var file = File.ReadAllLines("input.txt");

int total = 0;

int lineNumber = 1;

const string RegexPattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

foreach (var line in file)
{
    int wholeNumber = 0;

    var firstMatch = Regex.Match(line, RegexPattern);
    var lastMatch = Regex.Match(line, RegexPattern, RegexOptions.RightToLeft);

    var firstDigit = MatchToNumber(firstMatch);

    var lastDigit = MatchToNumber(lastMatch);

    wholeNumber = firstDigit * 10 + lastDigit;

    total += wholeNumber;

    Console.WriteLine("{0} - {1} - {2}", lineNumber, wholeNumber, total);

    lineNumber++;
}

// ==== Solution ====
Console.WriteLine(total);

static int MatchToNumber(Match match)
{
    // If match can parse to a number, return it
    if (int.TryParse(match.ToString(), out int number))
    {
        return number;
    }

    return match.ToString() switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => throw new Exception("Invalid number"),
    };
}
