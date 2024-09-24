using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace WebWatch.ApiRequests
{
    public class YandexTime : MonoBehaviour
    {
        [SerializeField] private GetYandexTimeApiRequest _getYandexTimeApiRequest;
        [SerializeField] private float _updateCicleInMinutes = 60;

        public UnityEvent<DateTime> TimeChanged;

        private Coroutine _nextUpdateTickerRoutine;

        private void OnEnable()
        {
            _getYandexTimeApiRequest.Succeeded.AddListener(OnSuccessYandexRequest);
            _getYandexTimeApiRequest.Errored.AddListener(OnErrorYandexRequest);
        }

        private void OnDisable()
        {
            _getYandexTimeApiRequest.Succeeded.RemoveListener(OnSuccessYandexRequest);
            _getYandexTimeApiRequest.Errored.RemoveListener(OnErrorYandexRequest);
        }

        private void Start()
        {
            _getYandexTimeApiRequest.SendRequest();
        }

        private void ChangeTime(DateTime newValue)
        {
            TimeChanged.Invoke(newValue);
        }

        private void OnSuccessYandexRequest(GetTimeResponse response)
        {
            Debug.Log($"[{GetType().Name}] Success request with data {response.time}");
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(response.time);
            ChangeTime(dateTimeOffset.DateTime);
            if (_nextUpdateTickerRoutine is not null)
                StopCoroutine(_nextUpdateTickerRoutine);
            _nextUpdateTickerRoutine = StartCoroutine(RequestTickRoutine());
        }

        private void OnErrorYandexRequest(UnityWebRequest.Result result, string errorText)
        {
            Debug.Log($"[{GetType().Name}] Error request with result {result} and error {errorText}");
        }

        private IEnumerator RequestTickRoutine()
        {
            var seconds = _updateCicleInMinutes * 60;
            while (seconds > 0)
            {
                seconds -= UnityEngine.Time.deltaTime;
                yield return null;
            }
            _getYandexTimeApiRequest.SendRequest();
        }
    }
}