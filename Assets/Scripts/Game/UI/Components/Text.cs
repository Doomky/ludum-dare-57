using DG.Tweening;
using Framework;
using Framework.Helpers;
using Framework.Managers;
using Game.Managers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SFXManager = Game.Managers.SFXManager;
using TMPText = TMPro.TextMeshProUGUI;

namespace Game.UI
{
    public class Text : ScreenUIEntity
    {
        [BoxGroup("Components"), Required]
        [SerializeField]
        private TMPText _text = null;

        [BoxGroup("Components"), Required]
        [SerializeField]
        private TMPText _shadow = null;

        [BoxGroup("Data"), InlineEditor, Required]
        [SerializeField]
        private TextConfiguration _configuration = null;

        [ShowInInspector, HideInEditorMode]
        private AudioClip _characterOverrideSFX;

        private Timer _fadeCharacterSFXIntervallTimer = new(0);

        private SFXManager _sfxManager = null;

        private DOTweenTMPAnimator _textAnimator;
        private DOTweenTMPAnimator _shadowAnimator;

        [ShowInInspector, HideInEditorMode]
        private bool _isFadePunched = false;

        private Sequence _fadePunchSequence;
        private Sequence _rainbowSequence;
        private DOTweenTMPAnimator _rainbowTextAnimator;
        private DOTweenTMPAnimator _rainbowShadowAnimator;

        public bool IsFadePunched => this._isFadePunched;

        public AudioClip CharacterOverrideSFX
        {
            get => this._characterOverrideSFX;
            set => this._characterOverrideSFX = value;
        }

        protected override void Awake()
        {
            base.Awake();

            this._textAnimator = new DOTweenTMPAnimator(this._text);
            this._shadowAnimator = new DOTweenTMPAnimator(this._shadow);

            bool isLeftAligned = this._shadow.horizontalAlignment == HorizontalAlignmentOptions.Left;
            this._shadow.rectTransform.anchoredPosition = ((isLeftAligned ? (this._text.fontSize / 24f) : (this._text.fontSize / 16f)) - 0.125f) * Vector2.down;

            this._sfxManager = SuperManager.Get<SFXManager>();
            this._fadeCharacterSFXIntervallTimer.Duration = this._configuration.CharaterSFXFadeIntervall;

            this.SetText(new TextData(this._text.text));

            if (this._configuration.IsColored)
            {
                this._text.color = this._configuration.Color.Color;
                this._shadow.color = Managers.ColorHelpers.GetShadowColor(this._configuration.Color.Color);
            }
        }

        public IEnumerator WaitForPunchText(TextData textData)
        {
            this.SetText(textData);

            // TODO: fix this shit...

            yield return new WaitForEndOfFrame();

            this._text.color = this._text.color.SetAlpha(0);
            this._text.gameObject.SetActive(true);
            this._text.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            this._shadow.color = this._shadow.color.SetAlpha(0);
            this._shadow.gameObject.SetActive(true);
            this._shadow.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            yield return new WaitForEndOfFrame();

            this.FadePunch();

            while (this._isFadePunched)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public void SetText(string str)
        {
            this._text.SetText(str);
            this._text.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            this._shadow.SetText(str);
            this._shadow.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            if (!string.IsNullOrEmpty(str))
            {
                if (this._configuration?.IsShownAsFadePunch ?? false)
                {
                    this.UpdateFadePunchSequence();
                    this._text.gameObject.SetActive(false);
                    this._shadow.gameObject.SetActive(false);
                }
            }
        }

        public void SetText(TextData textData)
        {
            this._text.SetText(textData.Text);
            this._text.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            this._shadow.SetText(textData.Shadow);
            this._shadow.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            if (this._configuration?.IsShownAsFadePunch ?? false)
            {
                this.UpdateFadePunchSequence();
                this._text.gameObject.SetActive(false);
                this._shadow.gameObject.SetActive(false);
            }
        }

        public void SetConfiguration(TextConfiguration configuration)
        {
            this._configuration = configuration;

            if (this._configuration.IsColored)
            {
                this._text.color = this._configuration.Color.Color;
                this._shadow.color = Managers.ColorHelpers.GetShadowColor(this._configuration.Color.Color);
            }
        }

        protected void Update()
        {
            ////if (this._configuration?.IsWobbly ?? false)
            ////{
            ////    this.UpdateWobbly(this._text);
            ////    this.UpdateWobbly(this._shadow);
            ////}
        }

        private void UpdateWobbly(TMPText text)
        {
            text.ForceMeshUpdate();

            TMP_TextInfo textInfo = text.textInfo;

            int wobblyCharcterIndex = 0;

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                ref TMP_CharacterInfo charInfo = ref textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                ref TMP_MeshInfo meshInfo = ref textInfo.meshInfo[charInfo.materialReferenceIndex];

                Vector3[] vertices = meshInfo.vertices;

                for (int j = 0; j < 4; j++)
                {
                    int verticeIndex = charInfo.vertexIndex + j;

                    Vector3 orig = vertices[verticeIndex];

                    float time = Time.time - wobblyCharcterIndex * this._configuration.WobblyTimeOffsetPerIndex;

                    float yOffset = this._configuration.WobblyYOffsetCurve.Evaluate((time / this._configuration.WobblyCurveDuration) % 1);

                    vertices[verticeIndex] = orig + new Vector3(0, yOffset, 0);
                }

                wobblyCharcterIndex++;
            }

            TMP_MeshInfo[] meshInfos = textInfo.meshInfo;
            int meshInfosCount = meshInfos.Length;
            for (int i = 0; i < meshInfosCount; ++i)
            {
                ref TMP_CharacterInfo charInfo = ref textInfo.characterInfo[i];

                ref TMP_MeshInfo meshInfo = ref textInfo.meshInfo[charInfo.materialReferenceIndex];

                Mesh mesh = meshInfo.mesh;

                mesh.vertices = meshInfo.vertices;

                text.UpdateGeometry(mesh, i);
            }
        }

