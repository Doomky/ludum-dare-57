using Framework.Databases;
using UnityEngine;

namespace Framework.Managers
{
    public abstract class ManagerDefinition : Definition, IManagerDefinition
    {
    }

    public class ManagerDefinition<TPersistentDataDefinition> : ManagerDefinition
        where TPersistentDataDefinition : PersistentDataDefinition<TPersistentDataDefinition>, new()
    {
        [SerializeField]
        protected TPersistentDataDefinition _persitentData = new();

        public TPersistentDataDefinition PersistentData => this._persitentData;
    }
}
