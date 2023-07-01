using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Dr.Jones_TraderPlus_Converter
{
    internal class Conversion
    {
        static void Main(string[] args)
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

            // Durchlaufe alle Zeilen der Textdatei
            foreach (string line in lines)
            {
                // Entferne führende und abschließende Leerzeichen und Tabs
                string trimmedLine = line.Trim();

                // Ignoriere Kommentarzeilen und Zeilen mit CurrencyName und Trader
                if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("<CurrencyName>") || trimmedLine.StartsWith("<Trader>"))
                    continue;

                // Ignoriere Leerzeilen
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                // Entferne Kommentare am Ende der Zeile
                int commentIndex = trimmedLine.IndexOf("//");
                if (commentIndex >= 0)
                    trimmedLine = trimmedLine.Substring(0, commentIndex).Trim();

                if (trimmedLine.StartsWith("<Category>"))
                {
                    // Neue Kategorie gefunden
                    string categoryName = trimmedLine.Replace("<Category>", "").Trim();
                    currentCategory = new TraderCategory { CategoryName = categoryName };
                    traderCategories.Add(currentCategory);
                }
                else if (currentCategory != null && !trimmedLine.StartsWith("<FileEnd>"))
                {
                    // Produkt gefunden
                    string[] productInfo = trimmedLine.Split(',').Select(info => info.Trim()).ToArray();
                    string productName = productInfo[0];
                    string ek = "1";
                    string vk = "-1";

                    if (productInfo.Length >= 3)
                    {
                        ek = productInfo[2];
                        vk = productInfo[3];
                    }

                    string productData = string.Join(",", productName, "1", "-1", "100", "-1", ek, vk);
                    currentCategory.Products.Add(productData);
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

            Console.WriteLine("Die Umwandlung wurde abgeschlossen. Die JSON-Datei wurde erstellt.");
        }
    }

    // Klasse, um Trader-Kategorien darzustellen
    public class TraderCategory
    {
        public string CategoryName { get; set; }
        public List<string> Products { get; set; }

        public TraderCategory()
        {
            Products = new List<string>();
        }
    }
}
