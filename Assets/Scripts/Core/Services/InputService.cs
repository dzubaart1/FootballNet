using System.Threading.Tasks;
using FootBallNet.NetPlayer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace FootBallNet
{
    public class InputService : Service<InputConfiguration>
    {
        public ETargetPlatform Platform = ETargetPlatform.Auto;

        public Camera Camera { get; private set; }
        public LocalPlayer LocalPlayer { get; private set; } = null;

        public override Task InitializeServiceAsync()
        {       
            var obj = Engine.CreateObject("InputManager", null, typeof(EventSystem));


            obj.AddComponent<InputSystemUIInputModule>();
            Camera = Engine.CreateObject("CameraManager", null, typeof(Camera)).GetComponent<Camera>();

            Camera.transform.position = Configuration.AdminCameraPosition;
            Camera.transform.eulerAngles = Configuration.AdminCameraRotation;
            
            return Task.CompletedTask;
        }

        public override void ResetService() { }
        public override void DestroyService() { }
    }
}