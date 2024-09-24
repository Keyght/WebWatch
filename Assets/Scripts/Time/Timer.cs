using System;
using UnityEngine;
using WebWatch.Time;

public class Timer : MonoBehaviour
{
    [SerializeField] TimeData _timeData;

    public bool IsActive { get; set; } = true;

    private void Awake()
    {
        _timeData.SetCurrentTime(DateTime.Now);
    }

    private void Update()
    {
        if (_timeData is not null)
        {
            if (IsActive)
            {
                _timeData.SetCurrentTime(_timeData.CurrentTime.AddSeconds(Time.deltaTime));
            }
        }
    }
}
