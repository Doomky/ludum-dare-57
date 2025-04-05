using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.Databases
{
    public partial class SuperDatabase
    {
#if UNITY_EDITOR
        private static readonly Type DatabaseType = typeof(IDatabase);

        private IEnumerable<Type> Editor_GetAllMissingDatabase()
        {
            AppDomain appDomain = AppDomain.CurrentDomain;

            Assembly[] assemblies = appDomain.GetAssemblies();

            int assembliesCount = assemblies?.Length ?? 0;
            for (int i = 0; i < assembliesCount; i++)
            {
                Assembly assembly = assemblies[i];

                Type[] types = assembly.GetTypes();

                int typesCount = types?.Length ?? 0;
                for (int j = 0; j < typesCount; j++)
                {
                    Type type = types[j];

                    if (type.IsInterface ||
                        type.IsAbstract ||
                        type.IsPrimitive ||
                        type.IsGenericType ||
                        !DatabaseType.IsAssignableFrom(type))
                    {
                        continue;
                    }

                    bool databaseIsAlreadyPresent = false;

                    int databaseCount = this._databases?.Length ?? 0;
                    for (int k = 0; k < databaseCount; k++)
                    {
                        if (type.IsAssignableFrom(this._databases[k].GetType()))
                        {
                            databaseIsAlreadyPresent = true;
                            break;
                        }
                    }

                    if (databaseIsAlreadyPresent)
                    {
                        continue;
                    }

                    yield return type;
                }
            }
        }

        private void Editor_CreateAllMissingDatabases()
        {
            IEnumerable<Type> missingDatabases = Editor_GetAllMissingDatabase();

            foreach (Type missingDatabaseType in missingDatabases)
            {
                IDatabase database = this.Editor_Create(missingDatabaseType);

                int newSize = (this._databases?.Length ?? 0) + 1;
                Array.Resize(ref this._databases, newSize);
                this._databases[newSize - 1] = database;
            }
        }

        [PropertySpace(SpaceBefore = 16, SpaceAfter = 16)]
        [Button(ButtonSizes.Large, Name = "Update", Icon = SdfIconType.ArrowRepeat)]
        private void Editor_Update()
        {
            this.Editor_CreateAllMissingDatabases();
            this.Editor_AddAllMissingDatabases();
            this.Editor_FixAllElementNames();
            this.Editor_UpdateAllElements();
        }

        private void Editor_UpdateAllElements()
        {
            int databasesCount = this._databases?.Length ?? 0;
            for (int i = 0; i < databasesCount; i++)
            {
                this._databases[i].Editor_Update();
            }
        }

        private void Editor_AddAllMissingDatabases()
        {
            foreach (Type type in this.Editor_GetAllMissingDatabase())
            {
                string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");

                if (guids.Length == 0)
                {
                    continue;
                }

                string path = AssetDatabase.GUIDToAssetPath(guids[0]);

                IDatabase database = (IDatabase)AssetDatabase.LoadAssetAtPath(path, type);

                if (database != null)
                {
                    int newSize = (this._databases?.Length ?? 0) + 1;
                    Array.Resize(ref this._databases, newSize);
                    this._databases[newSize - 1] = database;
                }
            }
        }

        private void Editor_FixAllElementNames()
        {
            int databaseCount = this._databases?.Length ?? 0;
            for (int i = 0; i < databaseCount; i++)
            {
                IDatabase database = this._databases[i];

                if (database == null)
                {
                    continue;
                }

                database.Editor_FixAllDefinitionNames();
            }
        }

        public IDatabase Editor_Create(Type type)
        {
            return SciptableObjectHelper.CreateInstance<Database>(type, $"Assets/Databases/", type.Name);
        }
#endif
    }
}