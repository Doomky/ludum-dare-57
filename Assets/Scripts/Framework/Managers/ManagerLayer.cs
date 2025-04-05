using Framework.Databases;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Framework.Managers.ManagerLayerDefinition;

namespace Framework.Managers
{
    public partial class ManagerLayer : MonoBehaviour
    {
        [ShowInInspector]
        private ManagerLayerDefinition _definition = null;

        [ShowInInspector]
        [HideInEditorMode]
        private List<Entry> _entries = new ();

        public ManagerLayerDefinition Definition => this._definition;

        public void Load(ManagerLayerDefinition managerLayerDefinition, SuperDatabase superDatabase)
        {
            this._definition = managerLayerDefinition;

            Type[] managerTypes = this._definition.Managers;
            
            int managerTypesCount = managerTypes?.Length ??0;
            for (int i = 0; i < managerTypesCount; i++)
            {
                Type managerType = managerTypes[i];

                GameObject managerGo = new($"{managerType.Name}");

                managerGo.transform.parent = this.transform;
                managerGo.transform.localPosition = Vector3.zero;

                Manager manager = managerGo.AddComponent(managerType) as Manager;
                manager.SuperDatabase = superDatabase;

                Entry entry = new(managerType, manager);

                this._entries.Add(entry);
            }

            for (int i = 0; i < managerTypesCount; i++)
            {
                this._entries[i].Manager.Load();
            }

            for (int i = 0; i < managerTypesCount; i++)
            {
                this._entries[i].Manager.PostLayerLoad();
            }
        }

        public void Unload()
        {
            Type[] managerTypes = this._definition.Managers;

            int managerTypesCount = managerTypes?.Length ?? 0;
            for (int i = managerTypesCount - 1; i >= 0; i--)
            {
                this._entries[i].Manager.PreLayerUnload();
            }

            for (int i = managerTypesCount - 1; i >= 0; i--)
            {
                Manager manager = this._entries[i].Manager;

                manager.Unload();
            }

            for (int i = managerTypesCount - 1; i >= 0; i--)
            {
                Manager manager = this._entries[i].Manager;

                GameObject.Destroy(manager.gameObject);
            }

            this._entries.Clear();

            this._definition = null;
        }

        public bool TryGet<TManager>(out TManager manager) where TManager : Manager
        {
            manager = default(TManager);

            int count = this._entries.Count;
            for (int i = 0; i < count; i++)
            {
                if (this._entries[i].Manager is TManager tmanager)
                {
                    manager = tmanager;
                    return true;
                }
            }

            return false;
        }
    }
}
