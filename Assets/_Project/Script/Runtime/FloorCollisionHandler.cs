using UnityEngine;

namespace _Project.Script.Runtime
{
    public class FloorCollisionHandler : MonoBehaviour
    {
        private TowerController _towerController;
        
        public void Init(TowerController towerController) => _towerController = towerController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BulletMover>())
            {
                _towerController.OnFloorHit(gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}