using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Framework.Managers.Audio
{
    public abstract class BGMManager<TDefinition, TBGMKeyEnum> : BGMManager<TDefinition> 
        where TDefinition : BGMManagerDefinition<TBGMKeyEnum>
        where TBGMKeyEnum : Enum
    {
        public void PlayBGM(TBGMKeyEnum key, float fadeDuration = 0)
        {
            if (!this._definition.BGMsByKey.TryGetValue(key, out AudioClip bgm))
            {
                DebugHelper.LogError(this, $"{key} was not found in the configuration.");
            }

            this.PlayBGM(bgm, fadeDuration);
        }
    }

    public abstract class BGMManager<TDefinition> : Manager<TDefinition, BGMManager_PersistentData> 
        where TDefinition : BGMManagerDefinition
    {
        [SerializeField]
        private AudioSource _audioSource;
        
        [ShowInInspector, HideInEditorMode]        
        private bool _isMuted = false;

        public bool IsMuted => this._isMuted;

        public float Volume => this._isMuted ? 0 : this._definition.PersistentData.Volume;

        public bool IsPlaying => this._audioSource.isPlaying;

        public override void Load()
        {
            base.Load();

            this._audioSource = this.gameObject.AddComponent<AudioSource>();
            this._audioSource.loop = true;
        }

        public void Mute()
        {
            if (!this._isMuted)
            {
                this._isMuted = true;
                this.SetBGMVolume(0);
            }
        }

        public void Unmute()
        {
            if (this._isMuted)
            {
                this._isMuted = false;
                this.SetBGMVolume(this.Volume);
            }
        }

        public void SetBGMVolume(float newVolume)
        {
            this._definition.PersistentData.Volume = newVolume;
            this.UpdateSourceVolume();
        }

        public void PlayBGM(AudioClip bgmClip, float fadeDuration = 0)
        {
            this.StopAllCoroutines();

            if (fadeDuration > 0)
            {
                if (this._audioSource.isPlaying)
                {
                    this.StartCoroutine(this.FadeOutThenIn(bgmClip, fadeDuration/2, fadeDuration/2));
                }
                else
                {
                    this.StartCoroutine(this.FadeIn(bgmClip, delay: 0, fadeDuration));
                }
            }
            else
            {
                this._audioSource.Stop();
                this._audioSource.loop = true;
                this._audioSource.volume = this.Volume;
                this._audioSource.clip = bgmClip;
                this._audioSource.Play();
            }
        }
        
        public void StopBGM(float delay = 0, float fadeDuration = 0)
        {
            this.StopAllCoroutines();

            if (!this._audioSource.isPlaying)
            {
                return;
            }

            this.StartCoroutine(this.FadeOut(delay, fadeDuration)); 
        }

        protected IEnumerator FadeOutThenIn(AudioClip bgmClip, float fadeOutDuration, float fadeInDuration)
        {
            yield return this.FadeOut(0, fadeOutDuration);

            yield return this.FadeIn(bgmClip, 0, fadeInDuration);
        }

        protected IEnumerator FadeOut(float delay, float fadeDuration)
        {
            yield return this.FadeVolume(0, delay, fadeDuration);
        }

        protected IEnumerator FadeIn(AudioClip bgmClip, float delay, float fadeDuration)
        {
            if (this._audioSource.isPlaying)
            {
                this._audioSource.Stop();
            }

            this._audioSource.volume = 0;
            this._audioSource.clip = bgmClip;
            this._audioSource.Play();

            yield return this.FadeVolume(this.Volume, delay, fadeDuration);
        }

        protected IEnumerator FadeVolume(float toVolume, float delay, float fadeDuration)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            if (fadeDuration > 0)
            {
                float fromVolume = this._audioSource.volume;
                float elapsed = 0f;
                while (elapsed < fadeDuration)
                {
                    float volume = Mathf.Lerp(fromVolume, toVolume, elapsed / fadeDuration);
                    this._audioSource.volume = Mathf.Clamp01(volume);

                    yield return new WaitForFixedUpdate();
                    elapsed += Time.fixedDeltaTime;
                }
            }

            this._audioSource.volume = toVolume;
        }

        private void UpdateSourceVolume()
        {
            this._audioSource.volume = this.Volume;
        }
    }
}