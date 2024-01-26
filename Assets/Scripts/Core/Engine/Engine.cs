using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

[assembly: AlwaysLinkAssembly, Preserve]
namespace FootBallNet
{
    public class Engine
    {
        private static Action PostInitialization;
        
        public static bool IsInitialized { get; private set; } = false;
        public static RuntimeBehaviour Behaviour { get; private set; }
        public static NetworkBehaviour NetworkBehaviour { get; private set; }
        public static IReadOnlyCollection<Type> Types => _typesCache ?? (_typesCache = GetEngineTypes());

        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
        private static ConfigurationProvider _configurationProvider;
        private static IReadOnlyCollection<Type> _typesCache;
        
        public static Task Initialize(ConfigurationProvider configurationProvider, RuntimeBehaviour behaviour)
        {
            Behaviour = behaviour;
            _configurationProvider = configurationProvider;

            AddService(new InputService());
            AddService(new AudioService());
            AddService(new SceneSwitchingService());
            AddService(new UIService());
            AddService(new NetworkService());
            AddService(new CachedService());
            AddService(new NetworkCachedService());

            var services = _services.Values.ToList();
            
            foreach (var service in services)
                service.InitializeServiceAsync();
            
            PostInitialization?.Invoke();
            
            IsInitialized = true;
            behaviour.BehaviourDestroyEvent += OnDestroy;
            
            return Task.CompletedTask;
        }

        public static void AddPostInitializeTask(Action action)
        {
            PostInitialization += action;
        }
        
        public static void RemovePostInitializeTask(Action action)
        {
            PostInitialization -= action;
        }
        
        public static void InitializeNetworkBehaviour(NetworkBehaviour behaviour)
        {
            NetworkBehaviour = behaviour;
        }

        public static void DestroyNetworkBehaviour()
        {
            Destroy(NetworkBehaviour);
        }

        public static void OnDestroy()
        {
            foreach (var service in _services.Values)
                service.DestroyService();
            
            _services.Clear();
        }

        public static T Instantiate<T>(T prototype, Transform parent = default) where T : Object
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            var newObj = Object.Instantiate(prototype, parent ? parent : Behaviour.transform);
            return newObj;
        }

        public static GameObject CreateObject(string name = default, Transform parent = default,
            params Type[] components)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            var objName = name ?? "PlatformObject";
            GameObject newObj = components != null ? new GameObject(objName, components) : new GameObject(objName);
            newObj.transform.SetParent(parent ? parent : Behaviour.transform);

            return newObj;
        }

        public static void Destroy<T>(T obj, float seconds = 0) where T:Object
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");
            
            Object.Destroy(obj, seconds);
        }

        public static void RPC(string methodName, RpcTarget target, params object[] parameters)
        {
            if (NetworkBehaviour is null)
                throw new Exception("Network behaviour doesn't exists.");
            
            NetworkBehaviour.photonView.RPC(methodName, target, parameters);
        }
        
        public static void RPC(string methodName, Player target, params object[] parameters)
        {
            if (NetworkBehaviour is null)
                throw new Exception("Network behaviour doesn't exists.");
            
            NetworkBehaviour.photonView.RPC(methodName, target, parameters);
        }
        
        public static void AddService<T>(T service) where T: IService
        {
            if (_services.ContainsKey(typeof(T)))
                throw new Exception($"Service {typeof(T)} already exists");
                
            _services.Add(typeof(T), service);
        }

        public static void RemoveService<T>() where T: IService
        {
            if (_services.ContainsKey(typeof(T)))
                _services.Remove(typeof(T));
            else
                throw new Exception($"Service {typeof(T)} doesn't exists");
        }

        public static T GetService<T>() where T : class, IService
        {
            if (_services.ContainsKey(typeof(T)))
                return (T) _services[typeof(T)];

            Type type = typeof(T);
            var result = _services.FirstOrDefault(x => type.IsInstanceOfType(x.Value));

            if (result.Value is null)
                throw new Exception($"Service {typeof(T)} doesn't exists");

            return (T) result.Value;
        }

        public static T GetConfiguration<T>() where T: Configuration
        {
            if (_configurationProvider is null)
                throw new Exception($"Failed to provide `{typeof(T).Name}` configuration object: Configuration provider is not available or the engine is not initialized.");

            return (T) _configurationProvider.GetConfiguration(typeof(T));
        }

        public static void StartCoroutine(IEnumerator enumerator)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            Behaviour.StartCoroutine(enumerator);
        }

        public static void StopCoroutine(IEnumerator enumerator)
        {
            if (Behaviour is null)
                throw new Exception("Engine is not initialized");

            Behaviour.StopCoroutine(enumerator);
        }
        
        private static IReadOnlyCollection<Type> GetEngineTypes()
        {
            var engineTypes = new List<Type>(1000);
            var engineConfig = ConfigurationProvider.LoadOrDefault<EngineConfiguration>();
            var domainAssemblies = ReflectionUtils.GetDomainAssemblies(true, true, true);
            
            foreach (var assemblyName in engineConfig.TypeAssemblies)
            {
                var assembly = domainAssemblies.FirstOrDefault(a => a.FullName.StartsWithFast($"{assemblyName}"));
                if (assembly is null) continue;
                engineTypes.AddRange(assembly.GetExportedTypes());
            }
            
            return engineTypes;
        }
    }
}
