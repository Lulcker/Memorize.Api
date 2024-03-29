using Memorize.Api.Models;
using Memorize.Api.Models.Entities;
using Memorize.Library.Request;
using Memorize.Library.Response;
using Microsoft.EntityFrameworkCore;

namespace Memorize.Api.Repositories
{
    public class CollectionCardRepository : ICollectionCardRepository
    {
        private readonly ApplicationDbContext _db;

        public CollectionCardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(bool, string)> AddCollectionCard(CollectionCard request)
        {
            if (request == null || request.Name == string.Empty)
                return (false, "Ошибка заполнения");

            var collectionCard = await _db.CollectionCards.FirstOrDefaultAsync(c => c.Name == request.Name && c.UserName == request.UserName);

            if (collectionCard == null)
            {
                var newCollectionCard = new CollectionCard
                {
                    Name = request.Name,
                    UserName = request.UserName
                };

                await _db.CollectionCards.AddAsync(newCollectionCard);
                await _db.SaveChangesAsync();

                return (true, $"Коллекция {request.Name} добавлена");
            }

            return (false, $"Коллекция с названием {request.Name} уже существует");
        }

        public async Task<(bool, string)> UpdateCollectionCard(UpdateCollectionCardRequest request)
        {
            if (request == null || request.CurrentName == string.Empty || request.NewName == string.Empty)
                return (false, "Ошибка заполнения");

            var collectionCard = await _db.CollectionCards.FirstOrDefaultAsync(c => c.Name == request.CurrentName && c.UserName == request.UserName);

            if (collectionCard != null)
            {
                collectionCard.Name = request.NewName;

                _db.CollectionCards.Update(collectionCard);
                await _db.SaveChangesAsync();

                return (true, $"Коллекция переименована");
            }

            return (false, $"Коллекции с названием {request.CurrentName} не существует");
        }

        public async Task<(bool, string)> DeleteCollectionCard(CollectionCard request)
        {
            if (request == null || request.Id == 0)
                return (false, "Ошибка заполнения");

            var collectionCard = await _db.CollectionCards.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserName == request.UserName);

            if (collectionCard != null)
            {
                var listCards = await _db.Cards.Where(c => c.CollectionCardId == collectionCard.Id).ToListAsync();

                if (listCards.Any())
                {
                    _db.Cards.RemoveRange(listCards);
                }

                _db.CollectionCards.Remove(collectionCard);
                await _db.SaveChangesAsync();

                return (true,  $"Коллекция {collectionCard.Name} удалена");
            }

            return (false, $"Коллекции с названием {request.Name} не существует");
        }

        public async Task<List<CollectionCardResponse>> AllCollectionCardsByUser(string userName)
        {
            if (userName == null)
                return null!;
            
            var listCollectionCards = await _db.CollectionCards.Where(c => c.UserName == userName).ToListAsync();

            var response = new List<CollectionCardResponse>();
            foreach (var card in listCollectionCards)
            {
                response.Add(new CollectionCardResponse { Id = card.Id , Name = card.Name });
            }

            return response;
        }
    }
}
