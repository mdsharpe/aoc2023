using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var cardRegex = new Regex(@"^Card\s+(\d+):((\s+\d+)+)\s\|((\s+\d+)+)$");

var total = 0;

foreach (var line in input)
{
    var match = cardRegex.Match(line);

    if (!match.Success) throw new InvalidOperationException();

    var cardId = int.Parse(match.Groups[1].Value);

    var winningNumbers = match.Groups[3].Captures
        .Select(o => o.Value)
        .Select(int.Parse)
        .ToHashSet();

    var numbersYouHave = match.Groups[5].Captures
        .Select(o => o.Value)
        .Select(int.Parse)
        .ToHashSet();

    var cardWorth = 0;

    foreach (var number in numbersYouHave)
    {
        if (winningNumbers.Contains(number))
        {
            cardWorth = cardWorth == 0 ? 1 : (cardWorth * 2);
        }
    }

    Console.WriteLine($"Card {cardId} is worth {cardWorth}.");
    total += cardWorth;
}

Console.WriteLine($"Total: {total}.");
