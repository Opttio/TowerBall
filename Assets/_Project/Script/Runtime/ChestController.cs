using System;
using UnityEngine;

namespace _Project.Script.Runtime
{
    public class ChestController : MonoBehaviour
    {
        [SerializeField] private GameObject _chestPrefab;
        
        private GameObject _currentChest;

        public void CreateChest(Vector3 position, Transform parent)
        {
            if (_currentChest)
                return;
            _currentChest = Instantiate(_chestPrefab, position, parent.rotation, parent);
        }

        public void DestroyChest()
        {
            if (_currentChest)
            {
                Destroy(_currentChest);
                _currentChest = null;
            }
        }
    }
}