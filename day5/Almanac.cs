using System.Collections.Immutable;
using System.Text.RegularExpressions;

internal class Almanac
{
    private static readonly Regex RegexMapTitle = new Regex(@"(?<src>\w+)-to-(?<dest>\w+) map:");
    private static readonly Regex RegexMapEntry = new Regex(@"(?<destRangeStart>\d+) (?<srcRangeStart>\d+) (?<rangeLen>\d+)");

    public required IEnumerable<long> SeedsEnumerable { get; init; }
    public required long SeedCount { get; init; }
    public required IImmutableDictionary<string, AlmanacMap> Maps { get; init; }

    public static Almanac Parse(IList<string> input, bool interpretSeedsAsRanges = false)
    {
        var seedsRaw = input[0].Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToImmutableArray();
        IEnumerable<long> seeds;
        long seedCount;

        if (interpretSeedsAsRanges)
        {
            seedCount = SeedRanges.CountSeedsInRanges(seedsRaw);
            seeds = SeedRanges.EnumerateSeedsInRanges(seedsRaw);
        }
        else
        {
            seeds = seedsRaw;
            seedCount = seedsRaw.Length;
        }

        List<AlmanacMap> maps = [];
        string? src = null;
        string? dest = null;
        List<AlmanacMapEntry> entries = [];

        void TryFlushMap()
        {
            if (src is not null && dest is not null)
            {
                maps.Add(new AlmanacMap(src, dest, entries));
            }
        }

        foreach (var line in input.Skip(1))
        {
            if (RegexMapTitle.IsMatch(line))
            {
                TryFlushMap();

                var match = RegexMapTitle.Match(line);
                src = match.Groups["src"].Value;
                dest = match.Groups["dest"].Value;
                entries.Clear();
            }
            else if (RegexMapEntry.IsMatch(line))
            {
                var match = RegexMapEntry.Match(line);
                var destRangeStart = long.Parse(match.Groups["destRangeStart"].Value);
                var srcRangeStart = long.Parse(match.Groups["srcRangeStart"].Value);
                var rangeLen = long.Parse(match.Groups["rangeLen"].Value);
                entries.Add(new AlmanacMapEntry(destRangeStart, srcRangeStart, rangeLen));
            }
        }

        TryFlushMap();

        return new Almanac
        {
            SeedsEnumerable = seeds,
            SeedCount = seedCount,
            Maps = maps.ToImmutableDictionary(m => m.Src, m => m)
        };
    }

    public long Lookup(string src, string dest, long value)
    {
        AlmanacMap map;

        do
        {
            map = Maps[src];

            value = map.Map(value);

            if (map.Dest == dest)
            {
                return value;
            }

            src = map.Dest;
        } while (true);
    }
}
