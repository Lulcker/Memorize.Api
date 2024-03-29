using Memorize.Api.Models;
using Memorize.Api.Models.Entities;
using Memorize.Library.Request;
using Memorize.Library.Response;
using Microsoft.EntityFrameworkCore;

namespace Memorize.Api.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _db;

        public CardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(bool, string)> AddCard(CardRequest request)
        {
            if (request == null || request.CollectionCardId == 0 || request.FrontSide == null || request.BackSide == null )
                return (false, "Ошибка заполнения");

            var collectionCard = await _db.CollectionCards.FirstOrDefaultAsync(c => c.Id == request.CollectionCardId && c.UserName == request.UserName);
            if (collectionCard != null)
            {
                var newCard = new Card
                {
                    CollectionCardId = request.CollectionCardId,
                    FrontSide = request.FrontSide,
                    BackSide = request.BackSide,
                };

                await _db.Cards.AddAsync(newCard);
                await _db.SaveChangesAsync();

                return (true, "Карточка добавлена");
            }

            return (false, "Коллекция не найдена");
        }

        public async Task<(bool, string)> UpdateCard(CardRequest request)
        {
            if (request == null || request.CollectionCardId == 0 || request.FrontSide == null || request.BackSide == null)
                return (false, "Ошибка заполнения");

            var card = await _db.Cards.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (card != null)
            {
                card.FrontSide = request.FrontSide;
                card.BackSide = request.BackSide;

                _db.Cards.Update(card);
                await _db.SaveChangesAsync();

                return (true, "Карточка обновлена");
            }

            return (false, "Карточка не найдена");
        }

        public async Task<(bool, string)> DeleteCard(int cardId)
        {
            if (cardId == 0) 
                return (false, "Ошибка заполнения");

            var card = await _db.Cards.FirstOrDefaultAsync(x => x.Id == cardId);

            if (card != null)
            {
                _db.Cards.Remove(card); 
                await _db.SaveChangesAsync();

                return (true, "Карточка удалена");
            }

            return (false, "Карточка не найдена");
        }

        public async Task<List<CardResponse>> AllCardsByCollection(int collectionCardId)
        {
            var listCards = await _db.Cards.Where(c => c.CollectionCardId == collectionCardId).ToListAsync();

            List<CardResponse> cards = new List<CardResponse>();
            foreach (var card in listCards)
            {
                cards.Add(new CardResponse { Id = card.Id, CollectionCardId = card.CollectionCardId, FrontSide = card.FrontSide, BackSide = card.BackSide });
            }

            return cards;
        }
    }
}
