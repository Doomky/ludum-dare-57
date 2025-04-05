namespace Framework.StateMachine
{
    public interface IState
    {
        void OnEnter();

        void OnExit();
    }
}
