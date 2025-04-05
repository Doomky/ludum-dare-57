using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Framework.Databases
{
    public class Definition : SerializedScriptableObject, IDefinition 
    {
#if UNITY_EDITOR
        public virtual string Editor_GetName()
        {
            string definitionTypeName = this.GetType().Name;

            definitionTypeName = definitionTypeName.Replace("Definition", "");

            string elementName = $"{definitionTypeName}_{this.GetInstanceID()}";
            
            return elementName;
        }

        public virtual void Editor_Sanitize()
        {
        }
#endif
    }

    public class Definition<TDatabaseElement, TID> : Definition, IDefinition<TID>, IComparable<TDatabaseElement>
    where TID : Enum
    where TDatabaseElement : Definition<TDatabaseElement, TID>
    {
        [BoxGroup("Main", Order = 0)]
        [SerializeField]
        [PropertyOrder(0)]
        protected TID _id = default;

        public TID ID => this._id;

        public virtual int CompareTo(TDatabaseElement other)
        {
            return this._id.CompareTo(other._id);
        }

#if UNITY_EDITOR
        public override string Editor_GetName()
        {
            string definitionTypeName = this.GetType().Name;

            definitionTypeName = definitionTypeName.Replace("Definition", "");

            string elementName = $"{definitionTypeName}_{this._id}";

            return elementName;
        }
#endif
    }
}