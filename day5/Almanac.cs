using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

internal class Almanac
{
    private static readonly Regex RegexMapTitle = new Regex(@"(?<src>\w+)-to-(?<dest>\w+) map:");
    private static readonly Regex RegexMapEntry = new Regex(@"(?<destRangeStart>\d+) (?<srcRangeStart>\d+) (?<rangeLen>\d+)");

    public required IImmutableSet<long> Seeds { get; init; }
    public required IImmutableList<AlmanacMap> Maps { get; init; }

    public static Almanac Parse(IList<string> input)
    {
        var seeds = input[0].Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToImmutableHashSet();

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
            Seeds = seeds,
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
