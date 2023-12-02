using System.Text.Json;

var input = await File.ReadAllLinesAsync(args[0]);

var games = GameParser.ParseGames(input);

var targetAmounts = new Dictionary<Color, int>
{
    { Color.red, 12 },
    { Color.green, 13 },
    { Color.blue, 14 },
};

var possibleGameIdSum = 0;

foreach (var game in games)
{
    Console.WriteLine(JsonSerializer.Serialize(game));
    var gamePossible = true;

    foreach (var target in targetAmounts)
    {
        var color = target.Key;
        var targetAmount = target.Value;

        var maxDrawn = game.Handfuls.Where(o => o.ContainsKey(color))
            .Select(o => o[color])
            .DefaultIfEmpty()
            .Max();

        Console.WriteLine($"{color}: {maxDrawn} / {targetAmount}");

        if (maxDrawn > targetAmount)
        {
            gamePossible = false;
            break;
        }
    }

    if (gamePossible)
    {
        possibleGameIdSum += game.Id;
    }
}

Console.WriteLine(possibleGameIdSum);
