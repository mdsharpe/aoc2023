var input = await File.ReadAllLinesAsync(args[0]);
var connections = Parser.Parse(input);

Parallel.For(
    0,
    connections.Length,
    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
    (s0) =>
    {
        var visitor = new Visitor();

        for (var s1 = 0; s1 < connections.Length; s1++)
        {
            for (var s2 = 0; s2 < connections.Length; s2++)
            {
                if (!(s0 == s1 || s0 == s2 || s1 == s2))
                {
                    var connectionsExceptSevered = connections
                        .Except([connections[s0], connections[s1], connections[s2]]);

                    visitor.Clear();
                    visitor.Visit(connectionsExceptSevered);

                    if (visitor.Groups.Count == 2)
                    {
                        foreach (var g in visitor.Groups)
                        {
                            Console.WriteLine(string.Join(",", g.Count));
                        }

                        Environment.Exit(0);
                    }
                }
            }
        }
    });
