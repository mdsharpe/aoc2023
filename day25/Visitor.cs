class Visitor
{
    public List<HashSet<string>> Groups { get; } = [];

    public void Clear()
    {
        Groups.Clear();
    }

    public void Visit(IEnumerable<Connection> connections)
    {
        foreach (var c in connections)
        {
            var groups = Groups.Where(o => o.Contains(c.A) || o.Contains(c.B)).ToArray();

            if (groups.Length == 0)
            {
                Groups.Add([c.A, c.B]);
            }
            else if (groups.Length == 1)
            {
                groups[0].Add(c.A);
                groups[0].Add(c.B);
            }
            else if (groups.Length == 2)
            {
                var group = groups[0];
                group.UnionWith(groups[1]);
                Groups.Remove(groups[1]);
                groups[0].Add(c.A);
                groups[0].Add(c.B);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
