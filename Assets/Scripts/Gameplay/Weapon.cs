using Photon.Pun;
using UnityEngine;

namespace FootBallNet.Gameplay
{
    public class Weapon : MonoBehaviourPun
    {
        public Transform ShootPoint => _spawnPoint;
        public float BulletSpeed => _bulletSpeed;
        public int ScoreAmount => _scoreAmount;

        [SerializeField] private float _timeBetweenSingleShoot = 0.5f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _scoreAmount;

        private double _lastSingleShoot;
        private bool _isShooting;
        private Color _weaponColor;
        private CachedService _cachedService;

        private void Awake()
        {
            _cachedService = Engine.GetService<CachedService>();
        }

        public Color GetWeaponColor()
        {
            return _weaponColor;
        }

        public void SetWeaponColor(Color color)
        {
            _weaponColor = color;
            _meshRenderer.material.color = color;
        }

        public void Shoot()
        {
            if (_lastSingleShoot + _timeBetweenSingleShoot <= PhotonNetwork.Time)
            {
                Engine.RPC(nameof(RPC_Fire), RpcTarget.All);
                _lastSingleShoot = PhotonNetwork.Time;
                return;
            }

            var ball = _cachedService.Spawn(_ballPrefab);
            ball.transform.position = _spawnPoint.position;
        }

        [PunRPC]
        public void RPC_Fire()
        {
            Engine.GetService<CachedService>().Spawn(_ballPrefab).Activate(this);
        }
    }
}
