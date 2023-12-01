
using System.Text.Json.Nodes;

var lines = File.ReadAllLines("input.txt");

var pairs = new List<(string LeftSide, string RightSide)>();

string? leftSide = null;

for (int i = 0; i < lines.Length; i++)
{
    var currentLine = lines[i];

    if (string.IsNullOrWhiteSpace(currentLine))
    {
        continue;
    }

    if (leftSide == null)
    {
        leftSide = currentLine;
    }
    else
    {
        pairs.Add((leftSide, currentLine));
        leftSide = null;
    }
}

var score = 0;

var validPairs = new List<string>();

foreach (var pair in pairs)
{
    validPairs.Add(pair.LeftSide);
    validPairs.Add(pair.RightSide);
}

validPairs.Add("[[2]]");
validPairs.Add("[[6]]");

Console.WriteLine("Score: {0}", score);

validPairs.Sort((item1, item2) => {
    var leftObj = JsonArray.Parse(item1) as JsonArray;
    var rightObj = JsonArray.Parse(item2) as JsonArray;

    var result = CompareArray(leftObj, rightObj);

    if (result == ElfResult.InvalidOrder)
    {
        return -1;
    }

    if (result == null)
    {
        return 0;
    }

    return 1;
});

validPairs.Reverse();

foreach (var validPair in validPairs)
{
    Console.WriteLine(validPair);
}

var decoderIndexA = validPairs.IndexOf("[[2]]") + 1;
var decoderIndexB = validPairs.IndexOf("[[6]]") + 1;

Console.WriteLine(decoderIndexA * decoderIndexB);

ElfResult? CompareArray(JsonArray leftSide, JsonArray rightSide)
{
    Console.WriteLine("Left Array: {0}", leftSide);
    Console.WriteLine("Right Array: {0}", rightSide);

    for (int i = 0; i < Math.Max(leftSide.Count, rightSide.Count); i++)
    {
        if (i > leftSide.Count - 1)
        {
            Console.WriteLine("Left ran out of entries");
            return ElfResult.RightOrder;
        }

        if (i > rightSide.Count - 1)
        {
            Console.WriteLine("Right ran out of entries");
            return ElfResult.InvalidOrder;
        }

        var leftElem = leftSide[i];
        var rightElem = rightSide[i];

        Console.WriteLine("Checking {0} to {1}", leftElem, rightElem);

        if (!(leftElem is JsonArray) && !(rightElem is JsonArray))
        {
            if (leftElem == rightElem)
            {
                Console.WriteLine("Same");
                continue;
            }

            if (((int)leftElem) < ((int)rightElem))
            {
                return ElfResult.RightOrder;
            }

            if (((int)rightElem) < ((int)leftElem))
            {
                return ElfResult.InvalidOrder;
            }

            continue;
        }

        if (!(leftElem is JsonArray))
        {
            Console.WriteLine("Converting {0} to Array", leftElem);
            leftElem = new JsonArray((int)leftElem);
        }

        if (!(rightElem is JsonArray))
        {
            Console.WriteLine("Converting {0} to Array", rightElem);
            rightElem = new JsonArray((int)rightElem);
        }

        Console.WriteLine(leftElem);
        Console.WriteLine(rightElem);

        var result = CompareArray(leftElem as JsonArray, rightElem as JsonArray);

        if (result != null)
        {
            return result;
        }
    }

    return null;
}

enum ElfResult
{
    RightOrder,
    InvalidOrder
}
