using _Project.Script.Core.EventBus;
using _Project.Script.Runtime;
using _Project.Script.UI.Models;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace _Project.Script.UI.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _secondsText;
        [SerializeField] private Image _yellowStar1;
        [SerializeField] private Image _yellowStar2;
        [SerializeField] private Image _yellowStar3;

        private int _secondsOnLevel;
        
        private void OnEnable()
        {
            SubscribeToEvents();
            _secondsOnLevel = GameModels.Seconds;
        }

        private void OnDisable() => UnsubscribeFromEvents();

        private void SubscribeToEvents()
        {
            GameEventBus.OnSecondsChanged += ChangeSeconds;
            GameEventBus.OnSecondsChanged += ChangeStars;
        }

        private void UnsubscribeFromEvents()
        {
            GameEventBus.OnSecondsChanged -= ChangeSeconds;
            GameEventBus.OnSecondsChanged -= ChangeStars;
        }

        private void ChangeSeconds(int seconds)
        {
            _secondsText.text = seconds.ToString();
            if (_secondsOnLevel == 0 && seconds > 0)
                _secondsOnLevel = seconds;
            if (seconds <= 0)
            {
                var tank = FindFirstObjectByType<TankController>();
                if (tank)
                    tank.Die();
            }
        }

        private void ChangeStars(int seconds)
        {
            // Debug.Log(_secondsOnLevel);
            float percentLeft = (float)seconds / _secondsOnLevel;
            if (percentLeft < 0.7f) 
                _yellowStar3.enabled = false;
            if (percentLeft < 0.3f) 
                _yellowStar2.enabled = false;
            if (seconds <= 0) 
                _yellowStar1.enabled = false;
        }
    }
}