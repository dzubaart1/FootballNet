using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace FootBallNet
{
    public class NetworkCachedService : Service
    {
        public Transform Container { get; private set; }
        private Dictionary<string, Stack<MonoBehaviourPun>> _cached;
        
        public override Task InitializeServiceAsync()
        {
            Container = Engine.CreateObject("NetworkCachedService").transform;
            _cached = new Dictionary<string, Stack<MonoBehaviourPun>>();
            
            return Task.CompletedTask;
        }
    
        public override void ResetService()
        {
        }
    
        public override void DestroyService()
        {
        }
        
        public T Spawn<T>(string prefabPath) where T:MonoBehaviourPun
        {
            if (!_cached.TryGetValue(prefabPath, out var stack))
            {
                stack = new Stack<MonoBehaviourPun>();
                _cached[prefabPath] = stack;
            }

            var cached = stack.FirstOrDefault(x => !x.gameObject.activeSelf);
            
            if (cached is null)
            {
                cached = PhotonNetwork.Instantiate(prefabPath, Vector3.zero, Quaternion.identity).GetComponent<MonoBehaviourPun>();
                stack.Push(cached);
                Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_SetNetworkObjectParentAsNetworkHolder), RpcTarget.All, cached.photonView.ViewID);
                return (T) Convert.ChangeType(cached, typeof(T));
            }

            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_ActivateObject), RpcTarget.All, cached.photonView.ViewID);
            return (T) Convert.ChangeType(cached, typeof(T));
        }
    }
}