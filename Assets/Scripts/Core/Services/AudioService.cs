using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace FootBallNet
{
    public class AudioService : Service<AudioConfiguration>
    {
        private List<AudioSource> _sources = new();
        private Transform _container;

        public override Task InitializeServiceAsync()
        {
            _container = Engine.GetService<InputService>().Platform == ETargetPlatform.VR
                ? Engine.CreateObject("AudioController").transform
                : Engine.CreateObject("AudioController", null, typeof(AudioListener)).transform;

            for (var i = 0; i < Configuration.StartPoolElements; i++)
                CreateSource();

            return Task.CompletedTask;
        }

        public void Play2DSound(ESound sound, bool loop = false, float volume = 1f)
        {
            var clip = Configuration.GetRandomSound(sound);

            if (clip is null)
            {
                Debug.LogError($"Sound {Enum.GetName(typeof(ESound), sound)} doesn't found.");
                return;
            }

            if (loop)
            {
                var audioSource = GetAudioSource(sound);
                audioSource.SetSettings(clip, loop, 0f, volume);
                audioSource.Play();
            }
            else
            {
                _sources[0].PlayOneShot(clip);
            }
        }

        public void Play3DSound(ESound sound, Vector3 position, bool loop = false, float volume = 1f)
        {
            var clip = Configuration.GetRandomSound(sound);

            if (clip is null)
            {
                Debug.LogError($"Sound {Enum.GetName(typeof(ESound), sound)} doesn't found.");
                return;
            }

            if (loop)
            {
                var audioSource = GetAudioSource(sound);

                audioSource.SetSettings(clip, loop, 1f, volume);
                audioSource.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, position);
            }
        }

        private AudioSource GetAudioSource(ESound sound)
        {
            var free = _sources.Where(x => x.isPlaying == false).ToList();

            if (free.Count == 0)
                return CreateSource();

            return free[0];
        }

        private AudioSource CreateSource()
        {
            var source = Engine.CreateObject("AudioSource", _container, typeof(AudioSource))
                .GetComponent<AudioSource>();
            _sources.Add(source);

            return source;
        }

        public override void ResetService()
        {
        }

        public override void DestroyService()
        {
        }
    }
}