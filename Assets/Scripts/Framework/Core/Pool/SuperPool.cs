using Framework.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class SuperPool<TComponent> where TComponent : Component
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        private bool _byPass = false;

        public bool ByPass
        {
            get => this._byPass;
            set
            {
                this._byPass = value;

                foreach (Pool<TComponent> pool in this._poolsByPrefab.Values)
                {
                    pool.IgnoreFreeInstances = value;
                }
            }
        }
#endif

        // TOOD: replace dictionary with a list.
        [ShowInInspector, HideInEditorMode]
        private readonly Dictionary<PrefabReference<TComponent>, Pool<TComponent>> _poolsByPrefab = new();

        [ShowInInspector, HideInEditorMode]
        private readonly Dictionary<TComponent, PrefabReference<TComponent>> _prefabByInstance = new();

        public void Allocate(PrefabReference<TComponent> prefabReference, Vector2 position, Transform parent, ref TComponent component)
        {
            if (!this._poolsByPrefab.TryGetValue(prefabReference, out Pool<TComponent> pool))
            {
#if UNITY_EDITOR
                pool = new Pool<TComponent>(prefabReference, this._byPass);
#else
                pool = new Pool<TComponent>(prefabReference);
#endif
            }

            component = pool.Allocate(position, parent);

            this._prefabByInstance.Add(component, prefabReference);
        }

        public void Allocate(PrefabReference<TComponent> prefabReference, Vector2 position, Quaternion rotation, Transform parent, ref TComponent component)
        {
            if (!this._poolsByPrefab.TryGetValue(prefabReference, out Pool<TComponent> pool))
            {
#if UNITY_EDITOR
                pool = new Pool<TComponent>(prefabReference, this._byPass);
#else
                pool = new Pool<TComponent>(prefabReference);
#endif
            }

            component = pool.Allocate(position, rotation, parent);

            this._prefabByInstance.Add(component, prefabReference);
        }

        public void Release(ref TComponent component, Transform parent)
        {
#if UNITY_EDITOR
            if (this._byPass)
            {
                GameObject.Destroy(component.gameObject);
                component = null;
                return;
            }
#endif

            PrefabReference<TComponent> prefab = this._prefabByInstance[component];

            if (!this._poolsByPrefab.TryGetValue(prefab, out Pool<TComponent> pool))
            {
                pool = new Pool<TComponent>(prefab);
            }

            pool.Release(component, parent);
        }
    }
}
