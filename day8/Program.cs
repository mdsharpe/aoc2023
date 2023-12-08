using System.Collections.Immutable;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var turns = input[0].Select(c => c switch
    {
        'L' => Turn.Left,
        'R' => Turn.Right,
        _ => throw new Exception("Invalid turn"),
    }).ToImmutableArray();

var mapEntryRegex = new Regex(@"(?<node>\w\w\w) = \((?<l>\w\w\w), (?<r>\w\w\w)\)");
var map = input.Skip(2).Select(line =>
{
    var match = mapEntryRegex.Match(line);
    return new { Key = match.Groups["node"].Value, Left = match.Groups["l"].Value, Right = match.Groups["r"].Value };
}).ToDictionary(o => o.Key, o => (o.Left, o.Right));

Node GetNode(string key)
{
    Node node;
    var (left, right) = map[key];
    node = new Node(
        key,
        left != key ? GetNode(left) : null,
        right != key ? GetNode(right) : null);
    return node;
}

var root = GetNode("AAA");

Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(root));
