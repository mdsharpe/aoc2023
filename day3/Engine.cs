

class Engine
{
    private readonly Dictionary<Coordinate, char> _schematic = new();
    private readonly int _width;
    private readonly int _height;

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
        _height = input.Length;
        _width = input.First().Length;

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                _schematic.Add(new Coordinate(x, y), input[y][x]);
            }
        }
    }

    public IEnumerable<SchematicNumber> EnumerateNumbers()
    {
        for (var y = 0; y < _height; y++)
        {
            var partNumber = new SchematicNumber();

            for (var x = 0; x <= _width; x++)
            {
                var c = new Coordinate(x, y);

                if (x < _width)
                {
                    if (char.IsDigit(_schematic[c]))
                    {
                        partNumber.Add(c, _schematic[c]);

                        partNumber.AddAdjacents(
                            EnumerateAdjacents(c)
                            .Where(a => a.c != '.')
                            .Where(a => !char.IsDigit(a.c)));

                        continue;
                    }
                }

                if (partNumber.Length > 0)
                {
                    yield return partNumber;
                    partNumber = new();
                }
            }
        }
    }

    public IEnumerable<Coordinate> EnumerateGears()
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var c = new Coordinate(x, y);

                if (_schematic[c] == '*')
                {
                    yield return c;
                }
            }
        }
    }

    private IEnumerable<(Coordinate coord, char c)> EnumerateAdjacents(Coordinate c)
    {
        foreach (var ac in _adjacentDeltas.Select(a => c.Add(a)))
        {
            if (ac.X < 0) continue;
            if (ac.X >= _width) continue;
            if (ac.Y < 0) continue;
            if (ac.Y >= _height) continue;

            yield return (ac, _schematic[ac]);
        }
    }
}
