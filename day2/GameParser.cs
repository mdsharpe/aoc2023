using System.Text.Json;
using System.Text.RegularExpressions;

static class GameParser
{
    private static Regex PrefixRegex = new(@"Game (\d+):");
    private static Regex BallRegex = new(@"(\d+) (red|green|blue)");

    public static List<Game> ParseGames(IEnumerable<string> input)
    {
        var games = new List<Game>();

        foreach (var line in input)
        {
            var prefixMatch = PrefixRegex.Match(line);

            if (!prefixMatch.Success) throw new Exception();

            var gameId = int.Parse(prefixMatch.Groups[1].Value);

            var ballsString = line.Substring($"Game {gameId}: ".Length);

            ICollection<IDictionary<Color, int>> handfuls = new List<IDictionary<Color, int>>();

            foreach (var handfulString in ballsString.Split(';'))
            {
                var handful = new Dictionary<Color, int>();

                foreach (var ball in handfulString.Split(','))
                {
                    var ballMatch = BallRegex.Match(ball);
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
        }

        return games;
    }
}
