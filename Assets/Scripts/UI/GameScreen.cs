using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameScreen : Screen
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private TextMeshProUGUI _text;

        
        public override void Show()
        {
            GameController.onCoinsChanged += OnCoinsChanged;
            GameController.onGameRestart += ClearScreen;
            _menuButton.onClick.AddListener(OnButtonClick);
            base.Show();
            GameController.instance.StartGame();
        }

        private void ClearScreen()
        {
            ScreenManager.CloseScreen();
        }

        private void OnCoinsChanged(int coinsCount)
        {
            _text.text = coinsCount.ToString();
        }
        
        private void OnButtonClick()
        {
            ScreenManager.OpenScreen(ScreensType.MENU_SCREEN);
            Time.timeScale = 0.0f;
        }

        public override void Hide()
        {
            
            GameController.onCoinsChanged -= OnCoinsChanged;
            GameController.onGameRestart -= ClearScreen;
            _menuButton.onClick.RemoveListener(OnButtonClick);
            base.Hide();
        }
    }
}