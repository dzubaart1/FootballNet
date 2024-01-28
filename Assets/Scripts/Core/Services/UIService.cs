using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FootBallNet
{
    public class UIService : Service<UIServiceConfiguration>
    {
        private List<UIBehaviour> _allUI = new List<UIBehaviour>();
        private Dictionary<Type, UIBehaviour> _cachedUI = new Dictionary<Type, UIBehaviour>();
        
        private Transform _container;
        
        public override Task InitializeServiceAsync()
        {
            _container = Engine.CreateObject("UIManager").transform;
            
            foreach (var uiBehaviour in Configuration.UIs)
                InstantiateUI(uiBehaviour);
            
            return Task.CompletedTask;
        }

        public T GetUI<T>() where T : UIBehaviour
        {
            if (_cachedUI.TryGetValue(typeof(T), out var cachedResult))
                return (T) cachedResult;
            
            foreach (var uiBehaviour in _allUI)
                if (uiBehaviour is T)
                {
                    _cachedUI[typeof(T)] = uiBehaviour;
                    return (T) uiBehaviour;
                }

            return null;
        }
        
        public void AddUI(UIBehaviour ui)
        {
            if (_allUI.Contains(ui))
                return;
            
            _allUI.Add(ui);
        }

        public void RemoveUI(UIBehaviour ui)
        {
            if (!_allUI.Contains(ui))
                return;
            
            _allUI.Remove(ui);
        }

        private UIBehaviour InstantiateUI(UIBehaviour uiBehaviour, Transform parent = null)
        {
            var ui = Engine.Instantiate(uiBehaviour, parent ? parent : _container);
            AddUI(ui);

            return ui;
        }

        public override void ResetService() {}

        public override void DestroyService() {}
    }
}
