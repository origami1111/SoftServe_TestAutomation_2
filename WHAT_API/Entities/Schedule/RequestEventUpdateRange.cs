using Newtonsoft.Json;

namespace WHAT_API.Entity
{
    class RequestEventUpdateRange
    {
        [JsonProperty("filter")]
        public FilterUpdateRange Filter { get; set; }
        [JsonProperty("request")]
        public RequestUpdateRange Request { get; set; }

        public RequestEventUpdateRange WithFilter(FilterUpdateRange filter)
        {
            Filter = filter;
            return this;
        }

        public RequestEventUpdateRange WithRequest(RequestUpdateRange request)
        {
            Request = request;
            return this;
        }
    }
}
