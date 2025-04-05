using Framework.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowGroup : Entity, IWindowGroup
    {
        [BoxGroup("Components")]
        [SerializeField]
        protected CanvasGroup _canvasGroup = null;

        [BoxGroup("Data"), HideInPlayMode]
        [SerializeField]
        private List<PrefabReference<Window>> _windowPrefabs = null;

        [BoxGroup("Data"), HideInEditorMode]
        protected List<Window> _windows = new();

        public List<Window> Windows => this._windows;

        public bool IsVisible => this.transform.gameObject.activeSelf;

        public void Add(Window window)
        {
            this._windows.Add(window);
        }

        public void Remove(Window window)
        {
            this._windows.Remove(window);
        }

        public virtual void Load()
        {
            int windowsCount = this._windowPrefabs.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                Window window = this._windowPrefabs[i].InstantiateUI(this.transform);

                this._windows.Add(window);
            }

            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].Load();
            }
        }

        public virtual void Unload()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].Unload();
            }
        }

        public void UpdateVisibility()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].UpdateVisibility();
            }

            bool isVisible = this.IsVisible;
            bool newVisibility = this.ShouldBeVisible();
            if (isVisible != newVisibility)
            {
                this.transform.gameObject.SetActive(newVisibility);

                // TODO: replace with a show animation
                this._canvasGroup.alpha = newVisibility ? 1 : 0;
            }
        }

        protected abstract bool ShouldBeVisible();

        public T GetWindow<T>()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                if (this._windows[i] is T window)
                {
                    return window;
                }
            }

            return default;
        }
    }
}