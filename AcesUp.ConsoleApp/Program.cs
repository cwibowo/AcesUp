using AcesUp.Common;

namespace AcesUp.ConsoleApp
{
    internal static class Program
    {
        static void Main()
        {
            // Setting output encoding so the suit of the cards can be displayed in the console.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var r = new Random();

            var deck = Deck.CreateShuffledDeck(r);

            var game = new Game();

            var round = 1;

            while (!deck.IsEmpty)
            {
                game.RunAllSteps(deck);

                Console.WriteLine($"Content of the piles after round {round++}:");
                game.PrintPiles();
                Console.WriteLine();

            }

            if (game.IsGameWon())
            {
                Console.WriteLine("Hooray - you won!");
            }

            Console.ReadLine();
        }
    }
}