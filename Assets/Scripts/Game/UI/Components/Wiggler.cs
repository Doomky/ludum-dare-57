using Framework.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class Wiggler : ScreenUIEntity, IPointerEnterHandler, IPointerExitHandler
    {
        [BoxGroup("Data"), InlineEditor]
        [SerializeField]
        private WigglerConfiguration _configuration = null;
        
        [BoxGroup("Data")]
        [SerializeField]
        private int _timeOffset;

        [BoxGroup("Data"), ShowInInspector, HideInEditorMode]
        private bool _isHovered;

        public int TimeOffset
        {
            get => this._timeOffset;
            set => this._timeOffset = value;
        }

        private void Update()
        {
            float time = this._configuration.TimeMultiplier * Time.time + TimeOffset;

            Vector2 desiredPositionOffset = new(this._configuration.XMagnitude, this._configuration.YMagnitude);

            if (this._isHovered)
            {
                desiredPositionOffset *= this._configuration.HoveredMagnitudeMultiplier;
            }

            desiredPositionOffset *= Mathf.Cos(time);

            if (this._rectTransform.anchorMin == Vector2.up && this._rectTransform.anchorMax == Vector2.up && this._rectTransform.pivot == 0.5f * Vector2.one)
            {
                desiredPositionOffset += this._rectTransform.sizeDelta * new Vector2(0.5f, -0.5f);
            }

            this._rectTransform.anchoredPosition = Vector2.Lerp(this._rectTransform.anchoredPosition, desiredPositionOffset, 0.25f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._isHovered = false;
        }
    }
}
