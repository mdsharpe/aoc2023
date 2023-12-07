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

    public HandKind GetKind()
    {
        var cardKinds = Cards.Select(card => card.Kind).ToImmutableHashSet();
        var cardCounts = Cards.GroupBy(card => card.Kind).Select(group => group.Count()).ToImmutableHashSet();

        if (cardKinds.Count == 1)
        {
            return HandKind.FiveOfAKind;
        }

        if (cardKinds.Count == 2)
        {
            if (cardCounts.Any(o => o == 4))
            {
                return HandKind.FourOfAKind;
            }

            return HandKind.FullHouse;
        }

        if (cardKinds.Count == 3)
        {
            if (cardCounts.Any(o => o == 3))
            {
                return HandKind.ThreeOfAKind;
            }

            return HandKind.TwoPair;
        }

        if (cardCounts.Any(o => o == 2))
        {
            return HandKind.OnePair;
        }

        return HandKind.HighCard;
    }
}
