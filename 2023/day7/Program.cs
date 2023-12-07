var lines = File.ReadAllLines("input.txt");

var hands = new List<Hand>();

foreach (var line in lines)
{
    // Simple string split to find the hand value and the bid
    var val = line.Substring(0, 5);
    var bid = line.Substring(6);

    // Create the hand
    var hand = new Hand
    {
        Value = val,
        Bid = int.Parse(bid)
    };

    // Assign the hand type
    hand.CalculateHandType();

    // Add to the list
    hands.Add(hand);
}

// Sort the list, using the IComparer inside the class
hands.Sort();

Console.WriteLine("Sorted");

int rank = 1;

var totalScore = 0;

// Loop through each hand
foreach (var hand in hands)
{
    Console.Write($"Rank: {rank} - {hand.Value} - Type: {hand.HandType}");

    if (hand.HasJoker)
    {
        Console.Write(" - J");
    }

    Console.WriteLine();

    // Increment the score...
    totalScore += rank * hand.Bid;

    rank++;
}

Console.WriteLine($"Total Score: {totalScore}");

/// <summary>
/// Enum in order of strength and hand type
/// </summary>
public enum HandType
{
    FiveKind,
    FourKind,
    FullHouse,
    ThreeKind,
    TwoPair,
    OnePair,
    HighCard
}

public class Hand : IComparable<Hand>
{
    public string Value { get; set; }

    public int Bid { get; set; }

    public HandType HandType { get; set; }

    public bool HasJoker => Value.Contains("J");

    public void CalculateHandType()
    {
        if (Value.Length != 5)
        {
            throw new ArgumentOutOfRangeException();
        }

        var chars = Value.ToCharArray().OrderBy(i => i).ToArray();
        var distinctChars = chars.Distinct().ToArray();

        switch (distinctChars.Length)
        {
            case 1:
                HandType = HandType.FiveKind;
                break;
            case 2:
                HandType = (chars[0] == chars[3] || chars[1] == chars[4]) ? HandType.FourKind : HandType.FullHouse;
                break;
            case 3:
                // HandType = HandType.ThreeKind; // Or 2 pair
                if (chars[0] == chars[1] && chars[1] == chars[2])
                {
                    HandType = HandType.ThreeKind;
                }
                else if (chars[1] == chars[2] && chars[2] == chars[3])
                {
                    HandType = HandType.ThreeKind;
                }
                else if (chars[2] == chars[3] && chars[3] == chars[4])
                {
                    HandType = HandType.ThreeKind;
                }
                else
                {
                    HandType = HandType.TwoPair;
                }
                break;
            case 4:
                HandType = HandType.OnePair;
                break;
            default:
                HandType = HandType.HighCard;
                break;
        }

        var numberOfJokers = Value.Count(j => j == 'J');

        if (numberOfJokers == 0)
        {
            Console.WriteLine($"{new string(chars)} - {HandType}");
            return;
        }

        var oldHandType = HandType;

        // Work from lowest to highest
        if (HandType == HandType.HighCard)
        {
            // A high card with a joker always becomes a 1 pair
            HandType = HandType.OnePair;
        }
        else if (HandType == HandType.OnePair)
        {
            // A 1 pair can only contain 1 or 2 jokers, always becomes a 3 kind
            HandType = HandType.ThreeKind;
        }
        else if (HandType == HandType.TwoPair && numberOfJokers == 1)
        {
            // If 1 joker in 2 pair, becomes a full house
            HandType = HandType.FullHouse;
        }
        else if (HandType == HandType.TwoPair && numberOfJokers == 2)
        {
            // If 2 jokers in a 2 pair, becomes a four kind
            HandType = HandType.FourKind;
        }
        else if (HandType == HandType.ThreeKind)
        {
            // Jokers of 1 or 3 always makes a 4 kind - otherwise it would have been a full house
            HandType = HandType.FourKind;
        }
        else if (HandType == HandType.FullHouse)
        {
            // Jokers (2 or 3) always makes a 5 kind
            HandType = HandType.FiveKind;
        }
        else if (HandType == HandType.FourKind)
        {
            // Jokers (1 or 4) always makes a 5 kind
            HandType = HandType.FiveKind;
        }

        Console.WriteLine($"{new string(chars)} - JOKER - {oldHandType} to {this.HandType}");
    }

    public int CompareTo(Hand? other)
    {
        // If different hand types, sort initially
        if (other!.HandType != this.HandType)
        {
            return (other.HandType > this.HandType) ? 1 : -1;
        }

        // Array containing the order of strength (J is at the end as jokers are devalued)
        var strengthOrder = new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

        // Loop through each letter
        for (var i = 0; i < 5; i++)
        {
            // Extract the A and B strength for comparison
            var strengthA = Array.FindIndex(strengthOrder, k => k == this.Value[i]);
            var strengthB = Array.FindIndex(strengthOrder, k => k == other.Value[i]);

            // If the same, move onto the next letter
            if (strengthA == strengthB)
            {
                continue;
            }

            // Return -1 or 1 depending on the strength difference
            return (strengthA > strengthB) ? -1 : 1;
        }

        // All letters are the same, equal values
        return 0;
    }
}