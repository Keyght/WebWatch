using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace WebWatch.ApiRequests
{
    public abstract class ApiRequest<T> : MonoBehaviour where T : BaseApiResponse
    {
        [SerializeField] private bool _isDebug;
        [SerializeField] private bool _canBeReplacedByNewRequest;

        protected IEnumerator _currCoroutine;

        protected virtual void SendRequest(UnityWebRequest webRequest)
        {
            if (_currCoroutine != null && !_canBeReplacedByNewRequest)
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
            _canBeReplacedByNewRequest = isCanBeReplacedByNew;
        }

        public void StopRequest()
        {
            if (_currCoroutine != null)
                StopCoroutine(_currCoroutine);

            _currCoroutine = null;
        }


        protected abstract void OnError(UnityWebRequest.Result result, string error);
        protected abstract void OnSuccess(T response);

        protected virtual void OnSuccessRawData(string responseJson)
        {

        }

        protected virtual void OnProgress()
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
                        OnError(webRequest.result, webRequest.error);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(webRequest.url + ": DataProcessingError: " + webRequest.error);
                        OnError(webRequest.result, webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(webRequest.url + ": ProtocolError: " + webRequest.error);
                        Debug.LogError(webRequest.downloadHandler.text);
                        OnError(webRequest.result, webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:

                        var jsontext = webRequest.downloadHandler.text;
                        var response = JsonUtility.FromJson<T>(jsontext);

                        if (_isDebug)
                            Debug.Log($"Request downloadHandler.text:{jsontext}");

                        OnSuccessRawData(webRequest.downloadHandler.text);
                        OnSuccess(response);

                        break;
                    case UnityWebRequest.Result.InProgress:
                        Debug.Log(webRequest.url + ": in progress ");
                        OnProgress();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _currCoroutine = null;
        }
    }
}