using Photon.Pun;
using Photon.Realtime;
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
        private bool _isUsed;

        private Player _shootPlayer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(BulletDisable());
        }

        public void Activate(Weapon weapon, Player shootPlayer)
        {
            _shootPlayer = shootPlayer;

            var force = weapon.ShootPoint.forward * weapon.BallSpeed;
            _scoreAmount = weapon.ScoreAmount;

            transform.position = weapon.ShootPoint.position;
            transform.rotation = Quaternion.LookRotation(-force);
            _rigidbody.AddForce(force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isUsed || !collision.gameObject.TryGetComponent<Gate>(out var gate) || !gate.IsInitailized)
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_RemovePointToGate), Photon.Pun.RpcTarget.MasterClient, gate.ID, _scoreAmount, _shootPlayer);
                _isUsed = true;
            }
        }

        private IEnumerator BulletDisable()
        {
            yield return new WaitForSeconds(_lifeTimeBall);

            _rigidbody.velocity = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
            _isUsed = false;
        }

    }
}
