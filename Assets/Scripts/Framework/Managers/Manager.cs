using Framework.Databases;
using Sirenix.OdinInspector;

namespace Framework.Managers
{
    public abstract class Manager : SerializedMonoBehaviour, IManager
    {
        protected SuperDatabase _superDatabase;

        public SuperDatabase SuperDatabase 
        { 
            set
            {
                this._superDatabase = value;
            }
        }
        
        public virtual void Load()
        {

        }

        public virtual void Unload()
        {

        }

        public virtual void PostLayerLoad()
        {

        }

        public virtual void PreLayerUnload()
        {

        }
    }

    public abstract class Manager<TDefinition> : Manager where TDefinition : ManagerDefinition
    {
        [ShowInInspector, HideInEditorMode]
        protected TDefinition _definition;

        public override void Load()
        {
            this._definition = this._superDatabase.Get<ManagerDefinitionDatabase>().Get<TDefinition>();

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();

            this._definition = null;
        }
    }

    public abstract class Manager<TDefinition, TPersitentData> : Manager<TDefinition>
    where TDefinition : ManagerDefinition<TPersitentData>
    where TPersitentData : PersistentDataDefinition<TPersitentData>, new()
    {
        protected PersistentDataManager _persistentDataManager = null;

        public override void Load()
        {
            base.Load();

            this._persistentDataManager = SuperManager.Get<PersistentDataManager>();
        }

        public override void Unload()
        {
            this._persistentDataManager = null;

            base.Unload();
        }

        public override void PostLayerLoad()
        {
            base.PostLayerLoad();

            string persitentDataRelativeFilePath = this.GetPeristentDataRelativeFilePath();
            this._persistentDataManager.Add(persitentDataRelativeFilePath, this._definition.PersistentData);
            this._definition.PersistentData.DataChanged += this.OnPersistentDataDataChanged;
        }

        public override void PreLayerUnload()
        {
            this._definition.PersistentData.DataChanged -= this.OnPersistentDataDataChanged;
            this._persistentDataManager.Remove(this._definition.PersistentData);

            base.PreLayerUnload();
        }

        protected virtual void OnPersistentDataDataChanged(PersistentDataDefinition obj)
        {
        }

        private string GetPeristentDataRelativeFilePath()
        {
            return $"{this.GetType().Name}.json";
        }
    }
}
