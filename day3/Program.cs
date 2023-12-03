var input = await File.ReadAllLinesAsync(args[0]);
var engine = new Engine(input);

var partNumbersSum = 0;
var allNumbersSum = 0;
var gearRatiosSum = 0;
SchematicNumber partNumber;
Coordinate c;

for (var y = 0; y < engine.SchematicHeight; y++)
{
    partNumber = new();

    for (var x = 0; x <= engine.SchematicWidth; x++)
    {
        c = new Coordinate(x, y);

        if (x < engine.SchematicWidth)
        {
            if (char.IsDigit(engine.Schematic[c]))
            {
                partNumber.Add(c, engine.Schematic[c]);

                partNumber.AddAdjacents(
                    engine.EnumerateAdjacents(c)
                    .Where(a => a.c != '.')
                    .Where(a => !char.IsDigit(a.c)));

                continue;
            }

            if (engine.Schematic[c] == '*')
            {
                gearRatiosSum += engine.EnumerateAdjacents(c)
                    .Where(a => char.IsDigit(a.c))
                    .Select(a => int.Parse(a.c.ToString()))
                    .Aggregate((a, b) => a * b);
            }
        }

        if (partNumber.Length > 0)
        {
            if (partNumber.HasAdjacent)
            {
                partNumbersSum += partNumber.Value;
            }

            allNumbersSum += partNumber.Value;

            Console.WriteLine($"{partNumber.Value}: {new string(partNumber._adjacents.Select(o => o.Value).ToArray())}");

            partNumber = new();
        }
    }
}

Console.WriteLine();
Console.WriteLine("Part numbers sum: {0}", partNumbersSum);
Console.WriteLine("All numbers sum:  {0}", allNumbersSum);
Console.WriteLine("Gear ratios sum:  {0}", gearRatiosSum);
