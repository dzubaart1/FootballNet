using System.Threading.Tasks;
using FootBallNet.NetPlayer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace FootBallNet
{
    public class InputService : Service<InputConfiguration>
    {
        public PlayerEvents PlayerEvents { get; private set; }
        public Camera Camera { get; private set; }
        public LocalPlayer LocalPlayer { get; private set; } = null;

        private ColorsService _colorsService;

        public override Task InitializeServiceAsync()
        {
            PlayerEvents = new PlayerEvents();

            LocalPlayer = Engine.Instantiate(Configuration.LocalPlayer);

            _colorsService = Engine.GetService<ColorsService>();
            _colorsService.PlayerChooseColorEvent += OnPLayerChooseColor;

            var obj = Engine.CreateObject("InputManager", null, typeof(EventSystem));
            obj.AddComponent<InputSystemUIInputModule>();

            return Task.CompletedTask;
        }

        public void OnPLayerChooseColor()
        {
            PlayerEvents.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void ResetService() { }
        public override void DestroyService()
        {
            _colorsService.PlayerChooseColorEvent -= OnPLayerChooseColor;
        }
    }
}