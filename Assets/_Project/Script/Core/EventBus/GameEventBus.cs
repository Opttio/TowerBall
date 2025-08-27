using System;

namespace _Project.Script.Core.EventBus
{
    public static class GameEventBus
    {
        public static event Action<int> OnTowerHeightChanged;
        public static event Action<int> OnSecondsChanged;
        public static event Action<int> OnViewChanged;
        public static event Action<bool> OnPause;
        public static event Action OnRestart;
        
        public static void ChangeTowerHeight(int value) =>  OnTowerHeightChanged?.Invoke(value);
        public static void ChangeSeconds(int value) =>  OnSecondsChanged?.Invoke(value);
        public static void ChangeView(int viewId) => OnViewChanged?.Invoke(viewId);
        public static void PauseGameTrigger (bool isPause) => OnPause?.Invoke(isPause);
        public static void RestartTrigger() => OnRestart?.Invoke();
    }
}