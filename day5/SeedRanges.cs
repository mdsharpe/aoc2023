static class SeedRanges
{
    public static IEnumerable<long> EnumerateSeedsInRanges(IEnumerable<long> seeds)
    {
        var seedsAsRanges = EnumerateSeedRanges(seeds);

        foreach (var seedRange in seedsAsRanges)
        {
            var seedRangeEnd = seedRange.Start + seedRange.Len - 1;

            for (var j = seedRange.Start; j <= seedRangeEnd; j++)
            {
                yield return j;
            }
        }
    }

    public static long CountSeedsInRanges(IEnumerable<long> seeds)
        => EnumerateSeedRanges(seeds).Select(o => o.Len).Sum();

    private static IEnumerable<(long Start, long Len)> EnumerateSeedRanges(IEnumerable<long> seeds)
        => seeds
            .Select((value, index) => new { value, index })
            .GroupBy(o => o.index / 2, o => o.value)
            .Select(group => (Start: group.First(), Len: group.Last()));
}
