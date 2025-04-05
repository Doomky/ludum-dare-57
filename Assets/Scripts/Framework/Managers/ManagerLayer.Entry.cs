using Sirenix.OdinInspector;
using System;

namespace Framework.Managers
{
    public partial class ManagerLayer
    {
        [ShowInInspector, HideReferenceObjectPicker]
        public class Entry
        {
            [ShowInInspector]
            public Type Type { get; }

            [ShowInInspector]
            public Manager Manager { get; }

            public Entry(Type type, Manager manager)
            {
                this.Type = type;
                this.Manager = manager;
            }
        }
    }
}
