using UnityEngine;

namespace FootBallNet
{
    public abstract class Configuration : ScriptableObject
    {
        public static T GetOrDefault<T>() where T : Configuration
        {
            if (Engine.IsInitialized) return Engine.GetConfiguration<T>();
            return ConfigurationProvider.LoadOrDefault<T>();
        }
    }
}
