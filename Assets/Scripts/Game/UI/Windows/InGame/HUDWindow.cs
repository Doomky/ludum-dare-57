using Framework.Managers;
using Game.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Game.UI
{
    public class HUDWindow : GameWindow
    {
        [TitleGroup("Components")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _scoreText = null;

        [TitleGroup("Components")]
        [SerializeField]
        private Slider _comboTimerslider = null;

        [TitleGroup("Components")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _comboText = null;

        private int _currentScore = 0;

        private GameManager _gameManager = null;

        private Sequence _scoreTextSequence;
        private Sequence _comboTextSequence;

        protected override void Update()
        {
            base.Update();

            this._scoreText.text = $"{this._gameManager.Score}";

            bool isInCombo = this._gameManager.IsInCombo();

            this._comboTimerslider.gameObject.SetActive(isInCombo);
            if (isInCombo)
            {
                this._comboTimerslider.value = this._gameManager.ComboTimer.ProgressRatioLeft();
            }
        }

        public override void OnInGameEntered()
        {
            this._gameManager = SuperManager.Get<GameManager>();
            this._gameManager.ScoreChanged += this.OnScoreChanged;
            this._gameManager.ComboChanged += this.OnComboChanged;
        }

        public override void OnInGameExited()
        {
            this._gameManager.ComboChanged -= this.OnComboChanged;
            this._gameManager.ScoreChanged -= this.OnScoreChanged;
            this._gameManager = null;
        }

        protected override bool ShouldBeVisible()
        {
            return this._gameManager != null;
        }

        private void OnScoreChanged(int score)
        {
            this._scoreText.DOCounter(this._currentScore, this._gameManager.Score, 0.4f);
            this._currentScore = this._gameManager.Score;

            if (this._scoreTextSequence!= null)
            {
                this._scoreTextSequence.Kill();
                this._scoreTextSequence = null;
            }

            this._scoreText.rectTransform.localScale = Vector3.one;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(this._scoreText.rectTransform.DOShakePosition(0.2f, strength: 2f));
            sequence.Join(this._scoreText.rectTransform.DOPunchScale(0.5f * Vector2.one, 0.2f, elasticity: 0));
            sequence.OnKill(() => this._scoreTextSequence = null);

            this._scoreTextSequence = sequence;
        }

        private void OnComboChanged(int obj)
        {
            bool isInCombo = this._gameManager.IsInCombo();

            this._comboText.gameObject.SetActive(isInCombo);
            if (isInCombo)
            {
                if (this._comboTextSequence != null)
                {
                    this._comboTextSequence.Kill();
                    this._comboTextSequence = null;
                }

                Color comboColor = Framework.Helpers.ColorHelpers.ThreeLerp(Color.green, Color.yellow, Color.red, this._gameManager.Combo / 20f);

                this._comboText.text = $"combo x{this._gameManager.Combo}";
                this._comboText.color = comboColor;
                this._comboTimerslider.fillRect.GetComponent<Image>().color = comboColor;

                Sequence sequence = DOTween.Sequence();

                sequence.Append(this._comboText.rectTransform.DOShakePosition(0.4f, strength: 2f));
                sequence.Join(this._comboText.rectTransform.DOPunchScale(0.5f * Vector2.one, 0.2f, elasticity: 0));
                sequence.OnKill(() => this._comboTextSequence = null);

                this._comboTextSequence = sequence;
            }
        }
    }
}