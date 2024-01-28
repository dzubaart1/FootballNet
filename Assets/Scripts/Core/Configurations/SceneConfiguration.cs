using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FootBallNet
{
    public class SceneConfiguration : Configuration
    {
        public List<SceneSettings> Scenes;

        public SceneSettings GetSceneSettings(EScene scene)
        {
            return Scenes.FirstOrDefault(x => x.Scene == scene);
        }
    }

    [Serializable]
    public class SceneSettings
    {
        public EScene Scene;
        public Sprite Icon;
    }
}
