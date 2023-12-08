using System.Collections.Concurrent;
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
    o => new Node(o.Key));

foreach (var (key, (left, right)) in map)
{
    nodeDictionary[key].Left = nodeDictionary[left];
    nodeDictionary[key].Right = nodeDictionary[right];
}

Node[] startNodes;

if (ghostMode)
{
    startNodes = nodeDictionary
        .Where(o => o.Value.IsGhostStart)
        .Select(o => o.Value)
        .ToArray();
}
else
{
    startNodes = [nodeDictionary["AAA"]];
}

var results = new ConcurrentDictionary<long, ConcurrentDictionary<int, byte>>();
void PushResult(long stepCount, int threadIndex)
{
    results.TryAdd(stepCount, []);
    results[stepCount].TryAdd(threadIndex, 0);
}
Thread[] threads = new Thread[startNodes.Length];
CancellationTokenSource cts = new();
var startTime = DateTimeOffset.Now;

for (var i = 0; i < startNodes.Length; i++)
{
    threads[i] = new Thread((object? state) =>
    {
        object[] parameters = (object[])state!;
        var threadIndex = (int)parameters[0];
        var node = (Node)parameters[1];

        var stepCount = 0L;
        int currentTurnIndex = 0;
        var complete = false;

        do
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }

            node = turns[currentTurnIndex] switch
            {
                'L' => node.Left ?? throw new Exception("Uh oh"),
                'R' => node.Right ?? throw new Exception("Uh oh"),
                _ => throw new Exception("Invalid turn"),
            };
            stepCount++;

            if (ghostMode)
            {
                if (node.IsGhostEnd)
                {
                    PushResult(stepCount, threadIndex);
                }
            }
            else
            {
                complete = node.Key == "ZZZ";
                PushResult(stepCount, threadIndex);

                if (complete)
                {
                    return;
                }
            }

            currentTurnIndex++;
            if (currentTurnIndex >= turns.Length)
            {
                currentTurnIndex = 0;
            }
        } while (!complete);
    });
    threads[i].Start(new object[] { i, startNodes[i] });
}

long? finalResult = null;
var allTimeBest = 0;
var allTimeCleaned = 0L;
var maxStepSeenByThread = new Dictionary<int, long>();
do
{
    Thread.Sleep(5000);

    var resultStepNumbers = results.Keys.OrderBy(o => o).ToArray();
    var best = 0;
    foreach (var stepNumber in resultStepNumbers)
    {
        var threadsAtStep = results[stepNumber];
        best = Math.Max(threadsAtStep.Count, best);

        if (threadsAtStep.Count == startNodes.Length)
        {
            cts.Cancel();
            finalResult = stepNumber;
            break;
        }

        foreach (var threadIndex in threadsAtStep.Keys)
        {
            if (stepNumber > maxStepSeenByThread.GetValueOrDefault(threadIndex))
            {
                maxStepSeenByThread[threadIndex] = stepNumber;
            }
        }
    }

    var minStepSeenAcrossAllThreads = maxStepSeenByThread.Values.DefaultIfEmpty().Min();

    var cleanedCount = 0L;
    foreach (var stepNumber in resultStepNumbers.Where(o => o < minStepSeenAcrossAllThreads).ToArray())
    {
        results.TryRemove(stepNumber, out var _);
        cleanedCount++;
    }

    var maxStepsTaken = resultStepNumbers.DefaultIfEmpty().Max();
    var rate = Math.Ceiling(maxStepsTaken / (DateTimeOffset.Now - startTime).TotalSeconds);

    var threadsAlive = threads.Where(o => o.IsAlive).Count();

    allTimeBest = Math.Max(best, allTimeBest);
    allTimeCleaned += cleanedCount;

    Console.WriteLine($"S: {maxStepsTaken}\t\tR: {rate}\tT: {threadsAlive}\tB: {best} ({allTimeBest})\tC: {cleanedCount} ({allTimeCleaned})");

} while (finalResult is null);

Console.WriteLine("Final result: {0}", finalResult);