        [BoxGroup("Debug"), Button]
        public void FadePunch()
        {
            this._isFadePunched = true;

            this._text.color = this._text.color.SetAlpha(1);
            this._text.gameObject.SetActive(true);
            this._text.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            this._shadow.color = this._shadow.color.SetAlpha(1);
            this._shadow.gameObject.SetActive(true);
            this._shadow.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);

            this._fadePunchSequence.Goto(0, true);
        }

        public void StopFadePunch()
        {
            if (this._isFadePunched)
            {
                this._fadePunchSequence.Pause();
                this._isFadePunched = false;
            }
        }

        [BoxGroup("Debug"), Button]
        private void UpdateFadePunchSequence()
        {
            this._fadePunchSequence = DOTween.Sequence();
            this._fadePunchSequence.SetAutoKill(false);

            TMP_TextInfo textInfo = this._textAnimator.textInfo;

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                if (textInfo.characterInfo[i].character == ' ' || textInfo.characterInfo[i].character == '\n')
                {
                    continue;
                }

                this._fadePunchSequence.Append(this._textAnimator.DOFadeChar(i, 0, 0));
                this._fadePunchSequence.Append(this._shadowAnimator.DOFadeChar(i, 0, 0));
            }

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                if (textInfo.characterInfo[i].character == ' ' || textInfo.characterInfo[i].character == '\n')
                {
                    continue;
                }

                float duration = this._configuration.FadePunchDuration;
                Vector3 punch = this._configuration.Punch;

