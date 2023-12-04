using System.Collections.Immutable;
using System.Text.RegularExpressions;

internal class Card
{
    private static readonly Regex _regex = new(@"^Card\s+(\d+):((\s+\d+)+)\s\|((\s+\d+)+)$");

    public Card(int id, IEnumerable<int> winningNumbers, IEnumerable<int> numbersYouHave)
    {
        Id = id;
        WinningNumbers = winningNumbers.ToImmutableHashSet();
        NumbersYouHave = numbersYouHave.ToImmutableHashSet();
    }

    public int Id { get; }
    public ImmutableHashSet<int> WinningNumbers { get; }
    public ImmutableHashSet<int> NumbersYouHave { get; }

    public static Card Parse(string text)
    {
        var match = _regex.Match(text);
        if (!match.Success)
        {
            throw new ArgumentException();
        }

        var cardId = int.Parse(match.Groups[1].Value);

        var winningNumbers = match.Groups[3].Captures
            .Select(o => o.Value)
            .Select(int.Parse);

        var numbersYouHave = match.Groups[5].Captures
            .Select(o => o.Value)
            .Select(int.Parse);

        return new Card(cardId, winningNumbers, numbersYouHave);
    }

    public int CountWins() => NumbersYouHave.Count(WinningNumbers.Contains);

    public int GetWorth()
    {
        var worth = 0;
        var winCount = CountWins();

        for (var i = 0; i < winCount; i++)
        {
            worth = worth == 0 ? 1 : (worth * 2);
        }

        return worth;
    }
}
