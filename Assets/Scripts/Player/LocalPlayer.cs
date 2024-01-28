using FootBallNet.Gameplay;
using Photon.Pun;
using UnityEngine;

namespace FootBallNet.NetPlayer
{
    public class LocalPlayer : MonoBehaviour
    {
        public Camera Camera => _camera;
        public Weapon Weapon => _weapon;

        [SerializeField] private Camera _camera;
        [SerializeField] private float _sensitivity = 15f;
        [SerializeField] private Weapon _weapon;

        private float _dx = 0, _dy = 0;
        private double _powerTime;
        private Vector2 _mouseDelta;
        private NetworkService _networkService;

        private void Awake()
        {
            Engine.GetService<InputService>().PlayerEvents.Player.LBMouse.started += Power;
            Engine.GetService<InputService>().PlayerEvents.Player.LBMouse.performed += Shoot;
            Engine.GetService<InputService>().PlayerEvents.Player.MouseDelta.performed += RotateCamera;

            _networkService = Engine.GetService<NetworkService>();
        }
        private void Power(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _powerTime = PhotonNetwork.Time;
        }

        private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var offsetTime = (float)(PhotonNetwork.Time - _powerTime) * 1000;
            var speed = Mathf.Clamp(offsetTime, 500, 1500);


            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_Fire), RpcTarget.All, _networkService.NetworkPlayer.photonView.ViewID, speed);
        }

        private void RotateCamera(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _mouseDelta = context.ReadValue<Vector2>();

            _dx += _mouseDelta.y * _sensitivity * Time.deltaTime;
            _dy += _mouseDelta.x * _sensitivity * Time.deltaTime;

            _camera.transform.eulerAngles = new Vector3(_dx, _dy, 0f);
        }
    }
}
