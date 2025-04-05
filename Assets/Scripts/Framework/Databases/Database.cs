using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using System.Collections.Generic;
using Framework.Extensions;

namespace Framework.Databases
{
    public abstract partial class Database : SerializedScriptableObject, IDatabase
    {
    }

    public abstract partial class Database<TDatabase, TDefinition> : Database
        where TDatabase : Database<TDatabase, TDefinition>, new()
        where TDefinition : Definition
    {
        [BoxGroup("Elements", order: 0)]
        [SerializeField]
        [InlineEditor, HideReferenceObjectPicker, InlineProperty]
        [HideLabel]
        [Searchable]
#if UNITY_EDITOR
        [PropertyOrder(1)]
        [LabelText("Elements")]
        [ShowIf(nameof(_elementPreviewMode), Editor_ElementPreviewMode.Default)]
#endif
        protected TDefinition[] _definitions = null;

        public virtual IEnumerable<TDefinition> GetElements()
        {
            int elementsCount = this._definitions?.Length ?? 0;
            for (int i = 0; i < elementsCount; i++)
            {
                TDefinition element = this._definitions[i];

                yield return element;
            }
        }
    }

    public abstract partial class Database<TDatabase, TID, TDatabaseElement> : Database<TDatabase, TDatabaseElement>
        where TDatabase : Database<TDatabase, TID, TDatabaseElement>, new()
        where TID : Enum
        where TDatabaseElement : Definition<TDatabaseElement, TID>, IDefinition<TID>
    {
        public bool TryGet(TID id, out TDatabaseElement element)
        {
            int elementsCount = this._definitions?.Length ?? 0;
            for (int i = 0; i < elementsCount; i++)
            {
                if (this._definitions[i].ID.Equals(id))
                {
                    element = this._definitions[i];
                    return true;
                }
            }

            element = default;
            return false;
        }

        public TDatabaseElement GetRandom()
        {
            TDatabaseElement[] elements = this.GetElements().ToArray();
            int randomIndex = UnityEngine.Random.Range(0, elements.Length);
            
            return elements[randomIndex];
        }

        public TDatabaseElement GetRandom(Predicate<TDatabaseElement> predicate)
        {
            List<TDatabaseElement> pickableElements = new();

            TDatabaseElement[] elements = this.GetElements().ToArray();

            int count = elements.Length;
            for (int i = 0; i < count; i++)
            {
                TDatabaseElement databaseElement = elements[i];

                if (predicate.Invoke(databaseElement))
                {
                    pickableElements.Add(databaseElement);
                }
            }

            return pickableElements.GetRandom();
        }

        public TDatabaseElement Get(TID id)
        {
            if (this.TryGet(id, out TDatabaseElement element))
            {
                return element;
            }

            Debug.LogError($"Element with ID {id} not found in database {typeof(TDatabase).Name}.");
            return default;
        }
    }
}