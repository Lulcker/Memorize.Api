using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorize.Library.Response
{
    public class CardResponse
    {
        public int Id { get; set; }

        public int CollectionCardId { get; set; }

        public string FrontSide { get; set; } = string.Empty;

        public string BackSide { get; set; } = string.Empty;
    }
}
