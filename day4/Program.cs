var input = await File.ReadAllLinesAsync(args[0]);
var cards = input.Select(Card.Parse).ToList();

var total = 0;

foreach (var card in cards)
{
    Console.WriteLine($"Card {card.Id} is worth {card.GetWorth()}.");
    total += card.GetWorth();
}

Console.WriteLine($"Total: {total}.");
