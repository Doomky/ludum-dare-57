namespace Game.Managers
{
    public partial class GameManager
    {
        public enum State
        {
            None            = 0,

            LevelSelection  = 1,
            LevelSelected   = 2,
            HandSelection   = 3,
            Scoring_Before  = 4,
            Scoring         = 5,
            Scoring_After   = 6,
            Scoring_Result  = 7,
            GameOver        = 8,
            HellSelection   = 9,
            Sleeping        = 12,
            RoundResult     = 13,
            UpdateGoal      = 14,
        }
    }
}
