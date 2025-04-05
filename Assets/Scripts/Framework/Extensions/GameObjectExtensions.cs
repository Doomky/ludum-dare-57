using UnityEngine;

namespace Framework.Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}