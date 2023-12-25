using System.Data;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var regex = new Regex(@"(?<node>\S\S\S):( (?<con>\S\S\S))+");

var components = (from line in input
                  let match = regex.Match(line)
                  let n = match.Groups["node"].Value
                  let c = match.Groups["con"].Captures.Select(c => c.Value).ToArray()
                  select (Component: new Component(n), Connections: c))
            .ToDictionary(o => o.Component.Key, o => o);

var distinctKeys = components.Values
    .SelectMany(o => o.Connections)
    .Where(o => !components.ContainsKey(o))
    .Distinct()
    .ToArray();

foreach (var key in distinctKeys)
{
    components.Add(key, (new Component(key), Array.Empty<string>()));
}

foreach (var (component, connections) in components.Values)
{
    foreach (var connection in connections)
    {
        component.Connections.Add(components[connection].Component);
    }
}

foreach (var (component, _) in components.Values.OrderBy(o => o.Component.Key))
{
    Console.WriteLine($"{component.Key}: {string.Join(", ", component.Connections.Select(c => c.Key))}");
}