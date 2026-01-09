using System;
using System.Collections.Generic;

namespace ProjektGF2
{
    public class Player // Spillere navne og deres total score
    {
        public string Name { get; set; }
        public int TotalScore { get; set; }
    }

    public class Flip7
    {
        private Random random = new Random(); // Ruller et tilfældigt tal 
        private List<int> deck; // Vores virtuelle kort

        public void Start()
        {
            while (true) // Vores start menu for at starte spillet eller forlade det
            {
                Console.Clear();
                Console.WriteLine("=== FLIP 7 - Først til 200 points vinder! ===");
                Console.WriteLine("[1] Start spil");
                Console.WriteLine("[2] Exit");
                Console.Write("Valg: ");

                string choice = Console.ReadLine();

                if (choice == "2") // Vælger 2 går man tilbage til den forgående menu
                    return;

                if (choice == "1") // Vælger man 1 starter spillet
                    StartGame();
            }
        }

        private void StartGame()
        {
            Console.Clear();
            int playerCount = GetPlayerCount(); // Henter hvor mange spillere vi har valgt

            List<Player> players = new List<Player>(); // Listen med spillere og deres score

            for (int i = 1; i <= playerCount; i++) // For hver spiller man har, kører man denne kode
            {
                Console.Write($"Indtast navn for spiller {i}: ");
                players.Add(new Player { Name = Console.ReadLine(), TotalScore = 0 }); // Tilføjer navn og score til listen af spillere
            }

            BuildDeck();

            bool gameRunning = true;

            while (gameRunning) // Bliver ved med at kører, så længe spillet er i gang
            {
                foreach (Player player in players) // For hver spiller vi har valgt
                {
                    Console.Clear(); // Rydder konsollen for alt forgånende tekst
                    Console.WriteLine($"--- {player.Name}'s tur ---");
                    Console.WriteLine($"Total score: {player.TotalScore}");

                    bool exited = PlayTurn(player);

                    if (exited) // Hvis en spiller trykker E forlader vi spillet
                        return;

                    if (player.TotalScore >= 200) // Kontrollere om en spillers score er over 200, kårer en vinner hvis der er
                    {
                        Console.WriteLine($"--- {player.Name} vinder med {player.TotalScore} point! ---");
                        Console.WriteLine("Tryk på en tast for at vende tilbage til menuen...");
                        Console.ReadKey();
                        gameRunning = false;
                        break;
                    }
                }
            }
        }

        private bool PlayTurn(Player player) // Koden der kører under en spillers tur
        {
            HashSet<int> flippedNumbers = new HashSet<int>();
            int turnScore = 0; // Den samlede score under turen
            bool secondLife = false; // Kontrollere om man har fået Second Life
            bool doublePoints = false; // Kontrollere om man har fået Double Points
            bool turnActive = true; // Kontrollere om turen er aktiv

            while (turnActive)
            {
                Console.WriteLine("\n[F]lip  [S]top  [E]xit");
                Console.Write("Valg: ");
                string choice = Console.ReadLine()?.ToUpper() ?? "";

                if (choice == "E") // Hvis spilleren trykker E forlader vi spillet
                    return true;

                if (choice == "S") // Hvis spilleren trykker S stopper vores tur og tilføjer vores points til total score
                {
                    BankPoints(player, turnScore, doublePoints);
                    break;
                }

                if (choice != "F") // Flipper et "kort", fejlmelder hvis input er forkert og forsætter turen
                {
                    Console.WriteLine("Ugyldigt valg.");
                    continue;
                }

                int specialRoll = random.Next(1, 101); // Ruller et tilfældig tal mellem 1-100

                if (specialRoll <= 5) // Hvis det er under 5 stopper ens tur, men man beholder den score man har fået under turen
                {
                    Console.WriteLine("--- FREEZE! Turen stopper. ---");
                    Console.ReadKey();
                    BankPoints(player, turnScore, doublePoints);
                    break;
                }
                else if (specialRoll <= 10) // Ruller det under 10 får man Second Life
                {
                    secondLife = true;
                    Console.WriteLine("--- SECOND LIFE opnået! ---");
                }
                else if (specialRoll <= 15) // Ruller det under 15 får man x2 Bonus
                {
                    doublePoints = true;
                    Console.WriteLine("--- x2 BONUS aktiveret! ---");
                }
                else if (specialRoll <= 25) // Ruller det under 25 får man +5 Points til sin score under turen
                {
                    turnScore += 5;
                    Console.WriteLine("--- +5 POINTS! ---");
                }
                else if (specialRoll <= 30) // Ruller det under 30 får man +10 Points til sin score under turen
                {
                    turnScore += 10;
                    Console.WriteLine("--- +10 POINTS! ---");
                }

                int card = DrawCard(); // Trækker et kort fra vores Deck
                Console.WriteLine($"Du vendte: {card}");

                if (flippedNumbers.Contains(card)) // Stopper turen hvis vi trækker det samme kort vi allerede har
                {
                    if (secondLife) // Forsætter turen hvis vi har Second Life og fjerne det fra spilleren
                    {
                        Console.WriteLine("--- BUST! Second life brugt! ---");
                        secondLife = false;
                        continue;
                    }

                    Console.WriteLine("--- BUST! Turen giver 0 point. ---");
                    Console.WriteLine($"Tryk på en tast for at forsætte.");
                    Console.ReadKey(); // Venter på input fra spilleren
                    break;
                }

                flippedNumbers.Add(card); // Tilføjer kortet til flipped numbers
                turnScore += card; // Tilføjer nummer til vores tur score

                Console.WriteLine($"Tur score: {turnScore}.");
                
            }

            return false;
        }

        private void BankPoints(Player player, int turnScore, bool doublePoints) // Ligger vores turscore til total score
        {
            if (doublePoints) // Hvis vi har fået Double Points ganger den vores tur score med 2
                turnScore *= 2;

            player.TotalScore += turnScore;
            Console.WriteLine($"--- Tur slut. Du fik {turnScore} point. ---");
            Console.WriteLine($"Tryk på en tast for at forsætte.");
            Console.ReadKey();
        }      

        private void BuildDeck() // Bygger vores deck af kort som vi kan trække fra under spillet.
         
        {
            deck = new List<int>();

            for (int number = 0; number <= 12; number++)
            {
                int count = number == 0 ? 1 : number;
                for (int i = 0; i < count; i++)
                    deck.Add(number);
            }
        }

        private int DrawCard() // Trækker et kort fra decket og fjerne det
        {
            if (deck.Count == 0) // Hvis decket rammer nul kort, laver den et ny deck
            {
                BuildDeck();
                Console.WriteLine("--- Kortbunken blev blandet igen. ---");
            }

            int index = random.Next(deck.Count);
            int card = deck[index];
            deck.RemoveAt(index);

            return card;
        }

        private int GetPlayerCount() // Koden for hvor mange spillere vi er
        {
            int count;
            do
            {
                Console.Write("Antal spillere (1–5): ");
            }
            while (!int.TryParse(Console.ReadLine(), out count) || count < 1 || count > 5); // Vi kan minimum være 1 og max 5 spillere

            return count;
        }
    }
}
