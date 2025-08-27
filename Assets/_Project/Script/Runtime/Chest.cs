using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using UnityEngine;

namespace _Project.Script.Runtime
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Material[] _chestMaterials;
        private int _materialIndex = 0;

        private void Start()
        {
            SetMaterial(_materialIndex);
        }

        private void SetMaterial(int index)
        {
            if (index < 0 || index >= _chestMaterials.Length)
                return;
            
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.material = _chestMaterials[index];
            }
            _materialIndex++;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<BulletMover>())
                return;
            if (_materialIndex < _chestMaterials.Length)
            {
                SetMaterial(_materialIndex);
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
                var tank = FindFirstObjectByType<TankController>();
                if (tank) 
                    tank.DisableInputSystem();
                GameModels.ViewId = 3;
                GameEventBus.ChangeView(GameModels.ViewId);
            }
        }
    }
}