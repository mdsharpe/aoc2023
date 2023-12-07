using System.Collections.Immutable;

var input = await File.ReadAllLinesAsync(args[0]);
var jIsJoker = args.Length > 1 && args[1].Equals("-j", StringComparison.OrdinalIgnoreCase);
var handComparer = new HandComparer(jIsJoker);
var hands = input.Select(Hand.Parse)
    .OrderBy(hand => hand, handComparer)
    .ToImmutableArray();

var winnings = 0;

for (var rank = 1; rank <= hands.Length; rank++)
{
    var hand = hands[rank - 1];
    Console.WriteLine($"{rank}: {hand}\t{hand.GetKind(jIsJoker)}");
    winnings += hand.BidAmount * rank;
}

Console.WriteLine();
Console.WriteLine($"Winnings: {winnings}");
