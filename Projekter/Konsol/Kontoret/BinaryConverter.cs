using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kontoret
{
    public class BinaryConverter
    {
        // Metode til at konvertere en IPv4-adresse fra decimalformat til binær format
        public void ConverteripAdr()
        {
            Console.WriteLine();
            Console.WriteLine("Skriv en IPv4-adresse (fx 192.43.32.5):");
            string input = Console.ReadLine();

            // Splitter input på '.' eller ',' for at få de 4 dele af IP-adressen
            string[] parts = input.Split('.', ',');

            while (true)
            {
                // Tjekker om input består af præcis 4 dele
                if (parts.Length != 4)
                {
                    Console.WriteLine("Ugyldig IP-adresse");
                    input = Console.ReadLine();
                    parts = input.Split('.', ',');
                }
                if (parts.Length == 4)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("IP i binær:");

            // Loop igennem hver af de 4 oktetter
            for (int i = 0; i < parts.Length; i++)
            {
                int number = int.Parse(parts[i]); // Konverter tekst til tal
                string binary = Converter(number); // Konverter tallet til binær string
                Console.WriteLine();
                Console.WriteLine($"{number} -> {binary}");
            }
            Console.ReadKey();
        }

        // Metode til at konvertere en binær bit oktet (fx 10100010) til decimal
        public void ConverterBit()
        {
            Console.WriteLine();
            Console.WriteLine("Skriv en Bit oktet (fx 10100010.11000100):");
            string input = Console.ReadLine();

            // Splitter input på '.' eller ',' for at få de enkelte 8-bit dele
            string[] parts = input.Split('.', ',');

            while (true)
            {
                // Tjekker at der er mellem 1 og 4 dele
                if (parts.Length < 1 || parts.Length > 4)
                {
                    Console.WriteLine("Ugyldig Bit-adresse. Du skal skrive 1 til 4 dele adskilt af '.' eller ','.");
                    input = Console.ReadLine();
                    parts = input.Split('.', ',');
                    continue; // Spring resten over og prøv igen
                }

                bool valid = true;

                // Tjekker hver del for at være præcis 8 bits og kun bestå af 0 og 1
                foreach (string part in parts)
                {
                    if (part.Length != 8 || !part.All(c => c == '0' || c == '1'))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    Console.WriteLine("Ugyldigt input. Hver del skal være præcis 8 bits og kun bestå af 0 og 1.");
                    input = Console.ReadLine();
                    parts = input.Split('.', ',');
                    continue; // prøv igen
                }

                // Hvis input er gyldigt, forlades while loopet
                break;
            }

            // Konverterer hver binære del til decimal via ReverseConverter-metoden
            int[] decimalParts = parts.Select(part => ReverseConverter(part)).ToArray();

            // Udskriver resultatet som en IP-lignende decimal adresse
            Console.WriteLine();
            Console.WriteLine("Decimal format:");
            Console.WriteLine(string.Join(".", decimalParts));
            Console.ReadKey();
        }

        // Metode til at konvertere et heltal (0-255) til en 8-bit binær string
        public string Converter(int input)
        {
            string binary = "";

            // Værdier for hver bit position fra MSB til LSB
            int[] values = { 128, 64, 32, 16, 8, 4, 2, 1 };

            // For hver værdi: tjek om input er større eller lig med værdien, sæt 1 eller 0
            foreach (int value in values)
            {
                if (input >= value)
                {
                    binary += "1";
                    input -= value; // træk værdien fra input for at fortsætte
                }
                else
                {
                    binary += "0";
                }
            }
            return binary;
        }

        // Metode til at konvertere en 8-bit binær string til et decimaltal
        public int ReverseConverter(string input)
        {
            int ipaddress = 0;
            int[] values = { 128, 64, 32, 16, 8, 4, 2, 1 };

            if (input.Length != 8)
                throw new ArgumentException("Input skal være 8 tegn langt.");

            // For hver karakter i stringen: tjek om det er '1' og læg den tilsvarende værdi til resultatet
            for (int i = 0; i < 8; i++)
            {
                if (input[i] == '1')
                {
                    ipaddress += values[i];
                }
                else if (input[i] != '0')
                {
                    throw new ArgumentException("Input må kun indeholde 0 eller 1.");
                }
            }

            return ipaddress;
        }

        // En alternativ metode (uden loop) til at konvertere et tal til binær string, skrevet "manuelt"
        public void Start()
        {
            Console.WriteLine(@"Skriv jeres første tal fra jeres IP-adresse ind
                                - altså et tal fra 0-255");
            string input = Console.ReadLine();
            int number = int.Parse(input);

            string binary = "";

            // Tjek hver bit manuelt fra MSB til LSB og byg binær string
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

            // Udskriv resultatet
            Console.WriteLine($"Dit {input} svare til {binary} i binær!");
            Console.ReadKey();
        }
    }
}
