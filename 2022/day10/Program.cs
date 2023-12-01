
using System.Text;

var lines = File.ReadAllLines("input.txt");

var currentCycle = 1;
var currentX = 1;

var stringBuilder = new StringBuilder();

for (int i = 0; i < lines.Length; i++)
{
    HandlePixelPosition();
    CheckInterestingCycle();
    currentCycle++;

    var currentLine = lines[i];

    if (currentLine == "noop")
    {
        // Do nothing on noop
        continue;
    }
    else if (currentLine.StartsWith("addx"))
    {
        // Run an extra cycle
        HandlePixelPosition();
        CheckInterestingCycle();
        currentCycle++;

        var addXValue = int.Parse(currentLine.Split(" ")[1]);
        currentX += addXValue;
    }
}

void HandlePixelPosition()
{
    // Calcualte the CRT pixel value (mod 40)
    var currentPixelPosition = (currentCycle - 1) % 40;

    // Store the 3 pixel positions of where the sprite currently is
    var spritePositions = new[] { currentX - 1, currentX, currentX + 1 };

    // If we overlap with the sprite, write a #
    if (spritePositions.Contains(currentPixelPosition))
    {
        stringBuilder.Append("#");
    }
    // Or . if we don't overwrite
    else
    {
        stringBuilder.Append(".");
    }
}

void CheckInterestingCycle()
{
    var interestingCycles = new[] { 40, 80, 120, 160, 200, 240 };

    if (interestingCycles.Contains(currentCycle))
    {
        // Output the current line and rese
        Console.WriteLine(stringBuilder.ToString());
        stringBuilder = new StringBuilder();
    }
}
