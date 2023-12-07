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

    public HandKind GetKind(bool jIsJoker)
    {
        if (!jIsJoker)
        {
            return GetKindInternal();
        }

        var bestKind = HandKind.HighCard;

        var substitutions = Enum.GetValues<CardKind>().Where(kind => kind != CardKind.JackOrJoker);

        foreach (var jokerSubstitution in substitutions)
        {
            var kind = GetKindInternal(jokerSubstitution);

            if (kind > bestKind)
            {
                bestKind = kind;
            }
        }

        return bestKind;
    }

    private HandKind GetKindInternal(CardKind? jokerSubstitution = null)
    {
        var cards = jokerSubstitution is null
            ? Cards
            : Cards.Select(card => card.Kind == CardKind.JackOrJoker ? new Card(jokerSubstitution.Value) : card).ToImmutableArray();

        var cardKinds = cards.Select(card => card.Kind).ToImmutableHashSet();
        var cardCounts = cards.GroupBy(card => card.Kind).Select(group => group.Count()).ToImmutableHashSet();

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
