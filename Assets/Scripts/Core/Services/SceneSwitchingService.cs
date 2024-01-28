using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FootBallNet
{
    public class SceneSwitchingService : Service
    {
        public Action SceneLoadedEvent;
        public Action SceneUnloadedEvent;
        public Action SceneChangedEvent;

        public Scene CurrentScene { get; private set; }
        
        private int _currentScene = -1;
        private List<string> _availableScene = new List<string>();

        public override Task InitializeServiceAsync()
        {
            InitializeAvailableMaps();
            Engine.GetConfiguration<SceneConfiguration>();
            
            return Task.CompletedTask;
        }

        public void RPC_LoadScene(EScene scene)
        {
            if (PhotonNetwork.IsMasterClient == false || !TryGetMapName(scene, out var sceneIndex))
                return;
            
            if (_currentScene == sceneIndex)
            {
                Debug.Log($"Scene {scene.GetEnumName()} already loaded.");
                return;
            }

            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_LoadScene), RpcTarget.AllBuffered, (int) scene);
        }

        public void RPC_UnloadScene(EScene scene)
        {
            if (PhotonNetwork.IsMasterClient == false || !TryGetMapName(scene, out var sceneIndex))
                return;
            
            if (_currentScene != sceneIndex)
            {
                Debug.Log($"Scene {scene.GetEnumName()} doesn't loaded.");
                return;
            }

            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_UnloadScene), RpcTarget.All, (int) scene);
        }

        public void LoadScene(int scene)
        {
            if (!TryGetMapName((EScene) scene, out var sceneIndex))
                return;
            
            var op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            op.completed += OnSceneLoaded;
            
            UpdateScene(sceneIndex, op, (EScene) scene);
        }

        public void UnloadScene(int scene)
        {
            if (!TryGetMapName((EScene) scene, out var sceneIndex))
                return;
            
            var op = SceneManager.UnloadSceneAsync(sceneIndex);
            op.completed += OnSceneUnloaded;
            
            UpdateScene(-1, op, (EScene) scene);
        }
        
        public bool TryGetMapName(EScene scene, out int sceneIndex)
        {
            sceneIndex = int.MaxValue;
            var sceneName = scene.GetEnumName();

            if (!_availableScene.Contains(sceneName))
                return false;

            sceneIndex = _availableScene.IndexOf(sceneName);
            return true;
        }

        private void UpdateScene(int sceneIndex, AsyncOperation operation, EScene scene)
        {
            _currentScene = sceneIndex;
            
            if (_currentScene != -1)
                CurrentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        }

        private void OnSceneLoaded(AsyncOperation op)
        {
            op.completed -= OnSceneLoaded;

            SceneLoadedEvent?.Invoke();
            SceneChangedEvent?.Invoke();
        }

        private void OnSceneUnloaded(AsyncOperation op)
        {
            op.completed -= OnSceneUnloaded;

            SceneUnloadedEvent?.Invoke();
            SceneChangedEvent?.Invoke();
        }

        /// <summary>
        /// Получает список имен карт из BuildSettings
        /// </summary>
        private void InitializeAvailableMaps()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCount; i++)
                _availableScene.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }

        public override void ResetService()
        {
        }

        public override void DestroyService()
        {
        }
    }
}
