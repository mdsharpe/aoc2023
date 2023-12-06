using System.Collections.Immutable;

var input = await File.ReadAllLinesAsync(args[0]);

var times = input[0].Substring(5).Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(long.Parse).ToArray();

var distances = input[1].Substring(9).Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(long.Parse).ToArray();

Console.WriteLine($"Times:     {string.Join(',', times)}");
Console.WriteLine($"Distances: {string.Join(',', distances)}");

var races = times.Select((t, i) => new Race(i, t, distances[i])).ToList();

races.Add(new Race(
    races.Max(o => o.Id) + 1,
    long.Parse(races.Select(o => o.Duration.ToString()).Aggregate("", string.Concat)),
    long.Parse(races.Select(o => o.Distance.ToString()).Aggregate("", string.Concat)),
    true
));

var waysToWinByRace = races.ToDictionary(o => o, o => 0L);

foreach (var race in races)
{
    Console.WriteLine("Race {0}...", race.Id);

    var waysToWin = 0;

    IEnumerable<long> DurationEnumerator(long duration)
    {
        for (var i = 0L; i < duration; i++)
        {
            yield return i;
        }
    };

    Parallel.ForEach(
        DurationEnumerator(race.Duration),
        buttonHeldDownMs =>
        {
            var speed = buttonHeldDownMs;
            var travelTime = race.Duration - buttonHeldDownMs;
            var distanceTravelled = speed * travelTime;

            if (distanceTravelled > race.Distance)
            {
                Interlocked.Increment(ref waysToWin);
            }
        });

    waysToWinByRace[race] = waysToWin;
    Console.WriteLine("Found {0} ways to win race that lasted {1}ms", waysToWinByRace[race], race.Duration);
}

var waysProduct = waysToWinByRace
    .Where(o => !o.Key.IsKerningCompensated)
    .Select(o => o.Value)
    .Aggregate(1L, (a, b) => a * b);
Console.WriteLine(waysProduct);
