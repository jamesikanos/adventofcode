// Open the sample.txt in the same folder as the program
// For each line, split by the space character
// Remove duplicate entries

using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");

var leftList = new List<int>();
var rightList = new List<int>();

foreach (var line in lines)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    leftList.Add(int.Parse(parts[0].Trim()));
    rightList.Add(int.Parse(parts[1].Trim()));
}

leftList.Sort();
rightList.Sort();

Debug.Assert(leftList.Count == rightList.Count);

var distance = 0;

for (var i = 0; i < leftList.Count; i++)
{
    var leftNumber = leftList[i];
    var rightNumber = rightList[i];

    var newDistance = Math.Abs(rightNumber - leftNumber);

    distance += newDistance;
}

Console.WriteLine($"Part One - Distance - {distance}");

var similarityScore = 0;

for (var i = 0; i < leftList.Count; i++)
{
    var leftNumber = leftList[i];

    // Find the number of times the left number appears in the right list
    var rightCount = rightList.Count(x => x == leftNumber);

    similarityScore += leftNumber * rightCount;
}

Console.WriteLine($"Part Two - Similarity Score - {similarityScore}");
