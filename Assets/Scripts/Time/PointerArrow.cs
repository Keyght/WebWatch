using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerArrow : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1f;

    void OnMouseDrag()
    {
        var mousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Single angle;
        if (transform.localRotation.eulerAngles.x < 180)
        {
            angle = Vector2.Angle(Vector2.right, mousePosition);
            if (mousePosition.y > 0)
                angle = -angle;
        }
        else 
        {
            angle = Vector2.Angle(Vector2.left, mousePosition);
            if (mousePosition.y < 0)
                angle = -angle;
        }

        var tempEulerAngles = new Vector3(angle * _rotationSpeed * Time.deltaTime, 0, 0);

        transform.Rotate(tempEulerAngles);

        //transform.Rotate(new Vector3(transform.rotation.eulerAngles.x + cross, 0, 0));
        //transform.DOLocalRotate(new Vector3(transform.rotation.eulerAngles.x + cross, 0, 0), 0.1f);
    }
}
