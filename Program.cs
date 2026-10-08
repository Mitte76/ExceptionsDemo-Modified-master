namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Start av programmet ===");

            // Exempel 1: try-catch-finally
            try
            {
                Console.WriteLine("Försöker läsa fil och räkna...");
                //var result = ProcessFile(AppContext.BaseDirectory + @"fff\", "numbers.txt");
                var result = ProcessFile(AppContext.BaseDirectory, "1numbers.txt");

                Console.WriteLine($"\nResultat: {result}");
            }
            catch (FileNotFoundException ex)
            {
                // Specifikt fel om filen inte finns
                Console.WriteLine($"Filen hittades inte: {ex.Message}");
            }
            catch (FormatException ex)
            {
                // Specifikt fel om texten inte kan tolkas som tal
                Console.WriteLine($"Formatfel: {ex.Message}");
            }
            catch (DivideByZeroException ex)
            {
                // Specifikt fel om nolldivision
                Console.WriteLine($"Kan inte dividera med noll: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Fallback för alla övriga obekanta fel
                Console.WriteLine($"Övrigt fel: {ex.Message}");
            }
            finally
            {
                // Körs ALLTID, även om det blev undantag
                Console.WriteLine("Cleanup: Logging avslutat anrop.");
            }

            Console.WriteLine("Programmet avslutas normalt.");
        }

        // Exempel på metod som själv kastar ett undantag (throw)
        static double ProcessFile(string directory, string fileName)
        {
            // Om filnamnet är tomt vill vi signalera
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filnamn får inte vara tomt eller null.", nameof(fileName));
            }

            // Om Katalogsökvägen är tom vill vi signalera
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Katalogsökväg får inte vara tom eller null.", nameof(directory));
            }

            string path = Path.Combine(directory, fileName);

            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(path);

                string? line = reader.ReadLine();
                if (line == null)
                {
                    throw new InvalidOperationException("Filen är tom.");
                }
                // Försöker omvandla text till tal
                int number = int.Parse(line); // Kan ge FormatException

                // Division: kan ge DivideByZeroException
                if (number == 0)
                {
                    throw new DivideByZeroException("Hittade siffran noll i filen");
                }
                return 100.0 / number;
            }
            catch (FormatException ex)
            {
                // Vi kan logga eller omformulera felet
                Console.WriteLine($"Formatfel i ProcessFile: {ex.Message}");
                // Vi kan välja att låta metoden "kasta upp" felet
                throw; // När du i `catch` bara vill logga/analysera,
                       // men låta anroparen (t.ex. en högre nivå i applikationen)
                       // bestämma hur man ska återhämta sig. 
            }
            catch (DirectoryNotFoundException ex){
                throw new DirectoryNotFoundException("Katalogen finns inte: " + directory);
            }
            finally
            {
                // Garanterad stängning av resurs
                reader?.Close();
                Console.WriteLine("finally i ProcessFile: StreamReader stängd.");
            }
        }
    }
}