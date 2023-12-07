internal class HandComparer : IComparer<Hand>
{
    private readonly bool _jIsJoker;

    public HandComparer(bool jIsJoker)
    {
        _jIsJoker = jIsJoker;
    }

    public int Compare(Hand? a, Hand? b)
    {
        if (a is null || b is null)
        {
            throw new InvalidOperationException();
        }

        var aKind = a.GetKind(_jIsJoker);
        var bKind = b.GetKind(_jIsJoker);

        if (aKind > bKind)
        {
            return 1;
        }
        else if (aKind < bKind)
        {
            return -1;
        }

        for (var i = 0; i < a.Cards.Length; i++)
        {
            var aCard = a.Cards[i].Kind;
            var bCard = b.Cards[i].Kind;

            if (_jIsJoker)
            {
                aCard = aCard == CardKind.JackOrJoker ? CardKind.Weakest : aCard;
                bCard = bCard == CardKind.JackOrJoker ? CardKind.Weakest : bCard;
            }

            if (aCard > bCard)
            {
                return 1;
            }
            else if (aCard < bCard)
            {
                return -1;
            }
        }

        return 0;
    }
}
