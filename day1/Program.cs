// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var spelledDigits = new Dictionary<string, char>
{
    ["one"] = '1',
    ["two"] = '2',
    ["three"] = '3',
    ["four"] = '4',
    ["five"] = '5',
    ["six"] = '6',
    ["seven"] = '7',
    ["eight"] = '8',
    ["nine"] = '9',
};

var sum = 0;

foreach (var line in input)
{
    var numerics = from d in Enumerable.Range(1, 9).Select(o => o.ToString())
                   from m in Regex.Matches(line, Regex.Escape(d))
                   select new Digit(m.Index, d[0]);

    var spelled = from d in spelledDigits
                  from m in Regex.Matches(line, Regex.Escape(d.Key))
                  select new Digit(m.Index, d.Value);

    var all = Enumerable.Concat(numerics, spelled).OrderBy(d => d.Index).ToArray();

    var first = all.First();
    var last = all.Last();

    var calibrationString = new string([first.Value, last.Value]);

    Console.WriteLine("{0} => {1} => {2}",
        line,
        string.Join(",", all.Select(d => d.Value)),
        calibrationString);

    var calibrationValue = int.Parse(calibrationString);
    sum += calibrationValue;
}

Console.WriteLine();
Console.WriteLine(sum);

record Digit(int Index, char Value);
