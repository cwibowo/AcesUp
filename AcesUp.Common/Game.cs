namespace AcesUp.Common;

public class Game
{
    private readonly Pile[] _piles = new Pile[] { new(), new(), new(), new() };

    public void RunAllSteps(Deck deck)
    {
        DealNewCards(deck);
        RemoveLowerRankedCards();
        while (AreThereMovesToEmptyPiles())
        {
            MoveCardToEmptyPile();
            RemoveLowerRankedCards();
        }
    }

    private void DealNewCards(Deck deck)
    {
        foreach (var pile in _piles)
        {
            pile.Push(deck.Take());
        }
    }

    private void RemoveLowerRankedCards()
    {
        bool searching = true;
        while (searching)
        {
            searching = false;

            IDictionary<Suit, IList<Pile>> pileGroup = new Dictionary<Suit, IList<Pile>>();
            foreach (var pile in _piles.Where(pile => !pile.IsEmpty))
            {
                var suit = pile.Peek().Suit;
                if (!pileGroup.ContainsKey(suit)) pileGroup.Add(suit, new List<Pile>());

                pileGroup[suit].Add(pile);
            }

            foreach (var groupedPile in pileGroup)
            {
                if (groupedPile.Value.Count >= 2)
                {
                    var pileWithLowestRank = groupedPile.Value.OrderBy(pile => pile.Peek().Rank).First();
                    pileWithLowestRank.Pop();
                    searching = true;
                }
            }
        }
    }

    private bool AreThereMovesToEmptyPiles()
    {
        // Can only move if there's any empty piles.
        bool hasEmptyPiles = _piles.Any(pile => pile.IsEmpty);
        if (!hasEmptyPiles) return false;

        // Can only move if any piles have at least 2 cards.
        bool hasMovablePiles = _piles.Any(pile => pile.Count >= 2);

        return hasMovablePiles;
    }

    private void MoveCardToEmptyPile()
    {
        var emptyPiles = _piles.Where(pile => pile.IsEmpty);
        var movablePiles = _piles.Where(pile => pile.Count >= 2);

        // Simple removal strategy
        var card = movablePiles.First().Pop();
        emptyPiles.First().Push(card);
    }

    public bool IsGameWon()
    {
        foreach (var pile in _piles)
        {
            if (pile.Count != 1)
            {
                return false;
            }

            if (pile.Peek().Rank != Rank.Ace)
            {
                return false;
            }
        }

        return true;
    }

    public void PrintPiles()
    {
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Pile {i}: {_piles[i]}");
        }

        Console.WriteLine();
    }

}