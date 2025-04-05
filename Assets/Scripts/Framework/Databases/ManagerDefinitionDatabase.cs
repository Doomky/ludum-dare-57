using Framework.Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Framework.Databases
{
    public class ManagerDefinitionDatabase : Database<ManagerDefinitionDatabase, ManagerDefinition>
    {
        public TManagerDefinition Get<TManagerDefinition>() where TManagerDefinition : ManagerDefinition
        {
            int length = this._definitions?.Length ?? 0;
            for (int i = 0; i < length; i++)
            {
                ManagerDefinition definition = this._definitions[i];

                if (definition is TManagerDefinition tdef)
                {
                    return tdef;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private IEnumerable<Type> Editor_GetAllMissingIDs()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(assembly => assembly.GetTypes())
                .Aggregate((a, b) => Enumerable.Concat(a, b).ToArray())
                .Where(type => typeof(ManagerDefinition).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract  && !type.IsPrimitive && !type.IsGenericType)
                .Where(type => !Array.Exists(this._definitions, x => x.GetType() == type));
        }

        private void Editor_CreateAndAdd(Type type)
        {
            string definitionsFolderPath = this.Editor_GetDefinitionsFolderPath();

            ManagerDefinition element = SciptableObjectHelper.CreateInstance<ManagerDefinition>(type, definitionsFolderPath, $"{type.Name}");

            this.Editor_Add(element);
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("1. Create Missings", ButtonHeight = 24)]
        public override void Editor_CreateAndAddAllMissingDefinitions()
        {
            foreach (Type type in this.Editor_GetAllMissingIDs())
            {
                this.Editor_CreateAndAdd(type);
            }
        }
#endif
    }
}