using System.Diagnostics.CodeAnalysis;

class ConnectionComparer : IEqualityComparer<Connection>
{
    public bool Equals(Connection? x, Connection? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return (x.A == y.A && x.B == y.B)
            || (x.A == y.B && x.B == y.A);
    }

    public int GetHashCode([DisallowNull] Connection obj)
    {
        if (obj.A.CompareTo(obj.B) < 0)
        {
            return HashCode.Combine(obj.A, obj.B);
        }
        else
        {
            return HashCode.Combine(obj.B, obj.A);
        }
    }
}
