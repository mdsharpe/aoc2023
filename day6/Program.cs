using System.Collections.Immutable;

var input = await File.ReadAllLinesAsync(args[0]);

var times = input[0].Substring(5).Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse).ToArray();

var distances = input[1].Substring(9).Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse).ToArray();

Console.WriteLine($"Times:     {string.Join(',', times)}");
Console.WriteLine($"Distances: {string.Join(',', distances)}");

var races = times.Select((t, i) => new Race(i, t, distances[i])).ToImmutableArray();

var waysToWinByRace = races.ToDictionary(o => o, o => 0);

foreach (var race in races)
{
    Console.WriteLine("Race {0}", race.Id);

    var buttonHeldDownMs = -1;
    do
    {
        buttonHeldDownMs++;
        var speed = buttonHeldDownMs;
        var travelTime = race.Duration - buttonHeldDownMs;
        var distanceTravelled = speed * travelTime;

        Console.WriteLine("Button {0}ms => travel {1}mm => {2}",
            buttonHeldDownMs,
            distanceTravelled,
            distanceTravelled > race.Distance ? "win" : "lose");

        if (distanceTravelled > race.Distance)
        {
            waysToWinByRace[race] = waysToWinByRace[race] + 1;
        }
    } while (race.Duration > buttonHeldDownMs);
}

foreach (var race in races)
{
    Console.WriteLine("Found {0} ways to win race that lasted {1}ms!", waysToWinByRace[race], race.Duration);
}

var result = waysToWinByRace.Values.Aggregate(1, (a, b) => a * b);
Console.WriteLine(result);
