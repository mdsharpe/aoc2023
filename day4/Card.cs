using System.Collections.Immutable;

internal class Card
{
    public Card(int id, IEnumerable<int> winningNumbers, IEnumerable<int> numbersYouHave)
    {
        Id = id;
        WinningNumbers = winningNumbers.ToImmutableHashSet();
        NumbersYouHave = numbersYouHave.ToImmutableHashSet();
    }

    public int Id { get; }
    public ImmutableHashSet<int> WinningNumbers { get; }
    public ImmutableHashSet<int> NumbersYouHave { get; }

    public int GetWorth()
    {
        var worth = 0;
        foreach (var number in NumbersYouHave)
        {
            if (WinningNumbers.Contains(number))
            {
                worth = worth == 0 ? 1 : (worth * 2);
            }
        }

        return worth;
    }
}
