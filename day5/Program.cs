var input = await File.ReadAllLinesAsync(args[0]);

var almanac = Almanac.Parse(input);

var minLocation = long.MaxValue;

foreach (var seed in almanac.Seeds) {
    var location = almanac.Lookup("seed", "location", seed);
    minLocation = Math.Min(minLocation, location);
}

Console.WriteLine($"Min location: {minLocation}");
