using Framework.Helpers;
using Framework.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Framework.Core
{
    public partial class PrefabReference
    {
#if UNITY_EDITOR
        [HideLabel]
        [HorizontalGroup("Main")]
        [ValueDropdown(nameof(GetAllPrefabs), AppendNextDrawer = true, DisableGUIInAppendedDrawer = true, DropdownWidth = 300)]
        [PropertyOrder(0)]
#endif
        [SerializeField]
        private GameObject _prefab;

        public GameObject Prefab => this._prefab;

        public PrefabReference()
        {

        }

        public PrefabReference(GameObject prefabGameObject)
        {
            this._prefab = prefabGameObject;
        }

        public bool HasPrefab()
        {
            return this._prefab != null;
        }

        public GameObject InstantiateAtWorldPosition(Vector2 position, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtWorldPosition(this._prefab, position, Quaternion.identity, parent);
        }

        public GameObject InstantiateAtWorldPosition(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtWorldPosition(this._prefab, position, rotation, parent);
        }

        public GameObject InstantiateAtLocalPosition(Vector2 position, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtLocalPosition(this._prefab, position, Quaternion.identity, parent);
        }

        public GameObject Instantiate(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            if (this._prefab == null)
            {
                return default;
            }

            GameObject gameObject = GameObject.Instantiate(this._prefab, position, rotation, parent);
            gameObject.transform.position = position;

            return gameObject;
        }

        public static implicit operator bool(PrefabReference prefabReference) => prefabReference._prefab != null;
    }

    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public partial class PrefabReference<T>
    {
#if UNITY_EDITOR
        [HideLabel]
        [HorizontalGroup("Main")]
        [ValueDropdown(nameof(GetAllPrefabs), AppendNextDrawer = true, DisableGUIInAppendedDrawer = true, DropdownWidth = 300)]
        [PropertyOrder(0)]
#endif
        [SerializeField]
        private GameObject _prefab;

        public GameObject Prefab => this._prefab;

        public T PrefabT => this._prefab.GetComponent<T>();

        public PrefabReference()
        {

        }

        public PrefabReference(T t)
        {
            Component component = t as Component;
            this._prefab = component.gameObject;
        }

        public PrefabReference(GameObject prefabGameObject)
        {
            this._prefab = prefabGameObject;
        }

        public bool HasPrefab()
        {
            return this._prefab != null;
        }

        public T InstantiateAtWorldPosition(Vector2 position, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtWorldPosition<T>(this._prefab, position, Quaternion.identity, parent);
        }

        public T InstantiateAtWorldPosition(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtWorldPosition<T>(this._prefab, position, rotation, parent);
        }

        public T InstantiateAtLocalPosition(Vector2 position, Transform parent = null)
        {
            return PrefabHelpers.InstantiateAtLocalPosition<T>(this._prefab, position, Quaternion.identity, parent);
        }

        public T InstantiateUI(Transform parent = null)
        {
            IUIEntity uiEntity = (IUIEntity)this.PrefabT;

            return (T)PrefabHelpers.InstantiateUI<IUIEntity>(this._prefab, uiEntity.RectTransform.anchoredPosition, parent);
        }

        public T Instantiate(Transform parent = null)
        {
            return this.Instantiate(Vector2.zero, Quaternion.identity, parent);
        }

        public T Instantiate(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            if (this._prefab == null)
            {
                return default;
            }

            GameObject gameObject = GameObject.Instantiate(this._prefab, position, rotation, parent);
            gameObject.transform.position = position;

            return gameObject.GetComponent<T>();
        }

        public static implicit operator bool(PrefabReference<T> prefabReference) => prefabReference._prefab != null;
    }
}
