namespace Framework.Managers
{
    public interface ISettingsValueDefinition
    {
    }

    public interface ISettingsValueDefinition<T> : ISettingsValueDefinition
    {
        T DefaultValue { get; }
    }
}
