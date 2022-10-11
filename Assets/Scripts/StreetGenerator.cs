using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StreetGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _streetPrefab;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _cameraTransform;

    private readonly List<GameObject> _street = new List<GameObject>();
    public List<GameObject> _coins = new List<GameObject>();
    private const float _streetMaxCount = 3.0f;
    private const float _streetLength = 29.5f;


    private void CreatePies()
    {
        var pos = new Vector3(0.0f, 0.0f, _streetLength/2);
        if (_street.Count > 0)
        {
            pos = _street[^1].transform.position + new Vector3(0, 0, _streetLength);
        }
        var street = Instantiate(_streetPrefab, pos, Quaternion.identity);
        street.transform.SetParent(transform);
        _street.Add(street);
        CreateCoins(street,_coinPrefab);
    }

    private void CreateCoins(GameObject streetPart, GameObject coinPrefab)
    {
        var startPoint = streetPart.transform.position.z - _streetLength / 2.0f;
        var finishPoint = streetPart.transform.position.z + _streetLength / 2.0f;
        var offset = 1.0f;
        var previousPos = new Vector3(0.0f, 0.75f, startPoint + offset);
        var previousLine = (int)previousPos.x;
        
        while (true)
        {
            var coinPos = new Vector3(GetRandomLine(previousLine), previousPos.y, previousPos.z + offset);
            
            if(coinPos.z >= finishPoint) break;
            previousPos = coinPos;

            if(CheckPosition(coinPos) == false) continue;

            var coin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
            _coins.Add(coin);
            previousLine = (int)coin.transform.position.x;
        }
    }

    private bool CheckPosition(Vector3 pos)
    {
        var colliders = Physics.OverlapSphere(pos, 0.5f);
        return colliders.All(t => !t.gameObject.CompareTag("Obstacle"));
    }
    private int GetRandomLine(int line)
    {
        return line switch
        {
            -1 => Random.Range(-1, 1),
            1 => Random.Range(0, 2),
            _ => Random.Range(-1, 2)
        };
    }
    private void ResetGame()
    {
        while (_street.Count > 0)
        {
            Destroy(_street[0]);
            _street.Remove(_street[0]);
        }
        for (var i = 0; i < _streetMaxCount; i++)
        {
            CreatePies();
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void MoveStreetPies()
    {
        var pies = _street[0];
        pies.transform.position = _street[^1].transform.position + new Vector3(0.0f, 0.0f, _streetLength);
        _street.Remove(_street[0]);
        _street.Add(pies);
        CreateCoins(pies, _coinPrefab);
    }

    private void DeleteCoin(GameObject streetPart)
    {
        var finishPoint = streetPart.transform.position.z + _streetLength / 2.0f;
        _coins.RemoveAll(n => n == null);
        for (var i = 0; i < _coins.Count; i++)
        {
            if (_coins[i].transform.position.z < finishPoint)
            {
                _coins.RemoveAt(i);
                Destroy(_coins[i]);
                i--;
            }
        }
    }
    
    private void Update()
    {
        if (!(_street[0].transform.position.z < _cameraTransform.position.z - 15)) return;
        DeleteCoin(_street[0]);
        MoveStreetPies();
    }
}
