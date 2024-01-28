using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace FootBallNet.Gameplay
{
    public class Weapon : MonoBehaviour
    {
        public Transform ShootPoint => _spawnPoint;
        public float BallSpeed => _ballSpeed;
        public int ScoreAmount => _scoreAmount;

        [SerializeField] private float _timeBetweenSingleShoot = 0.5f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private int _scoreAmount;

        private double _lastSingleShoot;
        private bool _isShooting;
        private Color _weaponColor;
        private float _ballSpeed;
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

        public void DisableMesh()
        {
            _meshRenderer.enabled = false;
        }

        public void Shoot(float speed, Player shootPlayer)
        {
            _ballSpeed = speed;

            if (_lastSingleShoot + _timeBetweenSingleShoot <= PhotonNetwork.Time)
            {
                Engine.GetService<CachedService>().Spawn(_ballPrefab).Activate(this, shootPlayer);
                _lastSingleShoot = PhotonNetwork.Time;
                return;
            }

            var ball = _cachedService.Spawn(_ballPrefab);
            ball.transform.position = _spawnPoint.position;
        }
    }
}
