internal readonly record struct Card(CardKind Kind)
{
    public static Card Parse(char c) => c switch
    {
        '2' => new Card(CardKind.Two),
        '3' => new Card(CardKind.Three),
        '4' => new Card(CardKind.Four),
        '5' => new Card(CardKind.Five),
        '6' => new Card(CardKind.Six),
        '7' => new Card(CardKind.Seven),
        '8' => new Card(CardKind.Eight),
        '9' => new Card(CardKind.Nine),
        'T' => new Card(CardKind.Ten),
        'J' => new Card(CardKind.Jack),
        'Q' => new Card(CardKind.Queen),
        'K' => new Card(CardKind.King),
        'A' => new Card(CardKind.Ace),
        _ => throw new Exception("Invalid card")
    };

    public override string ToString() => Kind switch
    {
        CardKind.Ace => "A",
        CardKind.Two => "2",
        CardKind.Three => "3",
        CardKind.Four => "4",
        CardKind.Five => "5",
        CardKind.Six => "6",
        CardKind.Seven => "7",
        CardKind.Eight => "8",
        CardKind.Nine => "9",
        CardKind.Ten => "T",
        CardKind.Jack => "J",
        CardKind.Queen => "Q",
        CardKind.King => "K",
        _ => throw new NotImplementedException()
    };
}