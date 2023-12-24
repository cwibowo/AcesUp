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
        // Add code here

        return false;
    }

    private void MoveCardToEmptyPile()
    {
        // Add code here
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