
class SchematicNumber
{
    private readonly List<Coordinate> _coordinates = new();
    private readonly List<char> _chars = new();
    public readonly Dictionary<Coordinate, char> Adjacents = new();

    public int Length { get; private set; }
    public int Value { get; private set; }

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
            Adjacents.TryAdd(a.coord, a.c);
        }
    }
}
