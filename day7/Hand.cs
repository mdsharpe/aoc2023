using System.Collections.Immutable;

internal record Hand
{
    private Hand(IEnumerable<Card> cards, int bidAmount)
    {
        Cards = cards.ToImmutableArray();
        BidAmount = bidAmount;
    }

    public ImmutableArray<Card> Cards { get; }
    public int BidAmount { get; }

    public static Hand Parse(string input)
    {
        var parts = input.Split(" ");
        var cards = parts[0].Select(Card.Parse).ToArray();
        var bidAmount = int.Parse(parts[1]);
        return new Hand(cards, bidAmount);
    }

    public override string ToString() => $"{string.Join("", Cards)} {BidAmount}";
}
