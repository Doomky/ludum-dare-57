namespace Framework.Databases
{
    public interface IDefinition<TID> : IDefinition
    {
        TID ID { get; }
    }

    public interface IDefinition
    {
#if UNITY_EDITOR
        void Editor_Sanitize();
#endif
    }
}