public record AlmanacMapEntry(long DestRangeStart, long SrcRangeStart, long RangeLen)
{
    public bool Contains(long value)
    {
        return value >= SrcRangeStart && value < SrcRangeStart + RangeLen;
    }

    internal long Map(long value)
    {
        return value + (DestRangeStart - SrcRangeStart);
    }
}
