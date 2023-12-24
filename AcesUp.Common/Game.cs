using System.Net.NetworkInformation;

namespace AcesUp.Common;

public class Game
{
    private readonly Pile[] _piles = new Pile[] { new(), new(), new(), new() };

    public void RunAllSteps(Deck deck)
    {
        DealNewCards(deck);
        MoveCardToEmptyPile();
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

    private void MoveCardToEmptyPile()
    {
        RemoveLowerRankedCards();

        var emptyPiles = _piles.Where(pile => pile.IsEmpty);
        if (!emptyPiles.Any()) return;

        var movablePiles = _piles.Where(pile => pile.Count >= 2);
        if (!movablePiles.Any()) return;

        // Better strategy from wikihow.
        bool isCardMoved = false;
        foreach (var pile in movablePiles)
        {
            var topCard = pile.Peek();
            var secondCard = pile.Skip(1).First();

            if (topCard.Suit == secondCard.Suit && topCard.Rank < secondCard.Rank)
            {
                MoveCard(emptyPiles, pile);
                isCardMoved = true;
                break;
            }

            // if there's an ace, move it
            if (topCard.Rank == Rank.Ace)
            {
                MoveCard(emptyPiles, pile);
                isCardMoved = true;
                break;
            }
        }

        if (!isCardMoved)
        {
            var cardToMove = movablePiles.First().Pop();
            emptyPiles.First().Push(cardToMove);
        }


        MoveCardToEmptyPile();
    }

    private static void MoveCard(IEnumerable<Pile> emptyPiles, Pile? pile)
    {
        var card = pile.Pop();
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