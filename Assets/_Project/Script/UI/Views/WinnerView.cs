using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Script.UI.Views
{
    public class WinnerView : MonoBehaviour
    {
        
        [SerializeField] private Button _restartButton;
        [SerializeField] private Image _yellowStar2;
        [SerializeField] private Image _yellowStar3;
        private int _secondsOnLevel;
        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnsubscribeFromEvents();
        private void SubscribeToEvents()
        {
            _restartButton.onClick.AddListener(SignalRestartGame);
            GameEventBus.OnSecondsChanged += ChangeStars;
        }

        private void UnsubscribeFromEvents()
        {
            _restartButton.onClick.RemoveListener(SignalRestartGame);
            GameEventBus.OnSecondsChanged -= ChangeStars;
        }
        
        private void ChangeStars(int seconds)
        {
            if (_secondsOnLevel == 0 && seconds > 0)
                _secondsOnLevel = seconds;
            Debug.Log($"SecondsOnLevel: {_secondsOnLevel}");
            Debug.Log($"Seconds: {seconds}");
            float percentLeft = (float)seconds / _secondsOnLevel;
            if (percentLeft < 0.7f) 
                _yellowStar3.enabled = false;
            if (percentLeft < 0.3f) 
                _yellowStar2.enabled = false;
        }

        private void SignalRestartGame() => GameEventBus.RestartTrigger();
    }
}