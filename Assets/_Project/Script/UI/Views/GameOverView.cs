using _Project.Script.Core.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Script.UI.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnsubscribeFromEvents();
        private void SubscribeToEvents() => _restartButton.onClick.AddListener(SignalRestartGame);
        private void UnsubscribeFromEvents() => _restartButton.onClick.RemoveListener(SignalRestartGame);
        private void SignalRestartGame() => GameEventBus.RestartTrigger();
    }
}