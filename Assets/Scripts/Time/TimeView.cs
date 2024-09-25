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
            var hourRot = TransformUtils.GetInspectorRotation(_hourArrow);
            var minuteRot = TransformUtils.GetInspectorRotation(_minuteArrow);
            var secondRot = TransformUtils.GetInspectorRotation(_secondArrow);

            var second = (int)  (secondRot.x > 0 ? secondRot.x : 360 + secondRot.x) / _clockScaleInterval;
            var minute = (int) (minuteRot.x > 0 ? minuteRot.x : 360 + minuteRot.x) / _clockScaleInterval;
            var hour = (int) (hourRot.x > 0 ? hourRot.x : 360 + hourRot.x) / _clockScaleIntervalHours;

            _timeInput.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}:{second.ToString("D2")}";
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