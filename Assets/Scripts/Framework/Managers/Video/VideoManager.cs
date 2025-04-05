using UnityEngine;

namespace Framework.Managers
{
    public class VideoManager : Manager<VideoManagerDefinition, VideoManager_PersistentData>
    {
        public override void PostLayerLoad()
        {
            base.PostLayerLoad();

            this.UpdateResolution();
        }

        private void UpdateResolution()
        {
            Vector2Int resolutionVector = this._definition.PersistentData.Resolution;
            Screen.SetResolution(resolutionVector.x, resolutionVector.y, this._definition.PersistentData.FullScreenMode, Screen.currentResolution.refreshRateRatio);
        }

        protected override void OnPersistentDataDataChanged(PersistentDataDefinition obj)
        {
            this.UpdateResolution();
        }
    }
}
