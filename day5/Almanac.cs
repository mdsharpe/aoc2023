using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

internal class Almanac
{
    private static readonly Regex RegexMapTitle = new Regex(@"(?<src>\w+)-to-(?<dest>\w+) map:");
    private static readonly Regex RegexMapEntry = new Regex(@"(?<destRangeStart>\d+) (?<srcRangeStart>\d+) (?<rangeLen>\d+)");

    public required IImmutableSet<long> Seeds { get; init; }
    public required IImmutableList<AlmanacMap> Maps { get; init; }

    public static Almanac Parse(IList<string> input, bool seedsAsRanges = false)
    {
        var seeds = input[0].Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        if (seedsAsRanges)
        {
            var newSeeds = new List<long>();

            for (var i = 0; i < seeds.Length; i += 2)
            {
                var start = seeds[i];
                var len = seeds[i + 1];

                for (var j = start; j <= start + len; j++)
                {
                    newSeeds.Add(j);
                }
            }

            seeds = newSeeds.ToArray();
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
            Seeds = seeds.ToImmutableHashSet(),
            Maps = maps.ToImmutableArray()
        };
    }

    public long Lookup(string src, string dest, long value)
    {
        AlmanacMap map;

        do
        {
            map = Maps.Single(m => m.Src == src);

            value = map.Map(value);

            if (map.Dest == dest)
            {
                return value;
            }

            src = map.Dest;
        } while (true);
    }
}
