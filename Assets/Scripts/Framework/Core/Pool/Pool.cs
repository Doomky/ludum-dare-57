using Framework.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class Pool<TComponent> where TComponent : Component
    {
        [SerializeField]
        private PrefabReference<TComponent> _prefabReference = null;

        [HideInEditorMode, ShowInInspector]
        private Queue<TComponent> _freeInstances = new();

#if UNITY_EDITOR
        [HideInEditorMode, ShowInInspector]
        private bool _ignoreFreeInstances = false;

        public bool IgnoreFreeInstances
        {
            get => this._ignoreFreeInstances;
            set
            {
                this._ignoreFreeInstances = value;
            }
        }
#endif
        public PrefabReference<TComponent> PrefabReference => this._prefabReference;

        private Action<TComponent> _onItemLoaded;
        private Action<TComponent> _onItemUnloaded;

        public Pool()
        {

        }

        public Pool(TComponent prefab)
        {
            this._prefabReference = new(prefab);
        }

#if UNITY_EDITOR
        public Pool(TComponent prefab, bool ignoreFreeInstance = false)
        {
            this._prefabReference = new(prefab);
            this._ignoreFreeInstances = ignoreFreeInstance;
        }
#endif

        public Pool(PrefabReference<TComponent> prefabReference)
        {
            this._prefabReference = prefabReference;
        }

#if UNITY_EDITOR
        public Pool(PrefabReference<TComponent> prefabReference, bool ignoreFreeInstance = false)
        {
            this._prefabReference = prefabReference; 
            this._ignoreFreeInstances = ignoreFreeInstance;
        }
#endif

        public void Load(Action<TComponent> onItemLoaded = null, Action<TComponent> onItemUnloaded = null)
        {
            this._onItemLoaded = onItemLoaded;
            this._onItemUnloaded = onItemUnloaded;
        }

        public void Unload()
        {
            int count = this._freeInstances.Count;
            for (int i = 0; i < count; i++)
            {
                this._onItemUnloaded.Invoke(this._freeInstances.Dequeue());
            }
        }
        
        public TComponent Allocate(Vector2 position, Quaternion rotation, Transform parent = null)
        {
            TComponent instance = null;

#if UNITY_EDITOR
            if (this._ignoreFreeInstances)
            {
                instance = this._prefabReference.InstantiateAtWorldPosition(position, rotation, parent);
                instance.gameObject.SetActive(true);
                return instance;
            }
#endif

            if (this._freeInstances?.Count > 0)
            {
                instance = this._freeInstances.Dequeue();
                instance.transform.parent = parent;
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                instance = this._prefabReference.InstantiateAtWorldPosition(position, rotation, parent);
                this._onItemLoaded?.Invoke(instance);
            }

            instance.gameObject.SetActive(true);

            return instance;
        }

        public TComponent Allocate(Vector2 position, Transform parent = null)
        {
            TComponent instance = null;

#if UNITY_EDITOR
            if (this._ignoreFreeInstances)
            {
                instance = this._prefabReference.InstantiateAtWorldPosition(position, parent);
                instance.gameObject.SetActive(true);
                return instance;
            }
#endif

            if (this._freeInstances?.Count > 0)
            {
                instance = this._freeInstances.Dequeue();
                instance.transform.SetParent(parent);
                instance.transform.position = position;
            }
            else
            {
                instance = this._prefabReference.InstantiateAtWorldPosition(position, parent);
                this._onItemLoaded?.Invoke(instance);
            }

            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Release(TComponent instance, Transform parent = null)
        {
#if UNITY_EDITOR
            if (this._ignoreFreeInstances)
            {
                GameObject.Destroy(instance);
                return;
            }
#endif
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(parent);

            this._freeInstances ??= new();
            this._freeInstances.Enqueue(instance);
        }

        public void Release(TComponent instance)
        {
#if UNITY_EDITOR
            if (this._ignoreFreeInstances)
            {
                GameObject.Destroy(instance);
                return;
            }
#endif
            instance.gameObject.SetActive(false);

            this._freeInstances ??= new();
            this._freeInstances.Enqueue(instance);
        }
    }
}
