// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var prefixRegex = new Regex(@"Game (\d+):");
var ballRegex = new Regex(@"(\d+) (red|green|blue)");

var games = new List<Game>();

foreach (var line in input)
{
    var prefixMatch = prefixRegex.Match(line);

    if (!prefixMatch.Success) throw new Exception();

    var gameId = int.Parse(prefixMatch.Groups[1].Value);

    var ballsString = line.Substring($"Game {gameId}: ".Length);

    ICollection<IDictionary<Color, int>> handfuls = new List<IDictionary<Color, int>>();

    foreach (var handfulString in ballsString.Split(';'))
    {
        var handful = new Dictionary<Color, int>();

        foreach (var ball in handfulString.Split(','))
        {
            var ballMatch = ballRegex.Match(ball);
            if (!ballMatch.Success) throw new Exception();
            handful.Add(
                Enum.Parse<Color>(ballMatch.Groups[2].Value),
                int.Parse(ballMatch.Groups[1].Value)
            );
        }

        handfuls.Add(handful);
    }

    var game = new Game(gameId, handfuls);
    games.Add(game);

    Console.WriteLine($"Game {gameId}:");
    Console.WriteLine(JsonSerializer.Serialize(game));
    Console.WriteLine();
}
