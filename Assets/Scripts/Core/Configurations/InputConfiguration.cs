using FootBallNet.NetPlayer;
using UnityEngine;

namespace FootBallNet
{
    public class InputConfiguration : Configuration
    {
        public LocalPlayer LocalPlayer;

        public Vector3 AdminCameraPosition = new Vector3(0, 3.26f, 3.76f);
        public Vector3 AdminCameraRotation = new Vector3(36, 180, 0);
    }
}
