using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action onLeftSwipe;
    public event Action onRightSwipe;
    public event Action onUpSwipe;
    public event Action onDownSwipe;

    private Vector3 _startPoint;
    private Vector3 _finishPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _startPoint = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _finishPoint = Input.mousePosition;
            if (_startPoint == _finishPoint) return;
            
            var direction = Vector3.Normalize(_finishPoint - _startPoint);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0.0f) onRightSwipe?.Invoke();
                else onLeftSwipe?.Invoke();
            }
            else if (direction.y > 0.0f) onUpSwipe?.Invoke();
            else onDownSwipe?.Invoke();
        }
    }
}