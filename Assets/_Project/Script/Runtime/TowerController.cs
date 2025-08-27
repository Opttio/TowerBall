using System.Collections.Generic;
using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using UnityEngine;

namespace _Project.Script.Runtime
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private int _minTowerFloors;
        [SerializeField] private int _maxTowerFloors;
        [SerializeField] private int _towerRenderFloors = 10;
        [SerializeField] private GameObject _floorPrefab;
        [SerializeField] private Material[] _floorMaterials;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _floorsHolder;
        [SerializeField] private ParticleSystem _particle;

        private List<GameObject> _tower = new List<GameObject>();
        private int _floorsLeft;
        private LevelManager _levelManager;
        private readonly float _floorHeight = 2.05f;

        private void Start()
        {
            CreateTower();
        }

        public void Init(LevelManager levelManager)
        {
            _levelManager =  levelManager;
        }

        private void CreateTower()
        {
            int totalFloors = Random.Range(_minTowerFloors, _maxTowerFloors);
            _floorsLeft = totalFloors;
            _tower.Clear();
            for (int i = 0; i < totalFloors; i++)
            {
                Vector3 floorPosition = _spawnPoint.position + Vector3.up * i * _floorHeight;
                GameObject floor = Instantiate(_floorPrefab, floorPosition, Quaternion.identity, _floorsHolder);
                
                FillFloorByColor(floor);
                _tower.Add(floor);

                if (i >= _towerRenderFloors)
                    floor.SetActive(false);

                FloorCollisionHandler handler = floor.AddComponent<FloorCollisionHandler>();
                handler.Init(this);
            }
            GameModels.TowerHigh = totalFloors;
            GameEventBus.ChangeTowerHeight(GameModels.TowerHigh);
        }

        private void FillFloorByColor(GameObject floor)
        {
            if (_floorMaterials.Length == 0)
                return;
            floor.GetComponentInChildren<MeshRenderer>().material = _floorMaterials[Random.Range(0, _floorMaterials.Length)];
        }

        public void OnFloorHit(GameObject floor)
        {
            if (!floor)
                return;
            if (_tower.Contains(floor))
            {
                PlayEffect(floor.transform.position);
                _tower.Remove(floor);
                Destroy(floor);
                _floorsLeft--;
                GameModels.TowerHigh = _floorsLeft;
                GameEventBus.ChangeTowerHeight(GameModels.TowerHigh);
                _floorsHolder.Translate(Vector3.down * _floorHeight);
                for (int i = 0; i < _tower.Count; i++)
                {
                    if (i < _towerRenderFloors)
                        _tower[i].SetActive(true);
                    else
                        _tower[i].SetActive(false);
                }
            }
            if (_floorsLeft == 0)
            {
                _levelManager.OnTowerDestroyed();
                Destroy(gameObject);
            }
        }

        private void PlayEffect(Vector3 position)
        {
            ParticleSystem particleInstance = Instantiate(_particle, position, Quaternion.identity);
            particleInstance.Play();
            Destroy(particleInstance.gameObject, particleInstance.main.duration + particleInstance.main.startLifetime.constantMax);
            
        }
    }
}