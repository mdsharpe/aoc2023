using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

internal class Almanac
{
    private static readonly Regex RegexMapTitle = new Regex(@"(?<src>\w+)-to-(?<dest>\w+) map:");
    private static readonly Regex RegexMapEntry = new Regex(@"(?<destRangeStart>\d+) (?<srcRangeStart>\d+) (?<rangeLen>\d+)");

    public required IImmutableSet<int> Seeds { get; init; }
    public required IImmutableList<Map> Maps { get; init; }

    public static Almanac Parse(IList<string> input)
    {
        var seeds = input[0].Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToImmutableHashSet();

        List<Map> maps = [];
        string? src = null;
        string? dest = null;
        List<MapEntry> entries = [];

        foreach (var line in input.Skip(1))
        {
            if (RegexMapTitle.IsMatch(line))
            {
                if (src is not null && dest is not null)
                {
                    maps.Add(new Map(src, dest, entries));
                }

                var match = RegexMapTitle.Match(line);
                src = match.Groups["src"].Value;
                dest = match.Groups["dest"].Value;
                entries.Clear();
            }
            else if (RegexMapEntry.IsMatch(line))
            {
                var match = RegexMapEntry.Match(line);
                var destRangeStart = int.Parse(match.Groups["destRangeStart"].Value);
                var srcRangeStart = int.Parse(match.Groups["srcRangeStart"].Value);
                var rangeLen = int.Parse(match.Groups["rangeLen"].Value);
                entries.Add(new MapEntry(destRangeStart, srcRangeStart, rangeLen));
            }
        }

        return new Almanac
        {
            Seeds = seeds,
            Maps = maps.ToImmutableArray()
        };
    }

    public record Map
    {
        public Map(string src, string dest, IEnumerable<MapEntry> entries)
        {
            Src = src;
            Dest = dest;
            Entries = entries.ToImmutableArray();
        }

        public string Src { get; }
        public string Dest { get; }
        public IImmutableList<MapEntry> Entries { get; }
    }

    public readonly record struct MapEntry(int DestRangeStart, int SrcRangeStart, int RangeLen);
}