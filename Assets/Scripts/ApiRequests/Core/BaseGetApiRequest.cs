using UnityEngine.Networking;

public abstract class BaseGetApiRequest<T> : BaseApiRequest<T> where T : BaseApiResponse
{
    protected override UnityWebRequest CreateRequest() => UnityWebRequest.Get(CreateUrl());
}
