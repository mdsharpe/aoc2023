var input = await File.ReadAllLinesAsync(args[0]);

var hands = input.Select(Hand.Parse).ToArray();
var handComparer = new HandComparer();

var orderedHands = hands
    .OrderBy(hand => hand, handComparer)
    .ToArray();

var winnings = 0;

for (var rank = 1; rank <= orderedHands.Length; rank++)
{
    var hand = orderedHands[rank - 1];
    Console.WriteLine($"{rank}: {hand}");
    winnings += hand.BidAmount * rank;
}

Console.WriteLine($"Winnings: {winnings}");
