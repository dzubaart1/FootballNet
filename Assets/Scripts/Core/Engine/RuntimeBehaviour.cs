using System;
using UnityEngine;

namespace FootBallNet
{
    public class RuntimeBehaviour : MonoBehaviour
    {
        public event Action BehaviourUpdateEvent;
        public event Action BehaviourLateUpdateEvent;
        public event Action BehaviourDestroyEvent;

        private GameObject _rootGameObject;
        private MonoBehaviour _monoBehaviour;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void Initialize()
        {
            if (Engine.IsInitialized)
                return;
            
            var go = new GameObject("Platform<Runtime>");
            var comp = go.AddComponent<RuntimeBehaviour>();
            
            DontDestroyOnLoad(go);

            comp._rootGameObject = go;
            comp._monoBehaviour = comp;

            await Engine.Initialize(new ConfigurationProvider(), comp);
        }

        private void Update()
        {
            BehaviourUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            BehaviourLateUpdateEvent?.Invoke();
        }

        private void OnDestroy()
        {
            BehaviourDestroyEvent?.Invoke();
        }
    }
}
