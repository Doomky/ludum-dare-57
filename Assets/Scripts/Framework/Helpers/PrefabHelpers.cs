using Framework.UI;
using UnityEngine;

namespace Framework.Helpers
{
    public static class PrefabHelpers
    {
        public static T InstantiateAtWorldPosition<T>(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent = null) 
        {
            GameObject gameObject = GameObject.Instantiate(prefab, position, rotation, parent);
            gameObject.transform.position = position;
            gameObject.name = prefab.name;
            return gameObject.GetComponent<T>();
        }

        public static T InstantiateAtLocalPosition<T>(GameObject prefab, Vector2 localPosition, Quaternion rotation, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, Vector3.zero, rotation, parent);
            gameObject.transform.localPosition = localPosition;
            gameObject.name = prefab.name;
            return gameObject.GetComponent<T>();
        }

        public static GameObject InstantiateAtWorldPosition(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, position, rotation, parent);
            gameObject.transform.position = position;
            gameObject.name = prefab.name;
            return gameObject;
        }

        public static GameObject InstantiateAtLocalPosition(GameObject prefab, Vector2 localPosition, Quaternion rotation, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, Vector3.zero, rotation, parent);
            gameObject.transform.localPosition = localPosition;
            gameObject.name = prefab.name;
            return gameObject;
        }

        public static GameObject Instantiate(GameObject prefab, Vector2 position, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, parent);
            gameObject.transform.position = position;
            gameObject.name = prefab.name;
            return gameObject;
        }

        public static GameObject Instantiate(GameObject prefab, Vector2 position = default, Quaternion rotation = default, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, position, rotation, parent);
            return gameObject;
        }

        public static TUIEntity InstantiateUI<TUIEntity>(GameObject prefab, Vector2 anchoredPosition, Transform parent = null) where TUIEntity : IUIEntity
        {
            GameObject gameObject = GameObject.Instantiate(prefab, parent);
            TUIEntity uiEntity = gameObject.GetComponent<TUIEntity>();

            uiEntity.RectTransform.SetParent(parent);
            uiEntity.RectTransform.anchoredPosition = anchoredPosition;
            
            return uiEntity;
        }
    }
}