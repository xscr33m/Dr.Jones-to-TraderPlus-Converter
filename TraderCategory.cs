using System.Collections.Generic;

namespace Dr.Jones_TraderPlus_Converter
{
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
