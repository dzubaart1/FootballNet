using System;
using Photon.Pun;

namespace FootBallNet
{
    public static class ExtensionMethods
    {
        public static string GetEnumName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }
        public static int GetID(this MonoBehaviourPun networkObject)
        {
            return networkObject.photonView.ViewID;
        }
    }
}