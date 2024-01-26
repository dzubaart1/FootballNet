using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FootBallNet
{
    public enum ESound
    {
        WeaponSound,
        DroneExplosionSound,
        DroneHitSound
    }
    
    public class AudioConfiguration : Configuration
    {
        public int StartPoolElements = 5;
        public List<AudioSettings> Settings;

        public AudioClip GetFirstSound(ESound sound)
        {
            var clip = Settings.FirstOrDefault(x => x.Sound == sound);
            return clip?.Audio;
        }
        
        public AudioClip GetRandomSound(ESound sound)
        {
            var clips = Settings.Where(x => x.Sound == sound).ToList();
            
            if (clips.Count > 0)
                return clips[Random.Range(0, clips.Count)].Audio;

            return null;
        }
    }

    [Serializable]
    public class AudioSettings
    {
        public ESound Sound;
        public AudioClip Audio;
    }
}
