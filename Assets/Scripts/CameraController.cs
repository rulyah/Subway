using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private const float _playerDistance = 2;
    
   
    private void Update()
    {
        var cameraPos = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z - _playerDistance);
        transform.position = Vector3.Lerp(transform.position, cameraPos, 30.0f * Time.deltaTime);
    }
}