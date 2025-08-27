using System;
using _Project.Script.Core.EventBus;
using TMPro;
using UnityEngine;

namespace _Project.Script.UI.Views
{
    public class TowerCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _towerHeightText;

        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnsubscribeFromEvents();

        private void SubscribeToEvents()
        {
            GameEventBus.OnTowerHeightChanged += ChangeHeight;
        }

        private void UnsubscribeFromEvents()
        {
            GameEventBus.OnTowerHeightChanged -= ChangeHeight;
        }

        private void ChangeHeight(int height)
        {
            _towerHeightText.text = height.ToString();
        }
    }
}