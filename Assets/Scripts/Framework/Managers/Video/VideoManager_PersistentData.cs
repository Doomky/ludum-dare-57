using UnityEngine;

namespace Framework.Managers
{
    public class VideoManager_PersistentData : PersistentDataDefinition<VideoManager_PersistentData>
    {
        [SerializeField]
        private PersistentField<int> _resolutionX = new(1920);

        [SerializeField]
        private PersistentField<int> _resolutionY = new(1080);

        [SerializeField]
        private PersistentField<FullScreenMode> _fullScreenMode = new(FullScreenMode.FullScreenWindow);

        public Vector2Int Resolution
        {
            get => new(this._resolutionX, this._resolutionY);
            set
            {
                this._resolutionX.Set(value.x);
                this._resolutionY.Set(value.y);
                this.OnDataChanged();
            }
        }

        public FullScreenMode FullScreenMode
        {
            get => this._fullScreenMode;
            set
            {
                this._fullScreenMode.Set(value);
                this.OnDataChanged();
            }
        }
    }
}
