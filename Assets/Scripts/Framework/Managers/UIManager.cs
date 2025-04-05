using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Canvas = Framework.UI.Canvas;
using Framework.UI;

namespace Framework.Managers
{
    public abstract class UIManager : Manager<UIManagerDefinition>
    {
        [ShowInInspector, HideInEditorMode]
        private List<Canvas> _canvases = new();

        public IReadOnlyList<Canvas> Canvases => this._canvases;

        public override void PostLayerLoad()
        {
            List<Core.PrefabReference<Canvas>> canvasPrefabs = this._definition.CanvasPrefabs;
            
            int canvasesCount = canvasPrefabs.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                Canvas canvas = canvasPrefabs[i].InstantiateAtLocalPosition(Vector2.zero, this.transform);

                this._canvases.Add(canvas);
            }

            for (int i = 0; i < canvasesCount; i++)
            {
                Canvas canvas = this._canvases[i];

                canvas.Load();
                canvas.LoadWindowGroups();
            }
        }
        
        public override void PreLayerUnload()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].UnloadWindowGroups();
                this._canvases[i].Unload();

                GameObject.Destroy(this._canvases[i].gameObject);
            }

            this._canvases.Clear();
        }

        public T GetCanvas<T>() where T : class, ICanvas
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                if (this._canvases[i] is T canvas)
                {
                    return canvas;
                }
            }

            return null;
        }

        public T GetWindow<T>()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                T window = this._canvases[i].GetWindow<T>();
                if (window != null)
                {
                    return window;
                }
            }

            return default;
        }

        protected void Update()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].UpdateVisibility();
            }
        }
    }
}