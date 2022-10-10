using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] private Transform _moveTransform;
    [SerializeField] private Transform _spinTransform;
    private float _speed = 0.5f;
    
    private void Update()
    {
        _spinTransform.Rotate(0.0f, 0.0f, 200.0f * Time.deltaTime);
        _moveTransform.position += new Vector3(_speed, 0.0f, 0.0f) * Time.deltaTime;
        
        if (_moveTransform.position.x > 1.0f) _speed = -0.5f;
        if (_moveTransform.position.x < -1.0f) _speed = 0.5f;
        
    }
}