using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class Entity : SerializedMonoBehaviour, IUIEntity
    {
        [BoxGroup("Components"), SerializeField, Required]
        protected RectTransform _rectTransform;

        public GameObject GameObject => this.gameObject;

        public RectTransform RectTransform => this._rectTransform;

        protected virtual void Awake()
        {
            this._rectTransform = this.transform.GetComponent<RectTransform>();
            this._rectTransform.anchoredPosition3D = new (this._rectTransform.anchoredPosition.x, this._rectTransform.anchoredPosition.y, 0);
        }
    }
}