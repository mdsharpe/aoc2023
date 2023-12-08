using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var ghostMode = args.Length > 1 && args[1] == "-g";

var turns = input[0];

var mapEntryRegex = new Regex(@"(?<node>\w\w\w) = \((?<l>\w\w\w), (?<r>\w\w\w)\)");
var map = input.Skip(2).Select(line =>
{
    var match = mapEntryRegex.Match(line);
    return new { Key = match.Groups["node"].Value, Left = match.Groups["l"].Value, Right = match.Groups["r"].Value };
}).ToDictionary(o => o.Key, o => (o.Left, o.Right));

var nodeDictionary = map.ToDictionary(
    o => o.Key,
    o => new Node { Key = o.Key });

foreach (var (key, (left, right)) in map)
{
    nodeDictionary[key].Left = nodeDictionary[left];
    nodeDictionary[key].Right = nodeDictionary[right];
}

Node[] currentLocations;

if (ghostMode)
{
    currentLocations = nodeDictionary
        .Where(o => o.Key.EndsWith('A'))
        .Select(o => o.Value)
        .ToArray();
}
else
{
    currentLocations = [nodeDictionary["AAA"]];
}

var currentTurnIndex = 0;
var stepCount = 0L;
var startTime = DateTimeOffset.Now;

Timer timerConsole = new((_) =>
{
    var rate = Math.Ceiling(stepCount / (DateTimeOffset.Now - startTime).TotalSeconds);
    Console.WriteLine($"Step {stepCount}... ({rate} steps/sec)");
}, null, 0, 1000);

while (true)
{
    var turn = turns[currentTurnIndex];

    var complete = true;

    var nextNodes = currentLocations
        .Select(currentNode =>
        {
            var nextNode = turn switch
            {
                'L' => currentNode.Left ?? throw new Exception("Uh oh"),
                'R' => currentNode.Right ?? throw new Exception("Uh oh"),
                _ => throw new Exception("Invalid turn"),
            };

            if (complete)
            {
                if (ghostMode)
                {
                    if (!nextNode.Key.EndsWith('Z'))
                    {
                        complete = false;
                    }
                }
                else
                {
                    if (nextNode.Key != "ZZZ")
                    {
                        complete = false;
                    }
                }
            }

            return nextNode;
        }).ToArray();
        
    stepCount++;

    if (complete)
    {
        break;
    }
    else
    {
        currentLocations = nextNodes;
    }

    currentTurnIndex++;
    if (currentTurnIndex >= turns.Length)
    {
        currentTurnIndex = 0;
    }
}

timerConsole.Dispose();

Console.WriteLine("Steps: {0}", stepCount);
