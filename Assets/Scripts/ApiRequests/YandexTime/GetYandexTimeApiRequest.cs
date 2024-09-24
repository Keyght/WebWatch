using UnityEngine;

namespace WebWatch.ApiRequests
{
    [System.Serializable]
    public class GetTimeResponse : BaseApiResponse
    {
        public long time;
    }

    public class GetYandexTimeApiRequest : BaseGetApiRequest<GetTimeResponse>
    {
        [SerializeField] private string _url = "https://yandex.com/time/sync.json";
        [SerializeField] private string _geo = "utc";

        protected override string CreateUrl()
        {
            var additionalUrl = string.IsNullOrEmpty(_geo) ? string.Empty : "?geo=" + _geo;
            return _url + additionalUrl;
        }
    }
}