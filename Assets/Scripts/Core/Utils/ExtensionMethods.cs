using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FootBallNet
{
    public static class ExtensionMethods
    {
        public static void SetSettings(this AudioSource audioSource, AudioClip clip, bool loop, float spatialBlend, float volume)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.spatialBlend = spatialBlend;
            audioSource.volume = volume;
        }

        public static string GetValue(this Enum value)
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            return attributes?.Length > 0 ? attributes[0].Value : null;
        }

        public static string GetEnumName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        public static T[] FindObjectsOfType<T>(this Scene scene, bool includeInactive = false) where T : class
        {
            var objList = new List<T>();

            foreach (var rootGameObject in scene.GetRootGameObjects())
                objList.AddRange(rootGameObject.GetComponentsInChildren<T>(includeInactive));

            return objList.ToArray();
        }

        public static int GetID(this MonoBehaviourPun networkObject)
        {
            return networkObject.photonView.ViewID;
        }
    }
}