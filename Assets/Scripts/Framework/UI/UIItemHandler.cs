using Codice.CM.Common;
using Framework.Core;
using Game.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    [Serializable]
    public class UIItemHandler<TEntity, TUIItem> where TUIItem : Component, IUIItem<TEntity>
    {
        [SerializeField]
        private Transform _itemsParent;

        [SerializeField]
        [HideIf(nameof(IsInSampleMode))]
        private PrefabReference<TUIItem> _itemPrefab;

        [SerializeField]
        [HideIf(nameof(IsInPrefabMode))]
        private TUIItem _itemSample = null;

        [ShowInInspector]
        [HideInEditorMode]
        private List<TUIItem> _boundItems = null;

        [ShowInInspector]
        [HideInEditorMode]
        private Pool<TUIItem> _itemPool = null;

        public Transform ItemsParent => this._itemsParent;

        public List<TUIItem> BoundItems => this._boundItems;

        private Action<TUIItem> _onItemBound;
        private Action<TUIItem> _onItemUnbound;
        private Action<TUIItem> _onItemRefreshed;

        public bool IsInPrefabMode => this._itemPrefab != null && this._itemPrefab.HasPrefab();

        public bool IsInSampleMode => this._itemSample != null;

        public void Load(
            Action<TUIItem> onItemLoaded = null,       
            Action<TUIItem> onItemUnloaded = null,       
            Action<TUIItem> onItemBound = null, 
            Action<TUIItem> onItemRefreshed = null,
            Action<TUIItem> onItemUnbound = null)
        {
            this._onItemBound = onItemBound;
            this._onItemUnbound = onItemUnbound;
            this._onItemRefreshed = onItemRefreshed;

            this._boundItems = new();

            this._itemPool = this._itemPrefab != null ? new(this._itemPrefab) : new(this._itemSample);
            this._itemPool.Load(onItemLoaded, onItemUnloaded);
        }

        public void Unload()
        {
            this._onItemRefreshed = null;
            this._onItemBound = null;
            this._onItemUnbound = null;
        }

        public void Refresh(TEntity entity, Predicate<TEntity> predicate = null)
        {
            this.Refresh(new[] { entity }, predicate);
        }

        public void Refresh(IReadOnlyList<TEntity> entities, Predicate<TEntity> predicate = null)
        {
            int entityCount = entities?.Count ?? 0;
            
            // Unbind All items that have no data.
            int itemsCount = this._boundItems.Count;
            for (int i = itemsCount - 1; i >= 0; i--)
            {
                TUIItem item = this._boundItems[i];

                bool isStillBound = false;

                for (int j = 0; j < entityCount; j++)
                {
                    TEntity entity = entities[j];

                    isStillBound |= item.IsBoundTo(entity) && (predicate?.Invoke(entity) ?? true);
                }

                if (!isStillBound)
                {
                    this.Unbind(i, item);
                }
            }

            // Bind (if necessary) and Refresh (if possible) all items that have data.
            for (int i = 0; i < entityCount; i++)
            {
                TEntity entity = entities[i];

                if (predicate != null && !predicate.Invoke(entity))
                {
                    continue;
                }

                if (!this.TryGet(entity, out TUIItem item))
                {
                     this.Bind(entity, out item);
                }

                if (item is IRefreshable<TEntity> refreshableItem)
                {
                    refreshableItem.Refresh(entity);
                }

                this._onItemRefreshed?.Invoke(item);
            }
        }

        public void Refresh(IEnumerable<TEntity> entities, Predicate<TEntity> predicate = null)
        {
            // Unbind All items that have no data.
            int itemsCount = this._boundItems.Count;
            for (int i = itemsCount - 1; i >= 0; i--)
            {
                TUIItem item = this._boundItems[i];

                bool isStillBound = false;

                foreach (TEntity entity in entities)
                {
                    isStillBound |= item.IsBoundTo(entity) && (predicate?.Invoke(entity) ?? true);
                }

                if (!isStillBound)
                {
                    this.Unbind(i, item);
                }
            }

            // Bind (if necessary) and Refresh (if possible) all items that have data.
            foreach (TEntity entity in entities)
            {
                if (predicate != null && !predicate.Invoke(entity))
                {
                    continue;
                }

                if (!this.TryGet(entity, out TUIItem item))
                {
                    this.Bind(entity, out item);
                }

                if (item is IRefreshable<TEntity> refreshableItem)
                {
                    refreshableItem.Refresh(entity);
                }

                this._onItemRefreshed?.Invoke(item);
            }
        }

        public void Clear()
        {
            int count = this._boundItems.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                this.Unbind(i, this._boundItems[i], remove: false);
            }

            this._boundItems.Clear();
        }

        public TUIItem Get(TEntity entity)
        {
            int itemsCount = this._boundItems.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                TUIItem currentItem = this._boundItems[i];

                if (currentItem.IsBoundTo(entity))
                {
                    return currentItem;
                }
            }

            return null;
        }

        public bool TryGet(TEntity entity, out TUIItem item)
        {
            item = null;

            int itemsCount = this._boundItems.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                TUIItem currentItem = this._boundItems[i];

                if (currentItem.IsBoundTo(entity))
                {
                    item = currentItem;
                    return true;
                }
            }

            return false;
        }

        private void Bind(TEntity entity, out TUIItem item)
        {
            item = this._itemPool.Allocate(Vector2.zero, this._itemsParent);
            int itemIndex = this._boundItems.Count;
            
            item.Bind(entity);
            item.transform.SetSiblingIndex(itemIndex);
            
            this._onItemBound?.Invoke(item);

            this._boundItems.Add(item);
        }

        private void Unbind(int i, TUIItem item, bool remove = true)
        {
            if (remove)
            {
                this._boundItems.RemoveAt(i);
            }

            this._onItemUnbound?.Invoke(item);

            item.Unbind();

            this._itemPool.Release(item);
        }
    }
}
