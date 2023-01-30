using System.ComponentModel;

namespace DeckOfCardsLab.Models
{
    public class CardHandApi
    {
        [DisplayName("Deck ID")]
        public string deck_id { get; set; }
        public List<CardsApi> cards { get; set; }
        [DisplayName("Cards Reamining in Deck")]
        public int remaining { get; set; }
    }
}
