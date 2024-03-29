namespace Memorize.Api.Models.Entities
{
    public class Card
    {
        public int Id { get; set; }

        public CollectionCard CollectionCard { get; set; }

        public int CollectionCardId { get; set; }

        public string FrontSide { get; set; } = string.Empty;

        public string BackSide { get; set; } = string.Empty;
    }
}
