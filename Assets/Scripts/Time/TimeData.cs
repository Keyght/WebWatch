using System;
using UnityEngine;
using UnityEngine.Events;

namespace WebWatch.Time
{
    public class TimeData : MonoBehaviour
    {
        private DateTime _currentTime;
        
        public UnityEvent<DateTime> TimeChanged;

        public DateTime CurrentTime => _currentTime;
        public bool IsActive { get; set; } = true;

        public void SetCurrentTimeFromUTC(DateTime utcTime)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
            
        }

        public void SetCurrentTime(DateTime newTime)
        {
            SetTime(newTime);
        }

        private void SetTime(DateTime newTime)
        {
            if (!_currentTime.Equals(newTime))
            {
                _currentTime = newTime;
                if (IsActive) 
                    TimeChanged.Invoke(newTime);
            }
        }
    }
}