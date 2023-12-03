// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllLines("input.txt");

var firstBasementIndex = 0;

foreach (var line in lines)
{
    int index = 0;

    // Read each char
    int i = 0;
    foreach (var character in line)
    {
        i++;
        if (character == '(')
        {
            index++;
        }
        else if (character == ')')
        {
            index--;
        }

        if (index < 0 && firstBasementIndex == 0)
        {
            firstBasementIndex = i;
        }
    }

    Console.WriteLine("Floor: " + index);
    Console.WriteLine("First Basement Index: " + firstBasementIndex);
}

