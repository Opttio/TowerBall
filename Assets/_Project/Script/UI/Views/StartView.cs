using _Project.Script.Core.EventBus;
using _Project.Script.Runtime;
using _Project.Script.UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Script.UI.Views
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnsubscribeFromEvents();
        private void SubscribeToEvents() => _startButton.onClick.AddListener(SignalToStartGame);
        private void UnsubscribeFromEvents() => _startButton.onClick.RemoveListener(SignalToStartGame);

        private void SignalToStartGame()
        {
            GameModels.ViewId = 1;
            GameEventBus.ChangeView(GameModels.ViewId);
            GameModels.IsPaused =  false;
            GameEventBus.PauseGameTrigger(GameModels.IsPaused);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            var tank = FindFirstObjectByType<TankController>();
            if (tank) 
                tank.EnableInputSystem();
        }
    }
}