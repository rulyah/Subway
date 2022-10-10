using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum ScreensType
    {
        START_SCREEN,
        GAME_SCREEN,
        MENU_SCREEN,
        LOSE_SCREEN
    }
    public class ScreenManager : MonoBehaviour
    {
        public List<Screen> _screens = new List<Screen>();
        public static List<Screen> _staticScreens;
        private static Stack<Screen> _usedScreens = new Stack<Screen>();
        
        private void Start()
        {
            _staticScreens = _screens;
            OpenScreen(ScreensType.START_SCREEN);
        }

        public static void OpenScreen(ScreensType type)
        {
            var currentScreen = _staticScreens.Find(n => n.type == type);
            currentScreen.Show();
            _usedScreens.Push(currentScreen);
        }

        public static void CloseScreen()
        {
            if (_usedScreens.TryPop(out Screen screen))
            {
                screen.Hide();
            }
        }
        
    }
}