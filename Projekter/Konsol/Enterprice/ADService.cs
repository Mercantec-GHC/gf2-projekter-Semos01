using System.DirectoryServices.Protocols;
using System.Net;

namespace Enterprice
{
    public class ADService // Infomation programmet skal bruge til at logge ind med
    {
        private static string _server = "192.168.1.26"; // Addresse
        private static string _username = "Administrator"; // Brugernavn
        private static string _password = "Gruppe8!"; // Password
        private static string _domain = "NordicS.local"; // Domæne

        public static LdapConnection Connect()
        {
            try // Prøver at køre koden under
            {
                // Opretter credentials til AD baseret på brugernavn, domæne og password
                Console.WriteLine();
                Console.WriteLine("Defining Credentials");
                var credential = new NetworkCredential($"{_username}@{_domain}", _password);
                Console.WriteLine($"Credentials defined: {credential.UserName}");

                // Opretter selve LDAP-forbindelsen til den angivne AD-server
                Console.WriteLine("Creating Connection");
                Console.WriteLine($"Server: {_server}");
                var connection = new LdapConnection(_server)
                {
                    // Angiver hvilke credentials forbindelsen skal bruge
                    Credential = credential,

                    // Bruger Windows' forhandling af autentifikation (Kerberos/NTLM), Negotiate er default auth type for LDAP
                    AuthType = AuthType.Negotiate
                };
                // Forsøger at autentificere og skabe forbindelse til AD
                Console.WriteLine("Binding to AD");

                // Tester forbindelsen
                connection.Bind();

                // Returnerer en writeline hvis vi har en gyldig og aktiv LDAP-forbindelse
                Console.WriteLine();
                Console.WriteLine("Connection to AD established!");
                return connection;
            }
            catch (Exception ex) // Fanger hvis der sker en fejl, og kommer med en fejl meddelse
            {
                // Returnerer en writeline hvis vi ikke har en gyldig og aktiv LDAP-forbindelse
                Console.WriteLine();
                Console.WriteLine($"Error connecting to AD: {ex.Message}");
                throw;
            }
        }

        public void Start()
        {
            try
            {
                using var connection = Connect();
            }
            catch
            {
                Console.ReadKey(); // Får programmet til at vente med at køre videre til vi har trykket på en taste
                return;
            }

            // Viser menuen
            ShowMenu();
            // Skifter menuen
            switch (Console.ReadLine())
            {
                // Tillader os at køre programmet for at hente alle brugere i AD
                case "1":
                    var userADService = new UserADservice();
                    UserADservice.GetAllUsers();
                    Console.ReadKey(); 
                    break;

                // Tillader os at køre programmet for at hente alle Grupper i AD.
                case "2":
                    GroupADservice.GetAllGroups();
                    Console.ReadKey();
                    break;

                // Tillader os at køre programmet for at stempel ind og ud i systemet.
                case "3":
                    Console.WriteLine();
                    Console.Write("Indtast brugernavn: ");
                    var user = Console.ReadLine();
                    new TimeStampService().AutoStamp(user);
                    Console.ReadKey();
                    break;

                // Tillader os at køre programmet for at se vores stempel ind og ud historik i systemet
                case "4":
                    Console.WriteLine();

                    while (true)
                    {
                        Console.WriteLine();
                        Console.Write("Indtast brugernavn (eller 'exit' for at afbryde): ");
                        var userHist = Console.ReadLine();

                        if (userHist?.ToLower() == "exit")
                            break;

                        // Check om brugeren findes før vi henter historik
                        if (!UserADservice.UserExists(userHist))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"-- Brugeren '{userHist}' findes ikke i AD.");
                            continue;
                        }

                        var records = new TimeStampService().GetHistory(userHist);

                        if (records.Count == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("-- Ingen historik fundet for denne bruger.");
                        }
                        else
                        {
                            foreach (var r in records)
                                Console.WriteLine(r);
                        }

                        Console.ReadKey();
                        break;
                    }
                    break;

            }
        }
        public void ShowMenu()
        {
            // Giver text i menuen hvor vi skal vælge
            Console.WriteLine();
            Console.WriteLine("1. Get all users");
            Console.WriteLine("2. Get all groups");
            Console.WriteLine("3. Check in/out");
            Console.WriteLine("4. See history");
            Console.WriteLine("5. Exit");
        }
    }
}
