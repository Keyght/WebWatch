using UnityEngine.Networking;

namespace WebWatch.ApiRequests
{
    public abstract class BaseGetApiRequest<T> : BaseApiRequest<T> where T : BaseApiResponse
    {
        protected override UnityWebRequest CreateRequest() => UnityWebRequest.Get(CreateUrl());
    }
}