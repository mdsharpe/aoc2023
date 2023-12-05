using System.Collections.Concurrent;

var input = await File.ReadAllLinesAsync(args.Length > 0 ? args[0] : "../../../input.txt");

static long FindMinLocation(Almanac almanac)
{
    ConcurrentDictionary<long, byte> locations = [];

    Parallel.ForEach(
        almanac.SeedsEnumerable,
        new ParallelOptions { MaxDegreeOfParallelism = 1 },
        seed =>
        {
            var location = almanac.Lookup("seed", "location", seed);
            locations.TryAdd(location, 0);
        });

    return locations.Keys.Min();
}

Almanac almanac;
long minLocation;

almanac = Almanac.Parse(input);
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");

almanac = Almanac.Parse(input, interpretSeedsAsRanges: true);
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");
