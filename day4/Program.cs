
using System.Collections.Immutable;

var input = await File.ReadAllLinesAsync(args[0]);
var cards = input.Select(Card.Parse).ToImmutableDictionary(o => o.Id, o => o);

var totalWorth = 0;

foreach (var card in cards.Values)
{
    totalWorth += card.GetWorth();
}

Console.WriteLine($"Total worth: {totalWorth}.");

var queue = new Queue<Card>(cards.Values.OrderBy(o => o.Id));

var cardCountIncludingCopies = 0;

while (queue.Count != 0)
{
    var card = queue.Dequeue();
    cardCountIncludingCopies++;

    var winCount = card.CountWins();
    var cardsToCopy = Enumerable.Range(card.Id + 1, winCount).Select(o => cards[o]);

    foreach (var copy in cardsToCopy)
    {
        queue.Enqueue(copy);
    }
}

Console.WriteLine($"Total cards including copies: {cardCountIncludingCopies}.");
