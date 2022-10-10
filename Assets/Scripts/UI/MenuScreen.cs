using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MenuScreen : Screen
    {
        [SerializeField] private Button _close;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exitGame;

        public override void Show()
        {
            _close.onClick.AddListener(OnCloseClick);
            _restart.onClick.AddListener(OnRestartClick);
            _exitGame.onClick.AddListener(OnExitClick);
            base.Show();
        }

        private void OnCloseClick()
        {
            ScreenManager.CloseScreen();
            Time.timeScale = 1.0f;
        }

        private void OnRestartClick()
        {
            ScreenManager.CloseScreen();
            Time.timeScale = 1.0f;
            GameController.instance.RestartGame();
        }

        private void OnExitClick()
        {
            Application.Quit();
        }

        public override void Hide()
        {
            _close.onClick.RemoveListener(OnCloseClick);
            _restart.onClick.RemoveListener(OnRestartClick);
            _exitGame.onClick.RemoveListener(OnExitClick);
            base.Hide();
        }
    }
}