using Framework.Helpers;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework.Databases
{
    public abstract partial class Database
    {
#if UNITY_EDITOR
        public static TDatabase Editor_Get<TDatabase>() where TDatabase : Database
        {
            return AssetDatabase.FindAssets($"t:{typeof(TDatabase).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TDatabase>)
                .FirstOrDefault();
        }
    
        public abstract void Editor_CreateAndAddAllMissingDefinitions();
        public abstract void Editor_FixAllDefinitionNames();
        public abstract void Editor_Update();
#endif
    }   

    public abstract partial class Database<TDatabase, TDefinition>
    {
#if UNITY_EDITOR
        [BoxGroup("Elements", order: 0)]
        [EnumToggleButtons]
        [ShowInInspector]
        [HideLabel]
        [PropertyOrder(0)]
        [PropertySpace(spaceBefore: 8, spaceAfter: 8)]
        protected Editor_ElementPreviewMode _elementPreviewMode = Editor_ElementPreviewMode.Default;

        [BoxGroup("Elements", order: 0)]
        [ShowIf(nameof(_elementPreviewMode), Editor_ElementPreviewMode.Search)]
        [InlineEditor, HideReferenceObjectPicker, InlineProperty]
        [ShowInInspector]
        [PropertyOrder(1)]
        [LabelText("Elements")]
        protected TDefinition[] _filteredElements = null;

        [BoxGroup("Config", order: 1)]
        [EnumToggleButtons]
        [ShowInInspector]
        [HideLabel]
        [PropertyOrder(0)]
        [PropertySpace(spaceBefore: 8, spaceAfter: 8)]
        protected Editor_OperationMode _editor_operationMode = Editor_OperationMode.Default;

        protected virtual string Editor_GetDefinitionsFolderPath()
        {
            string definitionTypeName = typeof(TDefinition).Name;

            definitionTypeName = definitionTypeName.Replace("Definition", "");

            return $"Assets/Definitions/{definitionTypeName}s";
        }
        
        protected IEnumerable<TDefinition> Editor_GetMissingsExistingElements()
        {
            return EditorHelper
                .GetAssets<TDefinition>()
                .Where(x => this._definitions != null && Array.IndexOf(this._definitions, x) == -1);
        }

        protected void Editor_Add(TDefinition element)
        {
            if (Array.IndexOf(this._definitions, element) == -1)
            {
                int newSize = (this._definitions?.Length ?? 0) + 1;
                Array.Resize(ref this._definitions, newSize);
                this._definitions[newSize - 1] = element;
            }
        }
        

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Default)]
        [Button("Update Everything", Icon = SdfIconType.ArrowRepeat, Style = ButtonStyle.Box, ButtonAlignment = 0, ButtonHeight = 30)]
        public override void Editor_Update()
        {
            this.Editor_RemoveAllNulls();
            this.Editor_AddAllMissingExistingDefinitions();
            this.Editor_CreateAndAddAllMissingDefinitions();
            this.Editor_RemoveAllUnknownDefinitions();
            this.Editor_FixAllDefinitionNames();
            this.Editor_UpdateDefinitionLocations();
            this.Editor_SanitizeAllDefinitions();
        }

        private void Editor_RemoveAllNulls()
        {
            this._definitions = this._definitions
                .ToList()
                .Where(x => x != null)
                .ToArray();
        }

        public virtual void Editor_AddAllMissingExistingDefinitions()
        {
            IEnumerable<TDefinition> missingElements = this.Editor_GetMissingsExistingElements();
            foreach (TDefinition missingElement in missingElements)
            {
                this.Editor_Add(missingElement);
            }
        }

        public override void Editor_CreateAndAddAllMissingDefinitions()
        {
        }

        public virtual void Editor_UpdateDefinitionLocations()
        {
        }

        public virtual void Editor_RemoveAllUnknownDefinitions()
        {
        }

        public override void Editor_FixAllDefinitionNames()
        {
        }

        public virtual void Editor_SanitizeAllDefinitions()
        {
            int elementsCount = this._definitions.Length;
            for (int i = 0; i < elementsCount; i++)
            {
                this._definitions[i].Editor_Sanitize();
            }
        }

        [BoxGroup("Elements", order: 1)]
        [ShowIf(nameof(_elementPreviewMode), Editor_ElementPreviewMode.Search)]
        [PropertyOrder(101)]
        [Button("Search", ButtonSizes.Large, Icon = SdfIconType.Search)]
        protected virtual void Editor_UpdateFilteredElements()
        {
            this._filteredElements = this._definitions
                .Where(x => this.Editor_IsElementInFilter(x))
                .ToArray();
        }

        protected virtual bool Editor_IsElementInFilter(TDefinition element)
        {
            return true;
        }
#endif
    }

    public abstract partial class Database<TDatabase, TID, TDatabaseElement>
    {
#if UNITY_EDITOR
        private IEnumerable<TID> Editor_GetAllMissingIDs()
        {
            return Enum
                .GetValues(typeof(TID))
                .Cast<TID>()
                .Where(id => !typeof(TID).GetField(id.ToString()).IsDefined(typeof(ExcludeFromDatabaseGenerationAttribute), false))
                .Where(id => !Array.Exists(this._definitions, x => x.ID.Equals(id)));
        }

        private void Editor_CreateAndAdd(TID id)
        {
            string definitionsFolderPath = this.Editor_GetDefinitionsFolderPath();

            TDatabaseElement element = SciptableObjectHelper.CreateInstance<TDatabaseElement>(definitionsFolderPath, $"{id}");

            this.Editor_Add(element);
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("1. Create Missings", ButtonHeight = 24)]
        public override void Editor_CreateAndAddAllMissingDefinitions()
        {
            foreach (TID id in this.Editor_GetAllMissingIDs())
            {
                this.Editor_CreateAndAdd(id);
            }
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("2. Remove Unkowns", ButtonHeight = 24)]
        public override void Editor_RemoveAllUnknownDefinitions()
        {
            int elementsCount = this._definitions?.Length ?? 0;

            Type idType = typeof(TID);

            this._definitions = this._definitions
                .ToList()
                .Where(x => Enum.IsDefined(idType, x.ID))
                .ToArray();
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("3. Update Names", ButtonHeight = 24)]
        public override void Editor_FixAllDefinitionNames()
        {
            string newName = $"{typeof(TDatabase).Name}";

            if (this.name != newName)
            {
                string databasePath = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(databasePath, newName);
                EditorUtility.SetDirty(this);
            }

            foreach (TDatabaseElement element in this._definitions)
            {
                string elementNewName = element.Editor_GetName();
                if (element.name != elementNewName)
                {
                    string elementPath = AssetDatabase.GetAssetPath(element);
                    AssetDatabase.RenameAsset(elementPath, elementNewName);
                    EditorUtility.SetDirty(element);
                }
            }
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("4. Update Element Locations", ButtonHeight = 24)]
        public override void Editor_UpdateDefinitionLocations()
        {
            foreach (TDatabaseElement element in this._definitions)
            {
                string currentElementPath = AssetDatabase.GetAssetPath(element);

                string elementFolderPath = this.Editor_GetDefinitionsFolderPath();
                string elementName = element.Editor_GetName();

                string newElementPath = System.IO.Path.Combine(elementFolderPath, elementName);
                newElementPath = $"{newElementPath}.asset";

                if (currentElementPath != newElementPath)
                {
                    AssetDatabase.MoveAsset(currentElementPath, newElementPath);
                    EditorUtility.SetDirty(element);
                }
            }
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("5. Sanitize", ButtonHeight = 24)]
        public override void Editor_SanitizeAllDefinitions()
        {
        }

        [BoxGroup("Config", order: 1)]
        [ShowIf(nameof(_editor_operationMode), Editor_OperationMode.Detailed)]
        [Button("6. Sort", ButtonHeight = 24)]
        public virtual void Editor_Sort()
        {
            Array.Sort(this._definitions, (x, y) => x.ID.CompareTo(y.ID));
        }
#endif
    }
}