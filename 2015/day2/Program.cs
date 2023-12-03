var lines = File.ReadAllLines("input.txt");

var total = 0;
var totalRibbon = 0;

foreach (var line in lines)
{
    var dimensions = line.Split('x').Select(j => int.Parse(j)).ToList();

    var l = dimensions[0];
    var w = dimensions[1];
    var h = dimensions[2];

    // var total = (2 * l * w) + (2 * l * h) + (2 * h * l);

    var side1 = 2 * l * w;
    var side2 = 2 * w * h;
    var side3 = 2 * h * l;

    var slack = Math.Min(Math.Min(side1, side2), side3) / 2;

    var subtotal = side1 + side2 + side3 + slack;

    Console.WriteLine(subtotal);

    total += subtotal;

    // Ribbin and Bow calculation
    var bow = l * w * h;

    var allSides = new [] { l, w, h }.OrderBy(j => j).ToArray();

    var calc = allSides[0] + allSides[0] + allSides[1] + allSides[1];

    Console.WriteLine("Ribbon Length: " + (calc + bow));

    totalRibbon += (calc + bow);

}

Console.WriteLine(total);
Console.WriteLine(totalRibbon);
