using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ApiRequest<T> : MonoBehaviour where T : BaseApiResponse
{
    [SerializeField] private bool _isDebug;

    protected IEnumerator _currCoroutine;

    private bool _canBeReplacedByNew;

    protected virtual void SendRequest(UnityWebRequest webRequest)
    {
        if (_currCoroutine != null && !_canBeReplacedByNew)
        {
            Debug.LogWarning(webRequest.url + " request already running! New request not started:" + webRequest.url + "\nWait while existing one will be done!");
            return;
        }


        if (_currCoroutine != null)
            StopRequest();

        _currCoroutine = RequestRoutine(webRequest);
        StartCoroutine(_currCoroutine);
    }

    public void SetCanBeReplacedByNew(bool isCanBeReplacedByNew)
    {
       _canBeReplacedByNew = isCanBeReplacedByNew;
    }

    public void StopRequest()
    {
        if (_currCoroutine != null)
            StopCoroutine(_currCoroutine);

        _currCoroutine = null;
    }


    protected abstract void ErrorCallback(UnityWebRequest.Result result, string error);
    protected abstract void SuccessCallback(T response);

    protected virtual void SuccessRawDataCallback(string responseJson)
    {

    }

    protected virtual void InProgress()
    {

    }

    protected virtual void OnDisable()
    {
        StopRequest();
    }

    protected virtual IEnumerator RequestRoutine(UnityWebRequest webRequest)
    {
        Debug.Log($"SendWebRequest {webRequest.url} {webRequest.method}");
        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:

                    Debug.LogError(webRequest.url + ": ConnectionError: " + webRequest.error);
                    ErrorCallback(webRequest.result, webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.url + ": DataProcessingError: " + webRequest.error);
                    ErrorCallback(webRequest.result, webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(webRequest.url + ": ProtocolError: " + webRequest.error);
                    Debug.LogError(webRequest.downloadHandler.text);
                    ErrorCallback(webRequest.result, webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:

                    var jsontext = webRequest.downloadHandler.text;
                    var response = JsonUtility.FromJson<T>(jsontext);

                    if (_isDebug)
                        Debug.Log($"Request downloadHandler.text:{jsontext}");

                    SuccessRawDataCallback(webRequest.downloadHandler.text);
                    SuccessCallback(response);

                    break;
                case UnityWebRequest.Result.InProgress:
                    Debug.Log(webRequest.url + ": in progress ");
                    InProgress();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        _currCoroutine = null;
    }
}
