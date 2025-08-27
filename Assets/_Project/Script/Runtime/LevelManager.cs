using System;
using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using UnityEngine;

namespace _Project.Script.Runtime
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _towerSpawnPoints;
        [SerializeField] private TowerController _towerPrefab;
        [SerializeField] private ObstacleController _obstaclePrefab;
        [SerializeField] private int _secondsOnLevel;
        [SerializeField] private ChestController _chestControllerPrefab;

        private int _index = 0;
        private TowerController _activeTower;
        private ObstacleController _activeObstacle;
        private TankController _tank;
        private ChestController _chestControllerInstance;
        private Vector3 _lastTowerPosition;
        private Transform _lastTowerParent;

        private void Start()
        {
            InitChestController();
            InitTank();
            SpawnNextTower();

            GameModels.Seconds = _secondsOnLevel;
            GameEventBus.ChangeSeconds(GameModels.Seconds);
            SecondsCounter.StartSecondsCounter();
        }

        private void InitTank()
        {
            _tank = FindFirstObjectByType<TankController>();
            _tank.Init(_towerSpawnPoints);
        }

        private void InitChestController()
        {
            if (!_chestControllerInstance && _chestControllerPrefab)
            {
                _chestControllerInstance = Instantiate(_chestControllerPrefab);
            }
        }

        public void OnTowerDestroyed()
        {
            if (_activeTower)
            {
                _lastTowerPosition = _activeTower.transform.position;
                _lastTowerParent = _activeTower.transform.parent;

                if (_activeObstacle)
                {
                    Destroy(_activeObstacle.gameObject);
                    _activeObstacle = null;
                }

                Destroy(_activeTower.gameObject);
                _activeTower = null;
            }

            _tank.MoveToTheNextTarget(_index - 1);

            if (_index >= _towerSpawnPoints.Length)
            {
                if (_chestControllerInstance)
                {
                    _chestControllerInstance.CreateChest(_lastTowerPosition, _lastTowerParent);
                }
            }
            else
            {
                SpawnNextTower();
            }
        }

        private void SpawnNextTower()
        {
            if (_index >= _towerSpawnPoints.Length)
                return;

            var spawnPoint = _towerSpawnPoints[_index];

            _activeTower = Instantiate(_towerPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint.parent);
            _activeTower.Init(this);

            _activeObstacle = Instantiate(_obstaclePrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
            _activeObstacle.Init(this);

            _index++;
        }
    }
}