                this._fadePunchSequence.Append(this._textAnimator.DOPunchCharOffset(i, punch, duration));
                this._fadePunchSequence.Join(this._textAnimator.DOFadeChar(i, 1, duration));
                this._fadePunchSequence.Join(this._shadowAnimator.DOPunchCharOffset(i, punch, duration));
                this._fadePunchSequence.Join(this._shadowAnimator.DOFadeChar(i, 1, duration));
                this._fadePunchSequence.JoinCallback(this.PlayCharacterFadeSFX);
            }

            this._fadePunchSequence.AppendCallback(() => this._isFadePunched = false);
            this._fadePunchSequence.Rewind();
        }

        [BoxGroup("Debug"), Button]
        public void RainbowWave()
        {
            if (this._rainbowSequence != null)
            {
                this._rainbowSequence.Kill();
                this._rainbowSequence = null;
            }

            this._rainbowSequence = DOTween.Sequence();
            this._rainbowSequence.SetAutoKill(false);
            this._rainbowTextAnimator = new DOTweenTMPAnimator(this._text);
            this._rainbowShadowAnimator = new DOTweenTMPAnimator(this._shadow);

            TMP_TextInfo textInfo = this._rainbowTextAnimator.textInfo;

            List<int> visibileCharacters = new();

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
                if (!tMP_CharacterInfo.isVisible)
                {
                    continue;
                }

                visibileCharacters.Add(i);
            }

            int waveStepCount = 40;
            float waveDuration = 2.5f;
            float waveAmplitude = 4f;

            float stepDuration = waveDuration / waveStepCount;

            {
                for (int j = 0; j < visibileCharacters.Count; ++j)
                {
                    int charIndex = visibileCharacters[j];

                    float normalizedVisibileCharacterIndex = j / (visibileCharacters.Count - 1f);

                    float timeOffset = normalizedVisibileCharacterIndex % 1;
                    float h = (2f * normalizedVisibileCharacterIndex) % 1;

                    Color color = Color.HSVToRGB(h, 0.6f, 1);
                    Color shadowColor = color.GetShadowColor();

                    float charOffsetAmp = waveAmplitude * Mathf.Cos(2f * Mathf.PI * timeOffset);

                    Vector3 charOffset =  charOffsetAmp * Vector3.up;

                    this._rainbowSequence.Append(this._rainbowTextAnimator.DOOffsetChar(charIndex, charOffset, 0));
                    this._rainbowSequence.Join(this._rainbowShadowAnimator.DOOffsetChar(charIndex, charOffset, 0));

                    this._rainbowSequence.Join(this._rainbowTextAnimator.DOColorChar(charIndex, color, 0));
                    this._rainbowSequence.Join(this._rainbowShadowAnimator.DOColorChar(charIndex, shadowColor, 0));
                }
            }

            for (int step = 0; step < waveStepCount; step++)
            {
                // step 0 == step ^1
                float stepOffset = step / (waveStepCount - 1f);

                this._rainbowSequence.AppendInterval(stepDuration);

                for (int j = 0; j < visibileCharacters.Count; ++j)
                {
                    int charIndex = visibileCharacters[j];

                    float normalizedVisibileCharacterIndex = j / (visibileCharacters.Count - 1f);

                    float timeOffset = (normalizedVisibileCharacterIndex + stepOffset) % 1;
                    float h = (2f * (normalizedVisibileCharacterIndex + stepOffset)) % 1;

                    Color color = Color.HSVToRGB(h, 0.75f, 1);
                    Color shadowColor = color.GetShadowColor();

                    float charOffsetAmp = waveAmplitude * Mathf.Cos(2f * Mathf.PI * timeOffset);

                    Vector3 charOffset = charOffsetAmp * Vector3.up;

                    this._rainbowSequence.Join(this._rainbowTextAnimator.DOOffsetChar(charIndex, charOffset, stepDuration / 2));
                    this._rainbowSequence.Join(this._rainbowShadowAnimator.DOOffsetChar(charIndex, charOffset, stepDuration / 2));

                    this._rainbowSequence.Join(this._rainbowTextAnimator.DOColorChar(charIndex, color, 0));
                    this._rainbowSequence.Join(this._rainbowShadowAnimator.DOColorChar(charIndex, shadowColor, 0));
                }
            }

            this._rainbowSequence.SetLoops(-1);
            this._rainbowSequence.Play();
        }

        public IEnumerator WaitForCounter(int startingValue, int EndValue, AnimationCurve animationCurve, float duration)
        {
            float elapsedTime = 0;

            float startingPitch = 1.2f;
            float endPitch = 1f;

            Timer sfxTimer = new(0.05f);
            sfxTimer.Trigger();

            int previousValue = startingValue;

            Tweener tweener = null;
            while (elapsedTime < duration)
            {
                float progress = animationCurve.Evaluate(elapsedTime / duration);

                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startingValue, EndValue, progress));
                int valueDelta = currentValue - previousValue;

                // Value has changed
                if (valueDelta != 0)
                {
                    TextData currentValueText = new(currentValue.ToString());

                    this.SetText(currentValueText);

                    if (tweener == null)
                    {
                        this._rectTransform.localScale = Vector3.one;
                        tweener = this._rectTransform.DOPunchScale(0.1f * Vector3.one, 0.1f, 0, 0);
                        tweener.OnComplete(() => tweener = null);
                    }

                    float normalizedValueDelta = valueDelta / (startingValue - EndValue);

                    if (sfxTimer.IsTriggered())
                    {
                        float pitch = Mathf.Lerp(startingPitch, endPitch, normalizedValueDelta);
                        //this._sfxManager.PlayGlobalSFX(SFXKey.Pop, pitch);
                        sfxTimer.Reset();
                    }
                }

                yield return new WaitForFixedUpdate();
                elapsedTime += Time.fixedDeltaTime;

                previousValue = currentValue;
            }
        }

        private void PlayCharacterFadeSFX()
        {
            if (this._fadeCharacterSFXIntervallTimer.IsTriggered())
            {
                AudioClip characterSFX = this._characterOverrideSFX != null ? this._characterOverrideSFX : this._configuration.CharacterFadeSFX;

                this._sfxManager.PlayGlobalSFX(characterSFX, minPitch: this._configuration.MinPitch, maxPitch: this._configuration.MaxPitch);
                this._fadeCharacterSFXIntervallTimer.Duration = this._configuration.CharaterSFXFadeIntervall;
                this._fadeCharacterSFXIntervallTimer.Reset();
            }
        }
    }
}
