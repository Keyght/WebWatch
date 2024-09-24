using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Networking;

namespace WebWatch.ApiRequests
{
    public abstract class BaseApiRequest<T> : ApiRequest<T> where T : BaseApiResponse
    {
        public UnityEvent Sended;
        public UnityEvent<UnityWebRequest.Result, string> Errored;
        public UnityEvent<T> Succeeded;
        public UnityEvent<string> SucceededRawData;
        public UnityEvent Progressing;

        [ContextMenu("Send")]
        public virtual void SendRequest()
        {
            Sended.Invoke();
            SendRequest(CreateRequest());
        }
        protected abstract string CreateUrl();
        protected abstract UnityWebRequest CreateRequest();

        protected override void OnProgress()
        {
            Progressing.Invoke();
        }

        protected override void OnError(UnityWebRequest.Result result, string error) => Errored.Invoke(result, error);

        protected override void OnSuccess(T response) => Succeeded.Invoke(response);

        protected override void OnSuccessRawData(string responseJson) => SucceededRawData.Invoke(responseJson);

    }
}