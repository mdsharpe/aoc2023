using System.Text.RegularExpressions;

static class Parser
{
    public static Connection[] Parse(IEnumerable<string> input)
    {
        var regex = new Regex(@"(?<node>\S\S\S):( (?<con>\S\S\S))+");

        var cmpDict = (from line in input
                       let match = regex.Match(line)
                       let n = match.Groups["node"].Value
                       let c = match.Groups["con"].Captures.Select(c => c.Value).ToArray()
                       select (Component: n, Connections: c))
                    .ToDictionary(o => o.Component, o => o);

        var distinctKeys = cmpDict.Values
            .SelectMany(o => o.Connections)
            .Where(o => !cmpDict.ContainsKey(o))
            .Distinct()
            .ToArray();

        foreach (var key in distinctKeys)
        {
            cmpDict.Add(key, (key, Array.Empty<string>()));
        }

        var components = cmpDict.Values.Select(o => o.Component).ToHashSet();

        var connections = (from o in cmpDict.Values
                           let a = o.Component
                           from c in o.Connections
                           select new Connection(a, c))
                            .Distinct(new ConnectionComparer())
                            .ToArray();

        var testVisitor = new Visitor();
        testVisitor.Visit(connections);
        Console.WriteLine($"Test result: found {testVisitor.Groups.Count} groups");

        return connections;
    }
}