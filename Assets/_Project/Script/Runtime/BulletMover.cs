using UnityEngine;

namespace _Project.Script.Runtime
{
    public class BulletMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private TankController _tankController;
        
        public void Init(TankController tankController) => _tankController = tankController;

        private void Update()
        {
            BulletMove();
        }

        private void BulletMove()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "BulletDestroyer")
            {
                Destroy(gameObject);
            }
            
            else if (other.tag == "Obstacle")
            {
                _tankController?.Die();
                Destroy(gameObject);
            }
        }
    }
}