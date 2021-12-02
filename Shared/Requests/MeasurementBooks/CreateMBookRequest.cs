using System.Collections.Generic;

namespace Shared.Requests
{
    public class CreateMBookRequest : MBookRequest
    {
        public List<MBookItemRequest> Items { get; set; } = new List<MBookItemRequest>();
    }
}
