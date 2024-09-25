using DG.Tweening;
using UnityEngine;

namespace WebWatch.Time
{
    public class MouseDragArrow : MonoBehaviour
    {
        [SerializeField] private TimeView _timeView;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _duration = 0.1f;

        public void OnMouseDrag()
        {
            transform.DOLocalRotate(new Vector3(_rotationSpeed, 0f, 0f), _duration, RotateMode.LocalAxisAdd);

            _timeView.RequestArrowsData();
        }
    }
}