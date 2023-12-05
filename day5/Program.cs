var input = await File.ReadAllLinesAsync(args[0]);

var almanac = Almanac.Parse(input);

Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(almanac));
