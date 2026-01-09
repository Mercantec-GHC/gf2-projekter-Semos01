using System;
using System.Collections.Generic;

namespace ProjektGF2
{
    public class Player
    {
        public string Name { get; set; }
        public int TotalScore { get; set; }
    }

    public class Flip7
    {
        private Random random = new Random();

        public void Start()
        {
            while (true) // Start Menuen
            {
                Console.Clear(); // Rydder konsol vinduet for alt det forrige tekst
                Console.WriteLine("=== FLIP 7 - Først til 200 points vinder! ===");
                Console.WriteLine("[1] Start spil");
                Console.WriteLine("[2] Exit");
                Console.Write("Valg: ");

                string choice = Console.ReadLine(); // læser input brugeren taster

                if (choice == "2") // Går tilbage til den forrige menu
                    return;

                if (choice == "1") // starter spillet
                    StartGame();
            }
        }

        private void StartGame()
        {
            Console.Clear();
            int playerCount = GetPlayerCount(); // finder hvor mange spillere vi har valgt

            List<Player> players = new List<Player>(); // laver en liste af spiller

            for (int i = 1; i <= playerCount; i++)
            {
                Console.Write($"Indtast navn for spiller {i}: ");
                players.Add(new Player { Name = Console.ReadLine(), TotalScore = 0 });  // tilføjer navne og score til listen
            }

            bool gameRunning = true;

            while (gameRunning) // kører spillerens tur
            {
                foreach (Player player in players)
                {
                    Console.Clear();
                    Console.WriteLine($"--- {player.Name}'s tur ---"); // viser spillerens navn
                    Console.WriteLine($"Total score: {player.TotalScore}"); // viser spillerens total score

                    bool exited = PlayTurn(player);

                    if (exited)
                        return;

                    if (player.TotalScore >= 200) // tjekker om spilleres score er over 200
                    {
                        Console.WriteLine($"\n{player.Name} VINDER MED {player.TotalScore} POINT!"); // viser spilleres navn og score hvis over 200
                        Console.WriteLine("Tryk på en tast for at vende tilbage til menuen...");
                        Console.ReadKey(); // venter på input fra bruger før den tager den næste action
                        gameRunning = false;
                        break;
                    }
                }
            }
        }

        private bool PlayTurn(Player player)
        {
            HashSet<int> flippedNumbers = new HashSet<int>();
            int turnScore = 0;
            bool secondLife = false;
            bool doublePoints = false;
            bool turnActive = true;

            while (turnActive)
            {
                Console.WriteLine("\n[F]lip  [S]top  [E]xit"); // fortæller spilleren hvad valgmuligheder man har
                Console.Write("Valg: ");                
                string choice = Console.ReadLine()?.ToUpper() ?? "";

                if (choice == "E")
                    return true;

                if (choice == "S")
                {
                    BankPoints(player, turnScore, doublePoints); // ligger tur scoren og dobbelt, hvis man har, til spilleren total score
                    break;
                }

                if (choice != "F") // fejl tekst hvis du trykker andet end F, S eller E
                {
                    Console.WriteLine("Ugyldigt valg.");
                    continue;
                }

                int specialRoll = random.Next(1, 101); // ruller for chancen for at få et specialt kort

                if (specialRoll <= 5) // hvis den ruller under 5 fryser spillerens tur og ligger scoren til
                {
                    Console.WriteLine("FREEZE! Turen stopper.");                    
                    Console.WriteLine("Tryk på en tast for at fortsætte");
                    Console.ReadKey();
                    BankPoints(player, turnScore, doublePoints);
                    break;
                }
                else if (specialRoll <= 10) // hvis den ruller under 10 får man et second life på turen
                {
                    secondLife = true;
                    Console.WriteLine("SECOND LIFE opnået!");
                    Console.WriteLine();
                }
                else if (specialRoll <= 15) // hvis den ruller under 15 får man double bonus på turen
                {
                    doublePoints = true;
                    Console.WriteLine("x2 BONUS aktiveret!");
                    Console.WriteLine();
                }
                else if (specialRoll <= 25) // hvis den ruller under 25 får man 5 points til sin score på turen
                {
                    turnScore += 5;
                    Console.WriteLine("+5 POINTS!");
                    Console.WriteLine();
                }
                else if (specialRoll <= 30) // hvis den ruller under 30 får man 10 points til sin score på turen
                {
                    turnScore += 10;
                    Console.WriteLine("+10 POINTS!");
                    Console.WriteLine();
                }

                int card = DrawWeightedCard(); // trækker et tal fra DrawWeightedCard reglen
                Console.WriteLine($"Du vendte: {card}");

                if (flippedNumbers.Contains(card))
                {
                    if (secondLife) // tjekker om du har second life og giver dig en chance mere hvis du har
                    {
                        Console.WriteLine();
                        Console.WriteLine("BUST! Second life brugt!");
                        secondLife = false;
                        continue;
                    }

                    Console.WriteLine();
                    Console.WriteLine("BUST! Turen giver 0 point.");
                    Console.WriteLine("Tryk på en tast for at forsætte");
                    Console.ReadKey();
                    break;
                }

                flippedNumbers.Add(card); // tilføjer det trukket kort til scoren
                turnScore += card;

                Console.WriteLine($"Tur score: {turnScore}");
            }

            return false;
        }

        private void BankPoints(Player player, int turnScore, bool doublePoints) // dobbelt score reglen
        {
            if (doublePoints)
                turnScore *= 2;

            player.TotalScore += turnScore;
            Console.WriteLine($"Tur slut. Du fik {turnScore} point.");
        }

        private int DrawWeightedCard() // reglen for at trække kort
        {
            List<int> deck = new List<int>();

            for (int number = 0; number <= 12; number++)
            {
                int count = number == 0 ? 1 : number;
                for (int i = 0; i < count; i++)
                    deck.Add(number);
            }

            return deck[random.Next(deck.Count)];
        }

        private int GetPlayerCount() // spørger om hvor mange spillere vi vil have
        {
            int count;
            do
            {
                Console.Write("Antal spillere (1–5): ");
            }
            while (!int.TryParse(Console.ReadLine(), out count) || count < 1 || count > 5);

            return count;
        }
    }
}
