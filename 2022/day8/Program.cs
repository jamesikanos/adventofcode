
var lines = File.ReadAllLines("input.txt");

var gridRows = new List<List<int>>();

// Parse the input file into a simple number grid
foreach (var line in lines)
{
    var newRow = new List<int>();

    foreach (var column in line)
    {
        newRow.Add(int.Parse(column.ToString()));
    }

    gridRows.Add(newRow);
}

var maxScenicScore = 0;

for (var y = 0; y < gridRows.Count; y++)
{
    for (var x = 0; x < gridRows![0].Count; x++)
    {
        // Ignore Edge Trees (what's the point)
        if (x == 0 || y == 0 || x >= (gridRows![0].Count - 1) || y >= (gridRows.Count - 1))
        {
            continue;
        }

        // Calculate the visible trees by following the line

        var leftTreeCount = VisibleTreeCount(x, y, (x1, y1) => (x1 - 1, y1));
        var topTreeCount = VisibleTreeCount(x, y, (x1, y1) => (x1, y1 - 1));
        var rightTreeCount = VisibleTreeCount(x, y, (x1, y1) => (x1 + 1, y1));
        var bottomTreeCount = VisibleTreeCount(x, y, (x1, y1) => (x1, y1 + 1));

        Console.WriteLine("Co_ord {0}:{1} Height: {2}", x, y, GetTree(x, y));

        var scenicScore = leftTreeCount * topTreeCount * rightTreeCount * bottomTreeCount;

        Console.WriteLine("Scenic Score: {0}x{1}x{2}x{3} = {4}", leftTreeCount, topTreeCount, rightTreeCount, bottomTreeCount, scenicScore);

        // Simple Math.Max to pick the highest score
        maxScenicScore = Math.Max(maxScenicScore, scenicScore);
    }
}

Console.WriteLine("Max Scenic Score: {0}", maxScenicScore);

int VisibleTreeCount(int x, int y, Func<int, int, (int, int)> countFunc)
{
    var thisTreeSize = GetTree(x, y);

    int treeCount = 0;

    while (true)
    {
        (x, y) = countFunc(x, y);

        var treeSize = GetTree(x, y);

        // If the edge, return 0
        if (treeSize == -1)
        {
            break;
        }

        treeCount++;

        if (treeSize >= thisTreeSize)
        {
            break;
        }
    }

    return treeCount;
}

int GetTree(int x, int y)
{
    if (x < 0 || y < 0)
    {
        return -1;
    }

    if (x >= gridRows![0].Count || y >= gridRows.Count)
    {
        return -1;
    }

    return gridRows[y][x];
}