using FootBallNet.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace FootBallNet.NetPlayer
{
    public class NetworkPlayer : MonoBehaviourPun
    {
        public Player Player { get; set; } = null;
        public Weapon Weapon => _weapon;

        [SerializeField] private Weapon _weapon;

        private Transform _originGun;

        private InputService _inputService;

        private void Awake()
        {
            _inputService = Engine.GetService<InputService>();

            if (!PhotonNetwork.IsMasterClient || photonView.InstantiationData == null)
                return;

            Engine.GetService<NetworkService>().PlayerLeftRoomEvent += OnPlayerLeftEvent;
            Player = photonView.InstantiationData[0] as Player;
            photonView.TransferOwnership(Player);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                SetupLocalRig(_inputService.LocalPlayer);
            }
            else
            {
                Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_TryInitPlayerColorsRequest), PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer, photonView.InstantiationData[0] as Player, photonView.ViewID);
            }

            if((photonView.InstantiationData[0] as Player).IsLocal)
            {
                Weapon.DisableMesh();
            }
        }

        private void Update()
        {
            if (photonView.IsMine && _originGun != null)
            {
                MapPosition(_weapon.transform, _originGun.transform);
                MapPosition(transform, _inputService.LocalPlayer.Camera.transform);
            }
        }

        private void OnDestroy()
        {
            Engine.GetService<NetworkService>().PlayerLeftRoomEvent -= OnPlayerLeftEvent;
        }

        private void OnPlayerLeftEvent(Player player)
        {
            if (!Player.Equals(player))
                return;

            Engine.GetService<NetworkService>().PlayerLeftRoomEvent -= OnPlayerLeftEvent;
            PhotonNetwork.Destroy(gameObject);
        }

        private void SetupLocalRig(LocalPlayer localPlayer)
        {
            _originGun = localPlayer.Weapon.transform;
        }

        private void MapPosition(Transform current, Transform target)
        {
            current.position = target.transform.position;
            current.rotation = target.transform.rotation;
        }
    }
}
