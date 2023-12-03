class Engine
{
    public Dictionary<Coordinate, char> Schematic { get; private set; } = new();
    public int SchematicWidth { get; private set; }
    public int SchematicHeight { get; private set; }

    private readonly Coordinate[] _adjacentDeltas = [
        new Coordinate(-1,-1),
        new Coordinate(0,-1),
        new Coordinate(1,-1),
        new Coordinate(-1,0),
        new Coordinate(1,0),
        new Coordinate(-1,1),
        new Coordinate(0,1),
        new Coordinate(1,1)
    ];

    public Engine(string[] input)
    {
        SchematicHeight = input.Length;
        SchematicWidth = input.First().Length;

        for (var y = 0; y < SchematicHeight; y++)
        {
            for (var x = 0; x < SchematicWidth; x++)
            {
                Schematic.Add(new Coordinate(x, y), input[y][x]);
            }
        }
    }

    public IEnumerable<(Coordinate coord, char c)> EnumerateAdjacents(Coordinate c)
    {
        foreach (var ac in _adjacentDeltas.Select(a => c.Add(a)))
        {
            if (ac.X < 0) continue;
            if (ac.X >= SchematicWidth) continue;
            if (ac.Y < 0) continue;
            if (ac.Y >= SchematicHeight) continue;

            yield return (ac, Schematic[ac]);
        }
    }
}
