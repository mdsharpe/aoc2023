using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync(args[0]);

var cardRegex = new Regex(@"^Card\s+(\d+):((\s+\d+)+)\s\|((\s+\d+)+)$");

var total = 0;

var cards = (from line in input
             let match = cardRegex.Match(line)
             let cardId = int.Parse(match.Groups[1].Value)
             let winningNumbers = match.Groups[3].Captures
                 .Select(o => o.Value)
                 .Select(int.Parse)
             let numbersYouHave = match.Groups[5].Captures
                 .Select(o => o.Value)
                 .Select(int.Parse)
             select new Card(
                cardId,
                winningNumbers,
                numbersYouHave)).ToList();

foreach (var card in cards)
{
    Console.WriteLine($"Card {card.Id} is worth {card.GetWorth()}.");
    total += card.GetWorth();
}

Console.WriteLine($"Total: {total}.");
