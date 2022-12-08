using System.Text;
using System.Text.RegularExpressions;

var a1 = new[] { 'V', 'J', 'B', 'D' };
var a2 = new[] { 'F', 'D', 'R', 'W', 'B', 'V', 'P' };
var a3 = new[] { 'Q', 'W', 'C', 'D', 'L', 'F', 'G', 'R' };
var a4 = new[] { 'B', 'D', 'N', 'L', 'M', 'P', 'J', 'W' };
var a5 = new[] { 'Q', 'S', 'C', 'P', 'B', 'N', 'H' };
var a6 = new[] { 'G', 'N', 'S', 'B', 'D', 'R' };
var a7 = new[] { 'H', 'S', 'F', 'Q', 'M', 'P', 'B', 'Z' };
var a8 = new[] { 'F', 'L', 'W' };
var a9 = new[] { 'R', 'M', 'F', 'V', 'S' };

Stack<char> CreateStack(char[] s)
{
    return new Stack<char>(s.Reverse());
}

var stacks = new List<Stack<char>>();
stacks.Add(CreateStack(a1));
stacks.Add(CreateStack(a2));
stacks.Add(CreateStack(a3));
stacks.Add(CreateStack(a4));
stacks.Add(CreateStack(a5));
stacks.Add(CreateStack(a6));
stacks.Add(CreateStack(a7));
stacks.Add(CreateStack(a8));
stacks.Add(CreateStack(a9));

var lines = File.ReadAllLines("input.txt");

// Test for sample
// lines = File.ReadAllLines("sample.txt");
// stacks.Clear();

// stacks.Add(CreateStack(new[] { 'N', 'Z' }));
// stacks.Add(CreateStack(new[] { 'D', 'C', 'M' }));
// stacks.Add(CreateStack(new[] { 'P' }));

var pattern = new Regex("move ([0-9]*) from ([0-9]) to ([0-9])");

foreach (var line in lines)
{
    var match = pattern.Match(line);

    Console.WriteLine(line);

    var moveCount = int.Parse(match.Groups[1].Captures[0].Value);
    var moveFrom = int.Parse(match.Groups[2].Captures[0].Value);
    var moveTo = int.Parse(match.Groups[3].Captures[0].Value);

    var fromStack = stacks[moveFrom - 1];
    var tostack = stacks[moveTo - 1];

    var valuesToMove = new List<char>();

    for (var i = 0; i < moveCount; i++)
    {
        var valueToMove = fromStack.Pop();
        valuesToMove.Add(valueToMove);
    }

    valuesToMove.Reverse();

    foreach (var valuesToMoveReverse in valuesToMove)
    {
        tostack.Push(valuesToMoveReverse);
    }
}

var builder = new StringBuilder();

foreach (var stack in stacks)
{
    builder.Append(stack.Peek());
}

Console.WriteLine(builder);
