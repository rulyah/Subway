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
    private bool isGameStart;

    private void Start()
    {
        GameController.onGameStart += GameStart;
        GameController.onGameStop += GameStop;
    }

    private void GameStart()
    {
        isGameStart = true;
        _startPoint = Vector3.zero;
        _finishPoint = Vector3.zero;
    }
    private void GameStop()
    {
        GameController.onGameStart -= GameStart;
        GameController.onGameStop -= GameStop;
        isGameStart = false;
    }
    

    private void Update()
    {
        if (isGameStart)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _startPoint = Input.mousePosition;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(_startPoint == Vector3.zero) return;
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
}