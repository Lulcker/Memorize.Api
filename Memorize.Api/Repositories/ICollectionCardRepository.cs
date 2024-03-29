using Memorize.Api.Models.Entities;
using Memorize.Library.Request;
using Memorize.Library.Response;

namespace Memorize.Api.Repositories
{
    public interface ICollectionCardRepository
    {
        Task<(bool, string)> AddCollectionCard(CollectionCard request);

        Task<(bool, string)> UpdateCollectionCard(UpdateCollectionCardRequest request);

        Task<(bool, string)> DeleteCollectionCard(CollectionCard request);

        Task<List<CollectionCardResponse>> AllCollectionCardsByUser(string userName);
    }
}
