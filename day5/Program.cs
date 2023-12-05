var input = await File.ReadAllLinesAsync(args[0]);

static long FindMinLocation(Almanac almanac)
{
    long minLocation = long.MaxValue;
    object minLocationLock = new();
    long locationsVisted = 0L;
    Timer timerConsole = new((_) => Console.WriteLine($"Processed {locationsVisted} of {almanac.SeedCount} ({(float)locationsVisted / almanac.SeedCount * 100}%)... (min location = {minLocation})"), null, 0, 1000);

    Parallel.ForEach(
        almanac.SeedsEnumerable,
        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 },
        seed =>
    {
        var location = almanac.Lookup("seed", "location", seed);
        Interlocked.Increment(ref locationsVisted);

        if (location < minLocation)
        {
            lock (minLocationLock)
            {
                if (location < minLocation)
                {
                    minLocation = location;
                }
            }
        }
    });

    timerConsole.Dispose();

    return minLocation;
}

Almanac almanac;
long minLocation;

almanac = Almanac.Parse(input);
Console.WriteLine($"Almanac parsed with {almanac.SeedCount} seeds.");
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");

almanac = Almanac.Parse(input, interpretSeedsAsRanges: true);
Console.WriteLine($"Almanac parsed with {almanac.SeedCount} seeds.");
minLocation = FindMinLocation(almanac);
Console.WriteLine($"Min location: {minLocation}");
