var lines = (File.ReadAllLines("input.txt"));

var totalScore = 0;

var index = 0;

foreach (var line in lines)
{
    index++;

    var components = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    var compA = ParseGameValue(components[0]);
    var compB = ParseGameValue(components[1]);

    // Must lose with "X"
    if (components[1] == "X")
    {
        switch (compA)
        {
            // If Rock, chose scissors
            case AdventGameValue.Rock:
                compB = AdventGameValue.Scissors;
                break;
            case AdventGameValue.Paper:
                compB = AdventGameValue.Rock;
                break;
            case AdventGameValue.Scissors:
                compB = AdventGameValue.Paper;
                break;
        }
    }
    // Must draw with a "Y"
    else if (components[1] == "Y")
    {
        compB = compA;
    }
    // Must win with a "Z"
    else if (components[1] == "Z")
    {
        switch (compA)
        {
            // If Rock, chose Paper
            case AdventGameValue.Rock:
                compB = AdventGameValue.Paper;
                break;
            case AdventGameValue.Paper:
                compB = AdventGameValue.Scissors;
                break;
            case AdventGameValue.Scissors:
                compB = AdventGameValue.Rock;
                break;
        }
    }
    else
    {
        throw new Exception();
    }

    if (compA == null || compB == null)
    {
        throw new Exception();
    }

    if (compA == compB)
    {
        Console.WriteLine("{0} Draw " + compB, index);
        totalScore += (int)compB + 3;
        continue;
    }

    var isWin = false;

    // Rock beats Scissors
    if (compB == AdventGameValue.Rock && compA == AdventGameValue.Scissors)
    {
        isWin = true;
    }
    // Scissors beat Paper
    else if (compB == AdventGameValue.Scissors && compA == AdventGameValue.Paper)
    {
        isWin = true;
    }
    // Paper beats Rock
    else if (compB == AdventGameValue.Paper && compA == AdventGameValue.Rock)
    {
        isWin = true;
    }

    var additionalScore = (isWin ? 6 : 0) + (int)compB;

    Console.WriteLine("{0} Result: {1}    Me: {2}   Them: {3}", index, additionalScore, compB, compA);

    totalScore += additionalScore;
}

Console.WriteLine("Total Score: " + totalScore);

AdventGameValue? ParseGameValue(string str)
{
    if (str == "X" || str == "A")
    {
        return AdventGameValue.Rock;
    }

    if (str == "Y" || str == "B")
    {
        return AdventGameValue.Paper;
    }

    if (str == "Z" || str == "C")
    {
        return AdventGameValue.Scissors;
    }

    return null;
}
