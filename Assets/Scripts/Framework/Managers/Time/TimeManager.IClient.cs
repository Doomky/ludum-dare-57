namespace Framework.Managers
{
    public partial class TimeManager
    {
        public interface IClient
        {
            int Priority { get; }
            
            float GetTimeScale();
        }
    }
}