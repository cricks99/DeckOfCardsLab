using DeckOfCardsLab.Models;
using Flurl.Http;

namespace DeckOfCardsLab.DAL
{
    public class DeckOfCardsRepository
    {
        CardHandApi cardHandApi;

        public DeckOfCardsRepository()
        {
            cardHandApi = new CardHandApi();
        }

        public DeckOfCardsRepository(CardHandApi cardHand)
        {
            cardHandApi = new CardHandApi();
            cardHandApi = cardHand;
        }

        public CardHandApi GetNewDeck(string deckId)
        {
            return DrawCardsFromAPI(deckId, 5);
        }

        public CardHandApi GetCurrentHand()
        {
            return cardHandApi;
        }

        public void SaveCurrentHand(CardHandApi cards)
        {
            cardHandApi = cards;
        }

        public CardHandApi DrawCards()
        {
            int numCardsNeeded = Math.Min(cardHandApi.remaining, 5 - cardHandApi.cards.Count());
            string deckId = cardHandApi.deck_id;
            CardHandApi newCards = new CardHandApi();

            newCards = DrawCardsFromAPI(deckId, numCardsNeeded);

            foreach(CardsApi card in newCards.cards)
                cardHandApi.cards.Add(card);
            cardHandApi.remaining = newCards.remaining;

            return cardHandApi;
        }

        private CardHandApi DrawCardsFromAPI(string deckId, int numCards)
        {
            string apiUri = $"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count={numCards}";
            var apiTask = apiUri.GetJsonAsync<CardHandApi>();
            apiTask.Wait();
            
            return apiTask.Result;
        }

        public void ReshuffleCards()
        {
            string deckId = cardHandApi.deck_id;
            string apiUri = $"https://deckofcardsapi.com/api/deck/{deckId}/shuffle/";
            var apiTask = apiUri.GetJsonAsync<CardHandApi>();
            apiTask.Wait();

            cardHandApi = new CardHandApi();
            cardHandApi = GetNewDeck(deckId);
        }

        public void RemoveCard(string code)
        {
            int index = cardHandApi.cards.FindIndex(x => x.code == code);
            cardHandApi.cards.RemoveAt(index);
        }
    }
}
