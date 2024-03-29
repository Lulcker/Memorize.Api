using Memorize.Api.Models.Entities;
using Memorize.Library.Request;
using Memorize.Library.Response;

namespace Memorize.Api.Repositories
{
    public interface ICardRepository
    {
        Task<(bool, string)> AddCard(CardRequest request);

        Task<(bool, string)> UpdateCard(CardRequest request);

        Task<(bool, string)> DeleteCard(int cardId);

        Task<List<CardResponse>> AllCardsByCollection(int collectionCardId);
    }
}
