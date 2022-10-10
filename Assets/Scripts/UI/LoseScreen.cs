using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoseScreen : Screen
    {
        [SerializeField] private Button _restart;
        [SerializeField] private TextMeshProUGUI _text;

        public override void Show()
        {
            base.Show();
            _text.text = PlayerController.coinsCount.ToString();
            _restart.onClick.AddListener(OnRestartClick);
        }

        private void OnRestartClick()
        {
            ScreenManager.CloseScreen();
            GameController.instance.RestartGame();
        }
        public override void Hide()
        {           
            _restart.onClick.RemoveListener(OnRestartClick);
            base.Hide();
        }
    }
}