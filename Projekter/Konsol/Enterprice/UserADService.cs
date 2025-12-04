using System.DirectoryServices.Protocols;

namespace Enterprice
{
    public class UserADservice
    {
        public static List<ADUser> GetAllUsers()
        {
            // Opret en tom liste til at gemme alle AD brugere
            var users = new List<ADUser>();
            Console.WriteLine("Starter hentning af AD brugere...");

            // Opret forbindelse til Active Directory
            using (var connection = ADService.Connect())
            {
                Console.WriteLine("Forbindelse oprettet. Forbereder søgeforespørgsel efter brugere.");
                // Definer søgningen:
                // - Hvor skal vi søge: i "NordicS.local" domænet
                // - Hvad søger vi efter: alle objekter af typen "user"
                // - Hvilke informationer vil vi have: 
                // - navn (cn) og beskrivelse
                var searchRequest = new SearchRequest(
                    "DC=NordicS,DC=local", // Søg i dette domæne
                    "(objectClass=user)", // Find alle brugere
                    SearchScope.Subtree, // Søg i hele domænet
                    "cn", // Brugerens navn
                    "description", // Brugerens beskrivelse
                    "mail", // Brugerns Mail
                    "samAccountName", // Brugerns kontonavn
                    "department", // Brugerens afdeling
                    "title" // Brugerens title
                );

                try
                {
                    // Udfør søgningen
                    var response = (SearchResponse)connection.SendRequest(searchRequest);
                    Console.WriteLine($"Søgningen returnerede {response.Entries.Count} grupper.");

                    // For hver gruppe vi finder
                    foreach (SearchResultEntry user in response.Entries)
                    {
                        // Opret et nyt ADUser objekt med informationerne
                        var nyUser = new ADUser
                        {
                            // Hvis værdien ikke findes, brug "N/A" som standard
                            Name = user.Attributes["cn"]?[0]?.ToString() ?? "N/A",
                            Username = user.Attributes["samAccountName"]?[0]?.ToString() ?? "N/A",
                            Email = user.Attributes["mail"]?[0]?.ToString() ?? "N/A",
                            Department = user.Attributes["department"]?[0]?.ToString() ?? "N/A",
                            Title = user.Attributes["title"]?[0]?.ToString() ?? "N/A",
                            Description = user.Attributes["description"]?[0]?.ToString() ?? "N/A"
                        };

                        // Tilføj brugeren til vores liste
                        users.Add(nyUser);
                        Console.WriteLine($"Tilføjet bruger: {nyUser.Name} - {nyUser.Username}, {nyUser.Email}, " +
                            $"{nyUser.Department}, {nyUser.Title}, {nyUser.Description}");
                    }

                    Console.WriteLine("Alle brugere er nu hentet og tilføjet til listen.");
                }
                catch (Exception ex)
                {
                    // Hvis noget går galt, fortæl hvad der skete
                    Console.WriteLine("Der opstod en fejl under hentning af grupper.");
                    throw new Exception($@"Der skete en fejl ved hentning af grupper:
      {ex.Message}");
                }
            }

            // Send alle de fundne brugere tilbage
            Console.WriteLine($"Returnerer {users.Count} brugere til kaldende kode.");
            return users;
        }

        // Tjekker om brugeren findes i systemet
        public static bool UserExists(string username)
        {
            using var connection = ADService.Connect();

            var searchRequest = new SearchRequest(
                "DC=NordicS,DC=local",
                $"(samAccountName={username})",
                SearchScope.Subtree,
                "samAccountName"
            );

            var response = (SearchResponse)connection.SendRequest(searchRequest);

            return response.Entries.Count > 0;
        }
    }

    public class ADUser
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
    }
}