using DG.Tweening;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace WebWatch.Time
{
    public class TimeView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform _hourArrow;
        [SerializeField] private Transform _minuteArrow;
        [SerializeField] private Transform _secondArrow;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TMP_InputField _timeInput;
        [Header("Parameters")]
        [SerializeField] private float _duration = 0.1f;
        [SerializeField] private string _format = "HH:mm:ss";

        public UnityEvent<DateTime> ArrowsDataRequested;

        private const int _clockScaleInterval = 360 / 60;
        private const int _clockScaleIntervalHours = 360 / 12;

        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 50);
        }

        public void SetTimeFromInputField(bool flag)
        {
            if (flag)
                ChangeTimeView(DateTime.ParseExact(_timeInput.text, _format, System.Globalization.CultureInfo.InvariantCulture));
        }

        public void RequestArrowsData()
        {
            var hourRot = _hourArrow.localEulerAngles;
            var minuteRot = _minuteArrow.localEulerAngles;
            var secondRot = _secondArrow.localEulerAngles;

            RotationToEditor(hourRot, out var hourX);
            RotationToEditor(minuteRot, out var minuteX);
            RotationToEditor(secondRot, out var secondX);

            var second = (int)(secondX > 0 ? secondX : 360 + secondX) / _clockScaleInterval;
            var minute = (int)(minuteX > 0 ? minuteX : 360 + minuteX) / _clockScaleInterval;
            var hour = (int)(hourX > 0 ? hourX : 360 + hourX) / _clockScaleIntervalHours;

            _timeInput.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}:{second.ToString("D2")}";
        }

        private void RotationToEditor(Vector3 hourRot, out Single angle)
        {
            if (hourRot.x > 0 && hourRot.x < 180)
            {
                if (hourRot.y == 180)
                    angle = 180 - hourRot.x;
                else
                    angle = hourRot.x;
            }
            else
            {
                if (hourRot.y == 180)
                    angle = -(hourRot.x - 180);
                else
                    angle = -(360 - hourRot.x);
            }
        }

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