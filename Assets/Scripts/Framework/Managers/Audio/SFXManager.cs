using Framework.Audio;
using Framework.Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework.Managers
{
    public abstract class SFXManager<TDefinition, TSFXKeyEnum> : SFXManager<TDefinition> where TSFXKeyEnum : Enum
        where TDefinition : SFXManagerDefinition<TSFXKeyEnum>
    {
        public AudioSource PlayGlobalSFX(TSFXKeyEnum key, float pitch)
        {
            if (!this._definition.SFXsByKey.TryGetValue(key, out AudioClip sfx))
            {
                DebugHelper.LogError(this, $"{key} was not found in the configuration.");
            }

            return this.PlayGlobalSFX(sfx, pitch);
        }

        public AudioSource PlayGlobalSFX(TSFXKeyEnum key, float minPitch = 1, float maxPitch = 1)
        {
            if (!this._definition.SFXsByKey.TryGetValue(key, out AudioClip sfx))
            {
                DebugHelper.LogError(this, $"{key} was not found in the configuration.");
            }

            return this.PlayGlobalSFX(sfx, minPitch: minPitch, maxPitch: maxPitch);
        }
    }

    public abstract partial class SFXManager<TDefinition> : Manager<TDefinition, SFXManager_PersitentData>
        where TDefinition : SFXManagerDefinition
    {
        [ShowInInspector, HideInEditorMode] 
        protected bool _isMuted = false;

        [ShowInInspector, HideInEditorMode]
        [ReadOnly]
        protected List<AudioSource> _audioSources = new();

        private TimeManager _timeManager = null;

        public bool IsMuted => this._isMuted;
        
        public float Volume => this._isMuted ? 0f : this._definition.PersistentData.Volume;

        public void UpdatePitch()
        {
            int count = this._audioSources?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                this._audioSources[i].pitch = this._timeManager.TimeScale;
            }
        }

        protected AudioSource GetSFXAudioSource(Transform transform, Vector2 position, bool isSpatialized = false, bool isLooping = false, float pitch = 1)
        {
            AudioSource sfxSource;

            sfxSource = new GameObject("SFX").AddComponent<AudioSource>();

            sfxSource.loop = isLooping;
            sfxSource.playOnAwake = false;
            sfxSource.volume = this.Volume;

            sfxSource.spatialize = isSpatialized;
            sfxSource.spatialBlend = isSpatialized ? 1 : 0;

            sfxSource.transform.parent = transform ? transform : this.transform;
            sfxSource.transform.position = position;
            sfxSource.pitch = pitch * (isSpatialized ? this._timeManager.TimeScale : 1);

            sfxSource.Configure(this._definition.AudioSourceConfiguration);

            this._audioSources.Add(sfxSource);

            return sfxSource;
        }

        protected void UpdateAllSFXVolume()
        {
            float volume = this.Volume;

            int audioSourceCount = this._audioSources?.Count ?? 0;
            for (int i = 0; i < audioSourceCount; i++)
            {
                this._audioSources[i].volume = volume;
            }
        }

        public void MuteImmediate()
        {
            if (!this._isMuted)
            {
                this._isMuted = true;
                this.UpdateAllSFXVolume();
            }
        }

        public void UnmuteImmediate()
        {
            if (this._isMuted)
            {
                this._isMuted = false;
                this.UpdateAllSFXVolume();
            }
        }

        public void SetSFXVolume(float newVolume)
        {
            this._definition.PersistentData.Volume = Mathf.Clamp01(newVolume);
            this.UpdateAllSFXVolume();
        }

        public AudioSource PlayGlobalSFX(AudioClip sfxClip)
        {
            return this.PlayGlobalSFX(sfxClip, false, 1, 1);
        }

        public AudioSource PlayGlobalSFX(AudioClip sfxClip, bool isLooping = false, float minPitch = 1, float maxPitch = 1)
        {
            if (sfxClip == null)
            {
                return null;
            }

            float pitch = Random.Range(minPitch, maxPitch);

            AudioSource source = this.GetSFXAudioSource(null, Vector2.zero, isLooping: isLooping, pitch: pitch);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public AudioSource PlayGlobalSFX(AudioClip sfxClip, bool isLooping = false, bool isPitchRandomized = false)
        {
            return this.PlayGlobalSFX(sfxClip, isLooping, isPitchRandomized ? 0.8f : 1, isPitchRandomized ? 1.2f : 1);
        }

        public AudioSource PlayGlobalSFX(AudioClip sfxClip, float pitch, bool isLooping = false)
        {
            if (sfxClip == null)
            {
                return null;
            }

            AudioSource source = this.GetSFXAudioSource(null, Vector2.zero, isSpatialized: false, isLooping, pitch);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }
        
        public void StopSFX(AudioSource audioSource)
        {
            this.RemoveSFXSource(audioSource);
        }

        public AudioSource PlayLocalSFX(AudioClip sfxClip, Transform transform, bool isLooping = false, bool isPitchRandomized = false, float minPitch = 0.8f, float maxPitch = 1.2f)
        {
            if (sfxClip == null)
            {
                return null;
            }

            float pitch = isPitchRandomized ? Random.Range(minPitch, maxPitch) : 1;

            AudioSource source = this.GetSFXAudioSource(
                transform, 
                transform.position, 
                isSpatialized: true, 
                isLooping: isLooping, 
                pitch: pitch);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public AudioSource PlayLocalSFX(AudioClip sfxClip, Vector2 position, bool isLooping = false, bool isPitchRandomized = false)
        {
            if (sfxClip == null)
            {
                return null;
            }

            AudioSource source = this.GetSFXAudioSource(null, position, isLooping, pitch: isPitchRandomized ? Random.Range(0.85f, 1.15f) : 1);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public void PlaySFXFixedDuration(AudioClip sfxClip, float duration, Transform transform)
        {
            if (sfxClip == null)
            {
                return;
            }

            AudioSource source = this.GetSFXAudioSource(transform, transform != null ? transform.position : Vector2.zero);
            source.clip = sfxClip;
            source.loop = true;
            source.Play();

            this.StartCoroutine(this.RemoveSFXSourceFixedLength(source, duration));
        }

        protected void RemoveSFXSource(AudioSource sfxSource)
        {
            this._audioSources.Remove(sfxSource);

            GameObject.Destroy(sfxSource.gameObject);
        }

        protected IEnumerator RemoveSFXSourceAtEnd(AudioSource sfxSource)
        {
            yield return new WaitForSeconds(sfxSource.clip.length);

            this.RemoveSFXSource(sfxSource);
        }

        protected IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length)
        {
            yield return new WaitForSeconds(length);

            this._audioSources.Remove(sfxSource);
            GameObject.Destroy(sfxSource.gameObject);
        }

        public override void PostLayerLoad()
        {
            base.PostLayerLoad();

            SuperManager.TryGet(out this._timeManager);
            this._timeManager.TimeScaleChanged += this.TimeManager_TimeScaleChanged;
        }

        public override void PreLayerUnload()
        {
            this._timeManager.TimeScaleChanged -= this.TimeManager_TimeScaleChanged;
            this._timeManager = null;

            base.PreLayerUnload();
        }

        private void TimeManager_TimeScaleChanged(TimeManager arg1, float timescale)
        {
            this.UpdatePitch();
        }
    }
}