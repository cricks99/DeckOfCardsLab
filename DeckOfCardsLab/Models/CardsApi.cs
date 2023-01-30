using System.Text;

namespace DeckOfCardsLab.Models
{
    public class CardsApi
    {
        public string code { get; set; }
        public string image { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
        public bool Save { get; set; }
    }
}
