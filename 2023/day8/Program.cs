

using System.Collections;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var directions = lines[0].ToCharArray().ToList();

Dictionary<string, Node> allNodes = new();

// Assemble the tree
foreach (var map in lines.Skip(2))
{
    var pattern = @"[A-Z0-9]{3}";

    var coordMatch = Regex.Matches(map, pattern);

    var c1 = coordMatch[0].Value;
    var c2 = coordMatch[1].Value;
    var c3 = coordMatch[2].Value;

    var node1 = new Node
    {
        Value = c1
    };

    allNodes.Add(c1, node1);

    node1.Left = c2;
    node1.Right = c3;

    Console.WriteLine(node1.ToString());
}

Console.WriteLine("====");

foreach (var nodes in allNodes)
{
    Console.WriteLine(nodes.Value.ToString());
}

Console.WriteLine(allNodes.Count);

var allCounts = new List<long>();

foreach (var startingPoint in allNodes.Where(i => i.Key.EndsWith("A")).Select(i => i.Key))
{
    long count = 0;
    var enumerator = new CircularListEnumerator<char>(directions);

    var currentNode = startingPoint;

    Console.WriteLine("Starting On: " + currentNode);

    do
    {
        count++;

        var newCurrentNodes = new List<string>();

        var currentDirection = enumerator.Current;

        var nextNode = "";

        if (currentDirection == 'L')
        {
            nextNode = allNodes[currentNode].Left;
        }
        else
        {
            nextNode = allNodes[currentNode].Right;
        }

        // Console.WriteLine("{0} {3} - Current Node: {1} moving to: {2}", currentDirection, currentNode, nextNode, enumerator.CurrentIndex);

        currentNode = nextNode;

        if (currentNode.EndsWith("Z"))
        {
            break;
        }
    }
    while (enumerator.MoveNext());

    Console.WriteLine($"Ending at {currentNode} - Step Count: {count}");

    allCounts.Add(count);
}

Console.WriteLine(LCM(allCounts.ToArray()));

static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

public class CircularListEnumerator<T> : IEnumerator<T>
{
    public int CurrentIndex { get; set; }

    public IList<T> List { get; set; }

    public CircularListEnumerator(IList<T> list)
    {
        List = list;
    }

    public T Current => List[CurrentIndex];

    object IEnumerator.Current => this;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        CurrentIndex++;
        CurrentIndex %= List.Count;

        return true;
    }

    public void Reset()
    {
        CurrentIndex = 0;
    }
}

public class Node
{
    public string Left { get; set; }

    public string Right { get; set; }

    public string Value { get; set; }

    public override string ToString()
    {
        return string.Format("{0} = ({1}, {2})", Value, Left, Right);
    }
}
