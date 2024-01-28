using System.Collections;
using UnityEngine;

namespace FootBallNet.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _lifeTimeBall;

        private Rigidbody _rigidbody;
        private int _scoreAmount = 1;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(BulletDisable());
        }

        public void Activate(Weapon weapon)
        {
            var force = weapon.ShootPoint.forward * weapon.BulletSpeed;
            _scoreAmount = weapon.ScoreAmount;

            transform.position = weapon.ShootPoint.position;
            transform.rotation = Quaternion.LookRotation(-force);
            _rigidbody.AddForce(force);
        }

        private void OnTriggerEnter(Collider other)
        {
            /*if (!other.TryGetComponent<IHealth>(out var health))
            {
                return;
            }

            health.ApplyDamage(_damage);
            gameObject.SetActive(false);*/
        }

        private IEnumerator BulletDisable()
        {
            yield return new WaitForSeconds(_lifeTimeBall);

            _rigidbody.velocity = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
        }

    }
}
