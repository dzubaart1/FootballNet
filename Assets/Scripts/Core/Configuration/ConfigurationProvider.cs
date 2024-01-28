using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FootBallNet
{
    public class ConfigurationProvider
    {
        public const string DefaultResourcesPath = "Configuration";
        public const string DefaultCreatedPath = "Assets/Resources/Configuration";

        private readonly Dictionary<Type, Configuration> _configurations = new Dictionary<Type, Configuration>();

        public ConfigurationProvider(string resourcesPath = DefaultResourcesPath)
        {
            var configTypes = Engine.Types.Where(type =>
                typeof(Configuration).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var configType in configTypes)
            {
                var configAsset = LoadOrDefault(configType, resourcesPath);
                var configObject = UnityEngine.Object.Instantiate(configAsset);
                _configurations.Add(configType, configObject);
            }
        }

        public Configuration GetConfiguration(Type type)
        {
            if (_configurations.TryGetValue(type, out var result))
                return result;

            throw new Exception(
                $"Failed to provide `{type.Name}` configuration object: Requested configuration type not found in project resources.");
        }

        public static T LoadOrDefault<T>(string resourcesPath = DefaultResourcesPath)
            where T : Configuration
        {
            return (T) LoadOrDefault(typeof(T), resourcesPath);
        }
        
        public static Configuration LoadOrDefault(Type type, string resourcesPath = DefaultResourcesPath)
        {
            var resourcePath = $"{resourcesPath}/{type.Name}";
            var configAsset = UnityEngine.Resources.Load(resourcePath, type) as Configuration;

            if (!configAsset)
            {
                var createdPath = $"{DefaultCreatedPath}/{type.Name}";
                configAsset = ScriptableObject.CreateInstance(type) as Configuration;
                
                #if UNITY_EDITOR
                AssetDatabase.CreateAsset(configAsset, createdPath + ".asset");
                AssetDatabase.SaveAssets();
                #endif
            }
            
            return configAsset;
        }
    }
}
