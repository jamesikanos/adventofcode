using System.Text.RegularExpressions;

// Part 1 Sample Data - "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"
// Part 2 Sample Data - "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

var fileData = File.ReadAllText("input.txt");

const string pattern = "mul\\(([0-9]{1,3}),([0-9]{1,3})\\)|do\\(\\)|don't\\(\\)";

int part1Total = PerformCalculation(false, fileData, pattern);
Console.WriteLine($"Part1 Total: {part1Total}");

int part2Total = PerformCalculation(true, fileData, pattern);
Console.WriteLine($"Part2 Total: {part2Total}");

static int PerformCalculation(bool enableConditional, string fileData, string pattern)
{
    var matches = Regex.Matches(fileData, pattern);

    var isEnabled = true;
    var total = 0;

    foreach (var match in matches.Cast<Match>())
    {
        if (match.Value == "do()")
        {
            isEnabled = true;
            continue;
        }

        if (match.Value == "don't()")
        {
            isEnabled = false;
            continue;
        }

        if (!isEnabled && enableConditional)
        {
            Console.WriteLine($"Skipping: {match.Value}");

            continue;
        }

        var leftVal = int.Parse(match.Groups[1].Value);
        var rightVal = int.Parse(match.Groups[2].Value);

        var result = leftVal * rightVal;

        Console.WriteLine($"{match.Value} = {result}");

        total += result;
    }

    return total;
}