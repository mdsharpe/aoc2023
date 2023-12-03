
class SchematicNumber
{
    private readonly List<Coordinate> _coordinates = new();
    private readonly List<char> _chars = new();
    public readonly Dictionary<Coordinate, char> _adjacents = new();

    public void Add(Coordinate coord, char c)
    {
        _coordinates.Add(coord);
        _chars.Add(c);
        Length++;
        Value = int.Parse(new string(_chars.ToArray()));
    }

    public void AddAdjacents(IEnumerable<(Coordinate coord, char c)> adjacents)
    {
        foreach (var a in adjacents)
        {
            _adjacents.TryAdd(a.coord, a.c);
        }
    }

    public int Length { get; private set; }
    public int Value { get; private set; }
    public bool HasAdjacent => _adjacents.Any();
}
