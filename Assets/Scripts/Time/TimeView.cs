using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace WebWatch.Time
{
    public class TimeView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform _hourArrow;
        [SerializeField] private Transform _minuteArrow;
        [SerializeField] private Transform _secondArrow;
        [SerializeField] private TextMeshProUGUI _timeText;
        [Header("Parameters")]
        [SerializeField] private float _duration = 0.1f;
        [SerializeField] private string _format = "HH:mm:ss";

        private const int _clockScaleInterval = 360 / 60;
        private const int _clockScaleIntervalHours = 360 / 12;

        public void OnTimeChanged(DateTime dateTime)
        {
            ChangeTimeView(dateTime);
        }

        private void ChangeTimeView(DateTime dateTime)
        {
            RotateArrow(_hourArrow, new Vector3((dateTime.Hour % 12 + dateTime.Minute / 60f + dateTime.Second / 3600f) * _clockScaleIntervalHours, 0, 0));
            RotateArrow(_minuteArrow, new Vector3((dateTime.Minute + dateTime.Second / 60f) * _clockScaleInterval, 0, 0));
            RotateArrow(_secondArrow, new Vector3(dateTime.Second * _clockScaleInterval, 0, 0));

            _timeText.text = dateTime.ToString(_format);
        }

        private void RotateArrow(Transform arrow, Vector3 rotationVector)
        {
            arrow.DOLocalRotate(rotationVector, _duration);
        }
    }
}