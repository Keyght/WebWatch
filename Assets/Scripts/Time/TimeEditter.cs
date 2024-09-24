using System;
using UnityEngine;
using UnityEngine.Events;

namespace WebWatch.Time
{
    public class TimeEditter : MonoBehaviour
    {
        [SerializeField] private TimeData _timeData;
        [SerializeField] private string _format = "HH:mm:ss";

        public UnityEvent<bool> InputCorrect;

        public DateTime NewTime { get; set; }

        public void CkeckCorrectInput(string time)
        {
            Debug.Log(time + " " + DateTime.TryParseExact(time, _format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var newTime));
            InputCorrect.Invoke(DateTime.TryParseExact(time, _format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out newTime));
            NewTime = newTime;
        }

        public void ApplyNewInput()
        {
            _timeData.SetCurrentTime(NewTime);
        }
    }
}