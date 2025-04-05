namespace Framework.Databases
{
    public interface IDatabase
    {
#if UNITY_EDITOR
        void Editor_Update();

        void Editor_CreateAndAddAllMissingDefinitions();

        void Editor_FixAllDefinitionNames();
#endif
    }
}