using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dr.Jones_TraderPlus_Converter
{
    internal class Conversion
    {
        static void Main(string[] args)
        {
            // Setze die Größe des Konsolenfensters
            Console.SetWindowSize(155, 50); // Hier kannst du die gewünschte Breite und Höhe einstellen

            Console.WriteLine("##########################################################################################################################################################");
            Console.WriteLine("");
            Console.WriteLine("                                                                                 [..           [... [......                        [..        ");
            Console.WriteLine("                                               [.. [..   [.. [..                  [.                [..                            [..        ");
            Console.WriteLine("             [..   [..  [....     [... [. [...    [..       [..    [... [.. [..       [....         [..        [..        [..      [..  [.... ");
            Console.WriteLine("               [. [..  [..      [..     [..     [..       [..       [..  [.  [..     [..            [..      [..  [..   [..  [..   [.. [..    ");
            Console.WriteLine("                [.       [...  [..      [..        [..       [..    [..  [.  [..       [...         [..     [..    [.. [..    [..  [..   [... ");
            Console.WriteLine("              [.  [..      [..  [..     [..          [..       [..  [..  [.  [..         [..        [..      [..  [..   [..  [..   [..     [..");
            Console.WriteLine("             [..   [.. [.. [..    [... [...    [.....    [.....    [...  [.  [..     [.. [..        [..        [..        [..     [... [.. [..");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("[.....                   [..                                          [... [......                      [..                 [.......   [..               ");
            Console.WriteLine("[..   [..                [..                                               [..                          [..                 [..    [.. [..               ");
            Console.WriteLine("[..    [..[. [...        [..   [..    [.. [..     [..     [....            [..    [. [...   [..         [..   [..    [. [...[..    [.. [..[..  [.. [.... ");
            Console.WriteLine("[..    [.. [..           [.. [..  [..  [..  [.. [.   [.. [..    [.....     [..     [..    [..  [..  [.. [.. [.   [..  [..   [.......   [..[..  [..[..    ");
            Console.WriteLine("[..    [.. [..           [..[..    [.. [..  [..[..... [..  [...            [..     [..   [..   [.. [.   [..[..... [.. [..   [..        [..[..  [..  [... ");
            Console.WriteLine("[..   [..  [..      [.   [.. [..  [..  [..  [..[.            [..           [..     [..   [..   [.. [.   [..[.         [..   [..        [..[..  [..    [..");
            Console.WriteLine("[.....    [...   [.. [....     [..    [...  [..  [....   [.. [..           [..    [...     [.. [... [.. [..  [....   [...   [..       [...  [..[..[.. [..");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                                 [..                                                          [..                     ");
            Console.WriteLine("                              [..   [..                                                       [..                     ");
            Console.WriteLine("                             [..           [..     [.. [..   [..     [..    [..     [. [... [.[. [.    [..     [. [...");
            Console.WriteLine("                             [..         [..  [..   [..  [..  [..   [..   [.   [..   [..      [..    [.   [..   [..   ");
            Console.WriteLine("                             [..        [..    [..  [..  [..   [.. [..   [..... [..  [..      [..   [..... [..  [..   ");
            Console.WriteLine("                             [..   [..  [..  [..    [..  [..    [.[..    [.          [..      [..   [.          [..   ");
            Console.WriteLine("                               [....      [..      [...  [..     [..       [....    [...       [..    [....    [...   ");
            Console.WriteLine("");
            Console.WriteLine("##########################################################################################################################################################");
            Console.WriteLine("");
            Task.Delay(500).Wait();
            Console.WriteLine("[INFO] <Initializing....>");
            Console.WriteLine("");

            if (args.Length == 0)
            {
                Console.WriteLine("[INFO] <No input file specified. Please drag and drop a file onto the executable to convert it.>");
                Console.WriteLine("\n[INFO] <Press any key to exit the program.>");
                Console.ReadKey();
                return;
            }

            Task.Delay(750).Wait();
            Console.WriteLine("[INFO] <Loading File....>");
            Console.WriteLine("");
            Task.Delay(750).Wait();
            Console.WriteLine("[INFO] <Start Converting....>\n\n");

            Task.Delay(750).Wait();

            try
            {
                // Pfade zur Eingabe-Textdatei und Ausgabe-JSON-Datei
                string inputFilePath = args[0];
                string outputFilePath = Path.ChangeExtension(inputFilePath, ".json");

                // Lese die gesamte Textdatei ein
                string[] lines = File.ReadAllLines(inputFilePath);

                // Initialisiere die Trader-Kategorien-Liste
                List<TraderCategory> traderCategories = new List<TraderCategory>();

                // Initialisiere die aktuelle Trader-Kategorie
                TraderCategory currentCategory = null;

                // Initialisiere Zähler für Produkte und Kategorien
                int productCount = 0;
                int categoryCount = 0;

                // Initialisiere eine Liste für fehlerhafte Zeilen
                List<string> errorLines = new List<string>();

                // Durchlaufe alle Zeilen der Textdatei
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // Entferne führende und abschließende Leerzeichen und Tabs
                    string trimmedLine = line.Trim();

                    // Ignoriere Kommentarzeilen und Zeilen mit CurrencyName und Trader
                    if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("<CurrencyName>") || trimmedLine.StartsWith("<Trader>"))
                    {
                        Console.WriteLine($"[INFO] <Ignored line {i + 1}: {trimmedLine}>");
                        continue;

                    }

                    // Ignoriere Leerzeilen
                    if (string.IsNullOrWhiteSpace(trimmedLine))
                    {
                        Console.WriteLine($"[INFO] <Ignored empty line {i + 1}>");
                        continue;
                    }

                    // Entferne Kommentare am Ende der Zeile
                    int commentIndex = trimmedLine.IndexOf("//");
                    if (commentIndex >= 0)
                        trimmedLine = trimmedLine.Substring(0, commentIndex).Trim();

                    Console.WriteLine($"[INFO] <Converting line {i + 1}: {trimmedLine}>");

                    if (trimmedLine.StartsWith("<Category>"))
                    {
                        // Neue Kategorie gefunden
                        string categoryName = trimmedLine.Replace("<Category>", "").Trim();
                        currentCategory = new TraderCategory { CategoryName = categoryName };
                        traderCategories.Add(currentCategory);
                        categoryCount++;
                    }
                    else if (currentCategory != null && !trimmedLine.StartsWith("<FileEnd>"))
                    {
                        // Produkt gefunden
                        string[] productInfo = trimmedLine.Split(',').Select(info => info.Trim()).ToArray();
                        if (productInfo.Length >= 4)
                        {
                            string productName = productInfo[0];
                            string ek = productInfo[2];
                            string vk = productInfo[3];

                            string productData = string.Join(",", productName, "1", "-1", "1", ek, vk);
                            currentCategory.Products.Add(productData);
                            productCount++;
                        }
                        else
                        {
                            errorLines.Add($"[ERROR] <Error in line {i + 1}: Invalid product format.>\n");
                        }
                    }
                }

                // Erstelle das JSON-Objekt
                var jsonObject = new
                {
                    Version = "2.5",
                    EnableAutoCalculation = 0,
                    EnableAutoDestockAtRestart = 0,
                    EnableDefaultTraderStock = 0,
                    TraderCategories = traderCategories
                };

                // Serialisiere das JSON-Objekt in eine formatierte JSON-Zeichenkette
                string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                // Schreibe das JSON in die Ausgabe-Datei
                File.WriteAllText(outputFilePath, json);


                Console.WriteLine("\n##########################################################################################################################################################");
                Console.WriteLine("");
                Console.WriteLine("                                         [........ [.. [...     [.. [..   [.. ..   [..     [.. [........ [.....    ");
                Console.WriteLine("                                         [..       [.. [. [..   [.. [.. [..    [.. [..     [.. [..       [..   [.. ");
                Console.WriteLine("                                         [..       [.. [.. [..  [.. [..  [..       [..     [.. [..       [..    [..");
                Console.WriteLine("                                         [......   [.. [..  [.. [.. [..    [..     [...... [.. [......   [..    [..");
                Console.WriteLine("                                         [..       [.. [..   [. [.. [..       [..  [..     [.. [..       [..    [..");
                Console.WriteLine("                                         [..       [.. [..    [. .. [.. [..    [.. [..     [.. [..       [..   [.. ");
                Console.WriteLine("                                         [..       [.. [..      [.. [..   [.. ..   [..     [.. [........ [.....    ");
                Console.WriteLine("");

                Console.WriteLine("[INFO] <Conversion completed.>");
                Console.WriteLine($"[INFO] <Total categories converted: {categoryCount}>");
                Console.WriteLine($"[INFO] <Total products converted: {productCount}>\n");

                if (errorLines.Count > 0)
                {
                    Console.WriteLine("##########################################################################################################################################################");
                    Console.WriteLine("");
                    Console.WriteLine("                                         [........ [.......     [.......         [....      [.......       [.. ..  ");
                    Console.WriteLine("                                         [..       [..    [..   [..    [..     [..    [..   [..    [..   [..    [..");
                    Console.WriteLine("                                         [..       [..    [..   [..    [..   [..        [.. [..    [..    [..      ");
                    Console.WriteLine("                                         [......   [. [..       [. [..       [..        [.. [. [..          [..    ");
                    Console.WriteLine("                                         [..       [..  [..     [..  [..     [..        [.. [..  [..           [.. ");
                    Console.WriteLine("                                         [..       [..    [..   [..    [..     [..     [..  [..    [..   [..    [..");
                    Console.WriteLine("                                         [........ [..      [.. [..      [..     [....      [..      [..   [.. ..  ");
                    Console.WriteLine("");

                    Console.WriteLine("[ERROR] <The following lines were not converted due to invalid values:>");
                    Console.WriteLine("[ERROR] <Check the origin file for the faulty lines to fix and try again.>");
                    foreach (var errorLine in errorLines)
                    {
                        Console.WriteLine(errorLine);
                    }
                }
                Console.WriteLine("##########################################################################################################################################################");
                Console.WriteLine("");
                Console.WriteLine("                                         [........                            [.. [..                          [..     ");
                Console.WriteLine("                                         [..                                  [.. [..                          [..     ");
                Console.WriteLine("                                         [..          [..        [..          [.. [..          [..        [... [..  [..");
                Console.WriteLine("                                         [......    [.   [..   [.   [..   [.. [.. [.. [..    [..  [..   [..    [.. [.. ");
                Console.WriteLine("                                         [..       [..... [.. [..... [.. [.   [.. [..   [.. [..   [..  [..     [.[..   ");
                Console.WriteLine("                                         [..       [.         [.         [.   [.. [..   [.. [..   [..   [..    [.. [.. ");
                Console.WriteLine("                                         [..         [....      [....     [.. [.. [.. [..     [.. [...    [... [..  [..");
                Console.WriteLine("");

                Console.WriteLine("[INFO] <Thank you for using my tools! If you have Feedback for me, i would be happy to get in contact with you!>");
                Console.WriteLine("[INFO] <Join the Discord: https://discord.com/invite/PasvscT4Nh>");
                Console.WriteLine("");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] <Error while converting the file: {ex.Message}>\n");
            }

            Console.WriteLine("[INFO] <Press any key to exit the program.>");
            Console.ReadKey();
        }
    }
    internal class TraderCategory
    {
        public string CategoryName { get; set; }
        public List<string> Products { get; set; }

        public TraderCategory()
        {
            Products = new List<string>();
        }
    }
}

