namespace Memorize.Library.Request
{
    public class CardRequest
    {
        public int Id { get; set; }

        public int CollectionCardId { get; set; }

        public string FrontSide { get; set; } = string.Empty;

        public string BackSide { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
