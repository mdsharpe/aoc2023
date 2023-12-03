var input = await File.ReadAllLinesAsync(args[0]);
var engine = new Engine(input);

var numbers = engine.EnumerateNumbers().ToArray();
var partNumbersSum = numbers.Where(o => o.Adjacents.Any()).Select(o => o.Value).Sum();
Console.WriteLine("Part numbers sum: {0}", partNumbersSum);

var gears = engine.EnumerateGears().ToArray();
var gearRatiosSum = 0;

foreach (var g in gears)
{
    var adjacentNumbers = numbers.Where(o => o.Adjacents.ContainsKey(g))
                         .Select(o => o.Value)
                         .ToArray();

    if (adjacentNumbers.Length > 1)
    {
        var gearRatio = adjacentNumbers.Aggregate((a, b) => a * b);
        gearRatiosSum += gearRatio;
    }
}

Console.WriteLine("Gear ratios sum:  {0}", gearRatiosSum);
