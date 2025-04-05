using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace Framework.Databases
{
    [CreateAssetMenu(fileName = "SuperDatabase", menuName = "Framework/SuperDatabase")]
    public partial class SuperDatabase : SerializedScriptableObject
    {
        [InlineEditor, HideReferenceObjectPicker, InlineProperty]
        [HideLabel]
        [SerializeField]
        [Searchable]
        private IDatabase[] _databases = null;

        public TDatabase Get<TDatabase>() where TDatabase : IDatabase
        {
            IReadOnlyList<IDatabase> databases = this._databases;

            int databasesCount = databases.Count;
            for (int i = 0; i < databasesCount; i++)
            {
                IDatabase database = databases[i];

                if (database is TDatabase tdatabase)
                {
                    return tdatabase;
                }
            }

            return default;
        }
    }
}