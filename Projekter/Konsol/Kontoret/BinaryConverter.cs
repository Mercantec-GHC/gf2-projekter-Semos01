using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kontoret
{
    public class BinaryConverter
    { public void ConverteripAdr()
        {
            Console.WriteLine();
            Console.WriteLine("Skriv en IPv4-adresse (fx 192.43.32.5):");
            string input = Console.ReadLine();

            string[] parts = input.Split('.' , ','); // deler IP op i 4 dele

            while (true)
            {
                if (parts.Length != 4)
                {                    
                    Console.WriteLine("Ugyldig IP-adresse");
                    input = Console.ReadLine();
                    parts = input.Split("." , ',');
                }                
                if (parts.Length == 4)                
                {
                    break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("IP i binær:");

            // loop igennem de 4 dele
            for (int i = 0; i < parts.Length; i++)
            {
                int number = int.Parse(parts[i]); // gør teksten til et tal
                string binary = Converter(number); // brug din gamle metode
                Console.WriteLine();
                Console.WriteLine($"{number} -> {binary}");                
            }
            Console.ReadKey();
        }
    public string Converter(int input)
    {   
        string binary = "";
 
        int[] values = { 128, 64, 32, 16, 8, 4, 2, 1 };
 
        foreach (int value in values)
        {
            if (input >= value)
            {
                binary += "1";
                input -= value;
            }
            else
            {
                binary += "0";
            }
        }
            return binary;

        
    }

        public void Start()
        {
            Console.WriteLine(@"Skriv jeres første tal fra jeres IP-adresse ind
                                - altså et tal fra 0-255");
            string input = Console.ReadLine();
            int number = int.Parse(input);

            string binary = ""; // 
            // Tjekker om først bit skal være tændt eller slukket!
            if (number >= 128)
            {
                binary += "1";
                number -= 128;
            }
            else
            {
                binary += "0";
            }
            if (number >= 64)
            {
                binary += "1";
                number -= 64;
            }
            else
            {
                binary += "0";
            }
            if (number >= 32)
            {
                binary += "1";
                number -= 32;
            }
            else
            {
                binary += "0";
            }
            if (number >= 16)
            {
                binary += "1";
                number -= 16;
            }
            else
            {
                binary += "0";
            }
            if (number >= 8)
            {
                binary += "1";
                number -= 8;
            }
            else
            {
                binary += "0";
            }
            if (number >= 4)
            {
                binary += "1";
                number -= 4;
            }
            else
            {
                binary += "0";
            }
            if (number >= 2)
            {
                binary += "1";
                number -= 2;
            }
            else
            {
                binary += "0";
            }
            if (number >= 1)
            {
                binary += "1";
                number -= 1;
            }
            else
            {
                binary += "0";
            }
            Console.WriteLine($"Dit {input} svare til {binary} i binær!");
            Console.ReadKey();
        }
    }
}