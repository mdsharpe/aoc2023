var input = await File.ReadAllLinesAsync(args[0]);
var connections = Parser.Parse(input);

/**
 * Traverse connections
 */

for (int s0 = 0; s0 < connections.Length; s0++)
{
    for (int s1 = 0; s1 < connections.Length; s1++)
    {
        for (int s2 = 0; s2 < connections.Length; s2++)
        {
            if (s0 == s1 || s0 == s2 || s1 == s2)
            {
                continue;
            }

            var connectionsExceptSevered = connections
                .Except([connections[s0], connections[s1], connections[s2]]);

            var visitor = new Visitor();
            visitor.Visit(connectionsExceptSevered);
            Console.WriteLine($"Severing {s0},{s1},{s2} => found {visitor.Groups.Count} groups");
            if (visitor.Groups.Count == 2)
            {
                Environment.Exit(0);
            }
        }
    }
}
