// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var input = await System.IO.File.ReadAllLinesAsync(args[0]);

var sum = 0;

var regexDigit = new Regex(@"\d");

foreach (var line in input)
{
    var foo = from c in line
              where regexDigit.Match(c.ToString()).Success
              select c;

    sum += int.Parse(new string([foo.First(), foo.Last()]));
}

Console.WriteLine(sum);
