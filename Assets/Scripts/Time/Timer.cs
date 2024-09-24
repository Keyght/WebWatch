using System;
using UnityEngine;

namespace WebWatch.Time
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] TimeData _timeData;

        private void Awake()
        {
            _timeData.SetCurrentTime(DateTime.Now);
        }

        private void Update()
        {
            if (_timeData is not null)
            {
                _timeData.SetCurrentTime(_timeData.CurrentTime.AddSeconds(UnityEngine.Time.deltaTime));
            }
        }
    }
}