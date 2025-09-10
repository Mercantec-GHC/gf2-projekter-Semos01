namespace Hjemmet
{
    public class GuessANumber
    {
        public void Start()
        {
            Console.WriteLine("Gæt et tal er ikke implementeret endnu.");

            Random tilfældigtal = new Random();
            int nummeratgætte = tilfældigtal.Next(1, 101);
            Console.WriteLine();
            Console.WriteLine(nummeratgætte);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
