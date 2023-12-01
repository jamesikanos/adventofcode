
using System.Text.RegularExpressions;

const int TotalDiskSpace = 70000000;
const int RequiredUnusedSpace = 30000000;

var directories = new List<ElfDirectory>();

var cdPattern = new Regex("\\$ cd ([\\w/\\.]*)");
var lsPattern = new Regex("\\$ ls");

var lines = File.ReadAllLines("input.txt");

// Build the File System
BuildFileSystem(directories, cdPattern, lsPattern, lines);

// List the full file system (and calcualte sizes)
Console.WriteLine("  - / (directory)");
EnumerateDirectory("/", 1);

// Find all directories with a size of less than 10000
// var matchedDirectories = directories.Where(j => j.FullSize <= 100000).ToList();

// Console.WriteLine("Answer A: " + matchedDirectories.Aggregate(0L, (acc, val) => acc + val.FullSize));

var spaceToFind = RequiredUnusedSpace - (TotalDiskSpace - FetchDirectory("/").FullSize);

var smallestDirectoryToDelete = directories.OrderBy(j => j.FullSize).FirstOrDefault(j => j.FullSize > spaceToFind);

Console.WriteLine(smallestDirectoryToDelete.FullSize);

long EnumerateDirectory(string currentDirectory, int depth)
{
    currentDirectory = currentDirectory.Replace("//", "/");

    var elfDirectory = FetchDirectory(currentDirectory);

    var padding = string.Join("", Enumerable.Range(0, depth * 2).Select(_ => " "));

    elfDirectory.FullSize = 0;

    foreach (var i in elfDirectory.Files)
    {
        if (i.Size == -1)
        {
            Console.WriteLine("{0} - {1} (directory)", padding, i.Name);

            elfDirectory.FullSize += EnumerateDirectory(currentDirectory + "/" + i.Name, depth + 1);
            continue;
        }

        elfDirectory.FullSize += i.Size;

        Console.WriteLine("{0} - {1} ({2} bytes)", padding, i.Name, i.Size);
    }

    Console.WriteLine("{0} - Full Size: {1} - {2} bytes", padding, elfDirectory.FullPath, elfDirectory.FullSize);

    return elfDirectory.FullSize;
}

ElfDirectory FetchDirectory(string path)
{
    var i = directories.FirstOrDefault(j => j.FullPath == path);

    if (i != null)
    {
        return i;
    }

    i = new ElfDirectory { FullPath = path, Files = new List<(string Name, long Size)>() };

    directories.Add(i);

    return i;
}

void ParseCDCommand(string line, ref string currentDirectory)
{
    var match = cdPattern.Match(line);

    var newCd = match.Groups[1].Captures[0].Value;

    if (newCd == "/")
    {
        currentDirectory = "/";
    }
    else if (newCd == "..")
    {
        var t = new List<string>(currentDirectory.Split("/", StringSplitOptions.RemoveEmptyEntries));
        t.RemoveAt(t.Count - 1);
        currentDirectory = "/" + string.Join("/", t);
    }
    else
    {
        if (!currentDirectory.EndsWith("/"))
        {
            currentDirectory += "/";
        }

        currentDirectory += newCd;
    }

    currentDirectory = currentDirectory.Replace("//", "/");

    Console.WriteLine("Changed to: " + currentDirectory);
}

void BuildFileSystem(List<ElfDirectory> directories, Regex cdPattern, Regex lsPattern, string[] lines)
{
    string currentDirectory = "/";

    foreach (var line in lines)
    {
        Console.WriteLine(line);

        if (cdPattern.IsMatch(line))
        {
            ParseCDCommand(line, ref currentDirectory);
            continue;
        }

        if (lsPattern.IsMatch(line))
        {
            Console.WriteLine("Listing Directory: " + currentDirectory);
            continue;
        }

        var fetchedDirectory = FetchDirectory(currentDirectory);

        var splits = line.Split(" ");

        var sz = 0;

        if (splits[0] == "dir")
        {
            sz = -1;
        }
        else
        {
            sz = int.Parse(splits[0]);
        }

        fetchedDirectory.Files.Add((splits[1], sz));
    }
}

class ElfDirectory
{
    public string FullPath { get; set; }

    public List<(string Name, long Size)> Files { get; set; }

    public long FullSize { get; set; }
}
