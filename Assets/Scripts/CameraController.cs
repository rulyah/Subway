using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Camera _camera;
    private const float _playerDistance = 4;
    
   
    private void Update()
    {
        var distance = Vector3.Distance(_player.transform.position, transform.position);
        if (!(distance > _playerDistance)) return;
        var speed = _player.transform.position.z - transform.position.z - _playerDistance;
        if (speed < 0) speed = 0;
        transform.position += new Vector3(0.0f,0.0f,speed) * Time.deltaTime;
    }
}