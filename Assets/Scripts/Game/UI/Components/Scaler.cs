using DG.Tweening;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class Scaler : ScreenUIEntity, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private float _hoverScale = 1.1f;

        [SerializeField]
        private float _transitionDuration = 0.2f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._rectTransform.DOScale(this._hoverScale, this._transitionDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._rectTransform.DOScale(Vector2.one, this._transitionDuration);
        }
    }
}
