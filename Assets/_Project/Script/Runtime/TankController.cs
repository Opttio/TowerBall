using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Script.Runtime
{
    public class TankController : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private int _spawnTimerMs;
        [SerializeField] private float _tankSpeed = 5f;
        [SerializeField] private float _tankRotationSpeed = 25f;

        private MyInputSystem _myInputSystem;

        private bool _isShooting =  false;
        private bool _isShootLoopRunning = false;
        private Transform[] _towerPoints;

        private void Awake()
        {
            _myInputSystem = new MyInputSystem();
            _myInputSystem.Character.Fire.performed += StartShooting;
            _myInputSystem.Character.Fire.canceled += StopShooting;
        }

        public void Init(Transform[] towerPoints) => _towerPoints = towerPoints;

        public void EnableInputSystem() => _myInputSystem.Enable();
        public void DisableInputSystem() => _myInputSystem.Disable();

        public void Die()
        {
            _isShooting = false;
            Destroy(gameObject);
            GameModels.ViewId = 2;
            GameEventBus.ChangeView(GameModels.ViewId);
            DisableInputSystem();
        }
        private void StartShooting(InputAction.CallbackContext obj)
        {
            _isShooting = true;
            if (!_isShootLoopRunning)
                ShootLoopAsync(_bulletPrefab, _bulletSpawnPoint).Forget();
        }

        private void StopShooting(InputAction.CallbackContext obj)
        {
            _isShooting = false;
        }

        private async UniTaskVoid ShootLoopAsync(GameObject bullet, Transform position)
        {
            _isShootLoopRunning = true;
            var token = this.GetCancellationTokenOnDestroy();
            while (_isShooting && !token.IsCancellationRequested)
            {
                var bulletGo = Instantiate(bullet, position.position, position.rotation, _bulletHolder);
                var bulletMover = bulletGo.GetComponent<BulletMover>();
                bulletMover.Init(this);
                await UniTask.Delay(_spawnTimerMs, cancellationToken: token);
            }
            _isShootLoopRunning =  false;
        }

        public void MoveToTheNextTarget(int index)
        {
            if (_towerPoints == null || index >= _towerPoints.Length - 1)
                return;
            MoveTankAsync(index).Forget();
        }

        private async UniTask MoveTankAsync(int targetIndex)
        {
            Vector3 target = _towerPoints[targetIndex].position;
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 direction = (target - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _tankSpeed);
                await UniTask.NextFrame();
            }
            transform.position = target;
            int nextIndex = targetIndex + 1;
            Vector3 nextTarget = _towerPoints[nextIndex].position;
            RotateTankAsync(nextTarget).Forget();
        }

        private async UniTask RotateTankAsync(Vector3 lookTarget)
        {
            Vector3 direction = (lookTarget - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _tankRotationSpeed * Time.deltaTime);
                await UniTask.NextFrame();
            }
            transform.rotation = targetRotation;
        }
    }
}