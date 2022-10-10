using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private StreetGenerator _street;
    
    public static GameController instance { get; private set; }


    public static event Action onGameStart;
    public static event Action onGameStop;
    public static event Action onGameRestart;
    public static event Action<int> onCoinsChanged;
    
    private void Awake()
    {
        instance = this;
    }
    
    public void StartGame()
    {
        onGameStart?.Invoke();
    }

    public void StopGame()
    {
        onGameStop?.Invoke();
        ScreenManager.CloseScreen();
        ScreenManager.OpenScreen(ScreensType.LOSE_SCREEN);
    }

    public void RestartGame()
    {
        onGameRestart?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PickUpCoin(GameObject coin)
    {
        var currentCoin = 0;
        for (var i = 0; i < _street._coins.Count; i++)
        {
            if (_street._coins[i] == coin) currentCoin = i;
        }

        _street._coins.RemoveAt(currentCoin);
        Destroy(coin);
        onCoinsChanged?.Invoke(PlayerController.coinsCount);
    }
}
