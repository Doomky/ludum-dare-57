using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Entities
{
    public partial class Player
    {
        [TitleGroup("Fire")]
        [SerializeField]
        private Timer _oxygenTimer = new(20);

        [TitleGroup("Fire")]
        [SerializeField]
        private Slider _oxygenSlider = null;

        public bool NeedOxygen()
        {
            return this._oxygenTimer.ProgressRatioLeft() < 0.75f;
        }

        private void FixedUpdate_Oxygen()
        {
            if (this._oxygenTimer.IsTriggered())
            {
                this.Die();
                return;
            }

            this._oxygenSlider.value = this._oxygenTimer.ProgressRatioLeft();
        }

        public void RefillOxygen()
        {
            this._oxygenTimer.SetProgressRatio(this._oxygenTimer.ProgressRatio() - 0.4f);
        }
    }
}   