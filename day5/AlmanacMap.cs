using System.Collections.Immutable;

public record AlmanacMap
{
    public AlmanacMap(string src, string dest, IEnumerable<AlmanacMapEntry> entries)
    {
        Src = src;
        Dest = dest;
        Entries = entries.ToImmutableArray();
    }

    public string Src { get; }
    public string Dest { get; }
    public IImmutableList<AlmanacMapEntry> Entries { get; }

    public long Map(long value)
    {
        var entry = Entries.FirstOrDefault(e => e.Contains(value));
        return entry is not null ? entry.Map(value) : value;
    }
}
