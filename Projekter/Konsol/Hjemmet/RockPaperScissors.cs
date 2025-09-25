namespace Hjemmet
{
    public class RockPaperScissors
    {
        private enum Move { Sten = 1, Saks = 2, Papir = 3 }

        private readonly Dictionary<Move, Move> _winMap = new()
            {
                { Move.Sten, Move.Saks },
                { Move.Saks, Move.Papir },
                { Move.Papir, Move.Sten }
            };

        private readonly Dictionary<Move, int> _playerHistory = new()
            {
                { Move.Sten, 0 },
                { Move.Saks, 0 },
                { Move.Papir, 0 }
            };

        public void Start()
        {
            Console.WriteLine("Velkommen til Sten, Saks, Papir mod AI!");
            while (true)
            {
                Console.WriteLine("\nVælg dit træk: (1) Sten, (2) Saks, (3) Papir, (0) Afslut");
                Console.Write("Dit valg: ");
                string? input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Farvel!");
                    break;
                }

                if (!int.TryParse(input, out int playerChoice) || playerChoice < 1 || playerChoice > 3)
                {
                    Console.WriteLine("Ugyldigt valg. Prøv igen.");
                    continue;
                }

                Move playerMove = (Move)playerChoice;
                _playerHistory[playerMove]++;

                Move aiMove = GetSmartAIMove();
                Console.WriteLine($"AI vælger: {aiMove}");

                if (playerMove == aiMove)
                {
                    Console.WriteLine("Uafgjort!");
                }
                else if (_winMap[playerMove] == aiMove)
                {
                    Console.WriteLine("Du vinder!");
                }
                else
                {
                    Console.WriteLine("AI vinder!");
                }
            }
        }

        private Move GetSmartAIMove()
        {
            // Find det mest valgte træk fra spilleren
            Move mostCommon = _playerHistory.OrderByDescending(x => x.Value).First().Key;
            // AI vælger det træk, der slår det mest almindelige spillertræk
            foreach (var pair in _winMap)
            {
                if (pair.Value == mostCommon)
                    return pair.Key;
            }
            // fallback
            return (Move)(new Random().Next(1, 4));
        }
    }
}
