using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{

    [SerializeField] private Transform _cameraTransform;
    public GameObject[] obstacles;
    public List<GameObject> _usedObstacles = new List<GameObject>();
    private const int _maxObstacleCount = 15;


    private void Start()
    {
        ResetGame();
    }
    
    private void Create()
    {
        var currentObstacle = GetRandomObstacle();
        if (currentObstacle == obstacles[0])
        {
            currentObstacle = Instantiate(currentObstacle, GetRandomPos(), Quaternion.identity);
        }
        else
        {
            var pos = GetRandomPos();
            currentObstacle = Instantiate(currentObstacle);
            currentObstacle.transform.position = new Vector3(0.0f, 0.0f, pos.z);
            currentObstacle.transform.rotation = Quaternion.identity;
        }
        _usedObstacles.Add(currentObstacle);
    }

    private Vector3 GetRandomPos()
    {
        var posX = Random.Range(-1.0f, 1.0f) < 0.0f ? -1.0f : 1.0f;
        var posZ = 3.0f;
        if (_usedObstacles.Count > 0)
        {
            posZ = _usedObstacles[^1].transform.position.z;

        }
        return new Vector3(posX, 0.0f, Random.Range(posZ + 5, posZ + 10));
    }

    private GameObject GetRandomObstacle()
    {
        var currentObs = obstacles[Random.Range(0, obstacles.Length)];
        return currentObs;
    } 
    
    public void ResetGame()
    {
        while (_usedObstacles.Count > 0)
        {
            Destroy(_usedObstacles[0]);
            _usedObstacles.Remove(_usedObstacles[0]);
        }
        for (var i = 0; i < _maxObstacleCount; i++)
        {
            Create();
        }
    }
    private void Update()
    {
        if (!(_usedObstacles[0].transform.position.z < _cameraTransform.position.z - 10.0f)) return;
        Destroy(_usedObstacles[0]);
        _usedObstacles.Remove(_usedObstacles[0]);
        Create();
    }
}