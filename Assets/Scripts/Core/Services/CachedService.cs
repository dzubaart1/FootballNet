using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace FootBallNet
{
    public class CachedService : Service
    {
        private Dictionary<MonoBehaviour, Stack<MonoBehaviour>> _cached;
        private Transform _container;
        
        public override Task InitializeServiceAsync()
        {
            _container = Engine.CreateObject("CachedService").transform;
            _cached = new Dictionary<MonoBehaviour, Stack<MonoBehaviour>>();
            
            return Task.CompletedTask;
        }
    
        public override void ResetService()
        {
        }
    
        public override void DestroyService()
        {
        }

        public T Spawn<T>(T prefab) where T: MonoBehaviour
        {
            if (!_cached.TryGetValue(prefab, out var stack))
            {
                stack = new Stack<MonoBehaviour>();
                _cached[prefab] = stack;
            }

            var cached = stack.FirstOrDefault(x => !x.gameObject.activeSelf);
            
            if (cached is null)
            {
                cached = Engine.Instantiate(prefab, _container);
                stack.Push(cached);
                return (T) Convert.ChangeType(cached, typeof(T));
            }
            
            cached.gameObject.SetActive(true);
            return (T) Convert.ChangeType(cached, typeof(T));
        }
    }
}