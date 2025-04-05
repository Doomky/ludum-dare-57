using Framework.UI;
using Game.Core;

namespace Game.UI
{
    public class WindowGroup_Outgame : WindowGroup
    {
        private Core.Game _game;

        public override void Load()
        {
            base.Load();

            this._game = Core.Game.Singleton;
        }

        public override void Unload()
        {
            this._game = null;

            base.Unload();
        }

        protected override bool ShouldBeVisible()
        {
            return this._game.IsInState<GameState_OutGame>();
        }
    }
}
