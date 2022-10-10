using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScreen : Screen
    {
        [SerializeField] private Button _button;
        
        public override void Show()
        {
            _button.onClick.AddListener(OnButtonClick);
            base.Show();
        }

        private void OnButtonClick()
        {
            ScreenManager.CloseScreen();
            ScreenManager.OpenScreen(ScreensType.GAME_SCREEN);
        }
        public override void Hide()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            base.Hide();
        }
    }
}