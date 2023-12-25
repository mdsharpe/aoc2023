using System.Text.Json;

class Visitor
{
    public List<HashSet<string>> Groups { get; } = [];

    public void Visit(IEnumerable<Connection> connections)
    {
        foreach (var c in connections)
        {
            Console.WriteLine(JsonSerializer.Serialize(this));
            var group = Groups.SingleOrDefault(o => o.Contains(c.A) || o.Contains(c.B));

            if (group is null)
            {
                Groups.Add([c.A, c.B]);
            }
            else
            {
                group.Add(c.A);
                group.Add(c.B);
            }
        }
    }
}
