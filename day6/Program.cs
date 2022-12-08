
const string sample1 = "bvwbjplbgvbhsrlpgdmjqwftvncz";
const string sample2 = "nppdvjthqldpwncqszvftbrmjlhg";
const string sample3 = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg";
const string sample4 = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw";

const String sample5 = "mjqjpqmgbljsphdztnvjfqwrcgsmlb";
const String sample6 = "bvwbjplbgvbhsrlpgdmjqwftvncz";
const String sample7 = "nppdvjthqldpwncqszvftbrmjlhg";
const String sample8 = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg";
const String sample9 = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw";

const int headerCount = 4;
const int messageCount = 14;

var inputString = File.ReadAllText("input.txt");

var buffer = new List<char>();

var stringToCheck = inputString;

for (int i = 0; i < stringToCheck.Length; i++)
{
    var newChar = stringToCheck[i];

    buffer.Add(newChar);

    while (buffer.Count > messageCount)
    {
        buffer.RemoveAt(0);
    }

    if (buffer.Distinct().Count() == messageCount)
    {
        Console.WriteLine(stringToCheck);
        Console.WriteLine("The Answer Is: " + i + 1);
        break;
    }
}

