var input = await File.ReadAllLinesAsync(args[0]);
var games = GameParser.ParseGames(input);

var targetAmounts = new Dictionary<Color, int>
{
    { Color.red, 12 },
    { Color.green, 13 },
    { Color.blue, 14 },
};

var possibleGameIdSum = 0;
var minimumSetPowerSum = 0;

foreach (var game in games)
{
    var gamePossible = true;

    foreach (var target in targetAmounts)
    {
        var color = target.Key;
        var targetAmount = target.Value;

        var maxDrawn = game.Handfuls.Where(o => o.ContainsKey(color))
            .Select(o => o[color])
            .DefaultIfEmpty()
            .Max();

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

    var minimumByColor = from handful in game.Handfuls
                         from color in handful.Keys
                         group handful[color] by color into g
                         select g.Max();

    minimumSetPowerSum += minimumByColor.Aggregate((a, b) => a * b);
}

Console.WriteLine("Sum of IDs of possible games: {0}", possibleGameIdSum);
Console.WriteLine("Sum of the power of minimum sets: {0}", minimumSetPowerSum);
