using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Script.Runtime
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _minTimeTurnAround;
        [SerializeField] private float _maxTimeTurnAround;

        private LevelManager _levelManager;
        private bool _isTurned = true;
        private bool _isClockwise = true;
        private GameObject _obstacle;
        
        private void Awake()
        {
            CreateObstacle();
        }

        private void Update()
        {
            if (_isTurned)
            {
                _isTurned = false;
                RotationLoopAsync().Forget();
            }
        }

        public void Init(LevelManager levelManager)
        {
            _levelManager =  levelManager;
        }

        private void CreateObstacle()
        {
            var obstacle = Instantiate(_obstacles[Random.Range(0, _obstacles.Length)], _spawnPoint);
            _obstacle = obstacle;
        }

        private async UniTaskVoid RotationLoopAsync()
        {
            var rotationAxis = Vector3.up * (_isClockwise ? -1f : 1f);
            var duration = Random.Range(_minTimeTurnAround, _maxTimeTurnAround);
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                if (!_obstacle) return;
                _obstacle.transform.Rotate(rotationAxis, _angularSpeed * Time.deltaTime);
                await UniTask.NextFrame();
            }
            _isTurned = true;
            _isClockwise = !_isClockwise;
        }
    }
}