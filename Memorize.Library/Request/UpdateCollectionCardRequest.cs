namespace Memorize.Library.Request
{
    public class UpdateCollectionCardRequest
    {
        public string CurrentName { get; set; } = string.Empty;

        public string NewName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}