var input = await File.ReadAllLinesAsync(args[0]);

var hands = input.Select(Hand.Parse).ToArray();
var handComparer = new HandComparer();

var orderedHands = hands
    .OrderByDescending(hand => hand, handComparer)
    .ToArray();

foreach (var hand in orderedHands)
{
    Console.WriteLine(hand.ToString());
}
