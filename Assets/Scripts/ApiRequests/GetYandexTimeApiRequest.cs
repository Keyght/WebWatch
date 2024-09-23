using UnityEngine;

[System.Serializable]
public class GetTimeResponse : BaseApiResponse
{
    public int time;
}

public class GetYandexTimeApiRequest : BaseGetApiRequest<GetTimeResponse>
{
    [SerializeField] private string _url = "https://yandex.com/time/sync.json";
    [SerializeField] private string _geo = "msk";

    protected override string CreateUrl()
    {
        return _url + "?geo=" + _geo;
    }
}
