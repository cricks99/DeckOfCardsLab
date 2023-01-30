using DeckOfCardsLab.DAL;
using DeckOfCardsLab.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeckOfCardsLab.Controllers
{
    public class CardController : Controller
    {
        DeckOfCardsRepository repo;

        public CardController()
        {
            repo = new DeckOfCardsRepository();
        }
        
        // GET: Cards
        public ActionResult CardDeck()
        {
            if (!GetSessionCache())
            {
                repo.SaveCurrentHand(repo.GetNewDeck("new"));
                HttpContext.Session.SetString("DeckOfCards", JsonConvert.SerializeObject(repo.GetCurrentHand()));
            }
            
            return View(repo.GetCurrentHand());
        }

        // POST: Cards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CardDeck(IFormCollection collection)
        {
            try
            {
                if (GetSessionCache())
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[$"cards[{i}].Save"] == "false")
                            repo.RemoveCard(collection[$"cards[{i}].code"]);
                    }
                    
                repo.DrawCards();
                HttpContext.Session.SetString("DeckOfCards", JsonConvert.SerializeObject(repo.GetCurrentHand()));
                
                return RedirectToAction(nameof(CardDeck));
            }
            
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reshuffle(IFormCollection collection)
        {
            try
            {
                if (GetSessionCache())
                {
                    repo.ReshuffleCards();
                    HttpContext.Session.SetString("DeckOfCards", JsonConvert.SerializeObject(repo.GetCurrentHand()));
                }

                return RedirectToAction(nameof(CardDeck));
            }
            
            catch
            {
                return RedirectToAction(nameof(CardDeck));
            }
        }

        private bool GetSessionCache()
        {
            string deckOfCardsJson = HttpContext.Session.GetString("DeckOfCards");
            if (!string.IsNullOrEmpty(deckOfCardsJson))
            {
                repo.SaveCurrentHand(JsonConvert.DeserializeObject<CardHandApi>(deckOfCardsJson));
                return true;
            }

            return false;
        }
    }
}
