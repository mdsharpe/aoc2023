// See https://aka.ms/new-console-template for more information

var input = await System.IO.File.ReadAllLinesAsync(args[0]);

var sum = 0;

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

foreach (var line in input)
{
    Digit? dFirst = null;
    Digit? dLast = null;

    if (line.Any(char.IsDigit))
    {
        dFirst = new(
            line.IndexOf(line.First(char.IsDigit)),
            line.First(char.IsDigit));

        dLast = new(
            line.LastIndexOf(line.Last(char.IsDigit)),
            line.Last(char.IsDigit));
    }

    var sFirst = (from d in spelledDigits
                  let i = line.IndexOf(d.Key)
                  where i != -1
                  orderby i
                  select new Digit(i, d.Value)).FirstOrDefault();

    var sLast = (from d in spelledDigits
                 let i = line.LastIndexOf(d.Key) + (d.Key.Length - 1)
                 where i != -1
                 orderby i
                 select new Digit(i, d.Value)).LastOrDefault();

    var first = new Digit?[] { dFirst, sFirst }.Where(d => d is not null).Cast<Digit>().OrderBy(d => d.Index).First();
    var last = new Digit?[] { dLast, sLast }.Where(d => d is not null).Cast<Digit>().OrderBy(d => d.Index).Last();

    Console.WriteLine("{0} => {1}{2}", line, first.Value, last.Value);
    
    var calibrationValue = int.Parse(new string([first.Value, last.Value]));

    sum += calibrationValue;
}

Console.WriteLine();
Console.WriteLine(sum);

record Digit(int Index, char Value);
