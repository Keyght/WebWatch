using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Networking;

public abstract class BaseApiRequest<T> : ApiRequest<T> where T : BaseApiResponse
{
    public UnityEvent Send;
    public UnityEvent<UnityWebRequest.Result, string> Error;
    public UnityEvent<T> Success;
    public UnityEvent<string> SuccessRawData;
    public UnityEvent Progress;

    [ContextMenu("Send")]
    public virtual void SendRequest()
    {
        Send.Invoke();
        SendRequest(CreateRequest());
    }
    protected abstract string CreateUrl();
    protected abstract UnityWebRequest CreateRequest();

    protected override void InProgress()
    {
        Progress.Invoke();
    }

    protected override void ErrorCallback(UnityWebRequest.Result result, string error) => Error.Invoke(result, error);

    protected override void SuccessCallback(T response) => Success.Invoke(response);

    protected override void SuccessRawDataCallback(string responseJson) => SuccessRawData.Invoke(responseJson);

}
