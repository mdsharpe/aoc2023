var input = await File.ReadAllLinesAsync(args[0]);

static long FindMinLocation(Almanac almanac)
{
    var minLocation = long.MaxValue;

    foreach (var seed in almanac.Seeds)
    {
        var location = almanac.Lookup("seed", "location", seed);
        minLocation = Math.Min(minLocation, location);
    }

    return minLocation;
}

Almanac almanac;
long minLocation;

almanac = Almanac.Parse(input);
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");

almanac = Almanac.Parse(input, seedsAsRanges: true);
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");