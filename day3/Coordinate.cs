
readonly record struct Coordinate(int X, int Y)
{
    internal Coordinate Add(Coordinate other)
        => new(X + other.X, Y + other.Y);
}
