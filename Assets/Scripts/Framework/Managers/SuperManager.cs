using Framework.Databases;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public partial class SuperManager : SerializedMonoBehaviour
    {
        [InlineEditor]
        private List<ManagerLayer> _layers = new();

        private static Type ManagerType = typeof(Manager);

        private static SuperManager _singleton = null;

        public event Action<ManagerLayer> LayerLoaded;
        
        public event Action<ManagerLayer> LayerUnloaded;

        public static TManager Get<TManager>() where TManager : Manager
        {
            SuperManager superManager = _singleton;
            
            if (superManager == null)
            {
                Debug.LogError("SuperManager is not initialized.");
                return default;
            }

            List<ManagerLayer> _layers = superManager._layers;
            
            for (int i = 0; i < _layers.Count; i++)
            {
                if (superManager._layers[i].TryGet(out TManager manager))
                {
                    return manager;
                }
            }

            Debug.LogError($"Manager of type {typeof(TManager).Name} not found.");
            return default;
        }

        public static bool TryGet<TManager>(out TManager manager) where TManager : Manager
        {
            manager = null;
            SuperManager superManager = _singleton;

            if (superManager == null)
            {
                Debug.LogError("SuperManager is not initialized.");
                return false;
            }

            List<ManagerLayer> layers = superManager._layers;

            for (int i = 0; i < layers.Count; i++)
            {
                if (superManager._layers[i].TryGet(out TManager foundManager))
                {
                    manager = foundManager;
                    return true;
                }
            }

            return false;
        }

        public bool HasLayer(ManagerLayerDefinition managerLayerDefinition)
        {
            for (int i = 0; i < this._layers.Count; i++)
            {
                if (this._layers[i].Definition == managerLayerDefinition)
                {
                    return true;
                }
            }

            return false;
        }

        public static void InitSingleton(SuperManager superManager)
        {
            _singleton = superManager;
        }

        public void LoadLayer(ManagerLayerDefinition managerLayerDefinition, SuperDatabase superDatabase)
        {
            DebugHelper.Assert(this, managerLayerDefinition != null, "ManagerLayerDefinition is null.");
            DebugHelper.Assert(this, !this.HasLayer(managerLayerDefinition), $"{managerLayerDefinition.name} is already loaded.");

            GameObject layerGameObject = new($"{managerLayerDefinition.name}");

            layerGameObject.transform.parent = this.transform;

            ManagerLayer managerLayer = layerGameObject.AddComponent<ManagerLayer>();

            this._layers.Add(managerLayer);

            managerLayer.Load(managerLayerDefinition, superDatabase);
            
            Debug.Log($"Loading {managerLayerDefinition.name}");
            
            this.LayerLoaded?.Invoke(managerLayer);
        }

        public void UnloadLayer(ManagerLayerDefinition managerLayerDefinition)
        {
            bool layerWasfound = this.TryGet(managerLayerDefinition, out ManagerLayer managerLayer);

            DebugHelper.Assert(this, layerWasfound, $"{managerLayerDefinition.name} was not loaded.");

            managerLayer.Unload();
            
            this._layers.Remove(managerLayer);

            GameObject.Destroy(managerLayer.gameObject);

            this.LayerUnloaded?.Invoke(managerLayer);
        }

        private bool TryGet(ManagerLayerDefinition managerLayerDefinition, out ManagerLayer managerLayer)
        {
            for (int i = 0; i < this._layers.Count; i++)
            {
                if (this._layers[i].Definition == managerLayerDefinition)
                {
                    managerLayer = this._layers[i];
                    return true;
                }
            }

            managerLayer = null;
            return false;
        }
    }
}
