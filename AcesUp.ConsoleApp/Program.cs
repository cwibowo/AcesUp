using AcesUp.Common;

namespace AcesUp.ConsoleApp
{
    internal static class Program
    {
        static void Main()
        {
            // Setting output encoding so the suit of the cards can be displayed in the console.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int win = 0;
            int match = 10000;
            int degreeOfParallelism = -1;
            Parallel.For(0, match, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism}, (_, _) =>
            {
                var r = new Random();

                var deck = Deck.CreateShuffledDeck(r);

                var game = new Game();

                var round = 1;

                while (!deck.IsEmpty)
                {
                    game.RunAllSteps(deck);

                    // Only print when running the match(es) in series.
                    if (degreeOfParallelism == 1)
                    {
                        Console.WriteLine($"Content of the piles after round {round++}:");
                        game.PrintPiles();
                        Console.WriteLine();
                    }
                }

                if (game.IsGameWon())
                {
                    win = Interlocked.Increment(ref win);

                    if (degreeOfParallelism == 1)
                    {
                        Console.WriteLine("Hooray - you won!");
                    }
                }
            });

            Console.WriteLine($"Match: {match} - WIN/LOSE: {win}/{match-win}");
            Console.ReadLine();
        }
    }
}