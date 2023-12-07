internal class HandComparer : IComparer<Hand>
{
    public int Compare(Hand? a, Hand? b)
    {
        if (a is null || b is null)
        {
            throw new InvalidOperationException();
        }

        var aKind = a.GetKind();
        var bKind = b.GetKind();

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
            var aCard = a.Cards[i];
            var bCard = b.Cards[i];

            if (aCard.Kind > bCard.Kind)
            {
                return 1;
            }
            else if (aCard.Kind < bCard.Kind)
            {
                return -1;
            }
        }

        return 0;
    }
}
