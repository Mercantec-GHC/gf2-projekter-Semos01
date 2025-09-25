namespace Hjemmet
{
    public class GuessANumber
    {
        public void Start()
        {
            Console.WriteLine("Velkommen til Gæt et Tal spillet.");

            Random tilfældigtal = new Random();
            int nummeratgætte = tilfældigtal.Next(1, 10001);
            int gæt = 0;
            int antalgæt = 0;            
            
            while (gæt != nummeratgætte)
            {
                Console.WriteLine();
                Console.WriteLine("Indtast dit gæt (1-10000): ");
                Console.WriteLine();
                string input = Console.ReadLine();

                antalgæt++;
                if (int.TryParse(input, out gæt))
                {
                    if (gæt < nummeratgætte)
                    {
                        Console.WriteLine();
                        Console.WriteLine("For lavt! Prøv igen.");
                        Console.WriteLine();
                    }
                    else if (gæt > nummeratgætte)
                    {
                        Console.WriteLine();
                        Console.WriteLine("For højt! Prøv igen.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Tillykke! Du gættede rigtigt på {antalgæt} forsøg.");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Ugyldigt input. Indtast venligst et tal mellem 1 og 100.");
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Tak for spillet!");
            Console.WriteLine("Tryk på en tast for at komme tilbage til hovedemenuen.");
            Console.ReadKey();
        }
    }
}
