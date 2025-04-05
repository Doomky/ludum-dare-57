using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public partial class TimeManager : Manager
    {
        private static readonly IComparer<IClient> _comparaison = Comparer<IClient>.Create((lockerA, lockerB) => lockerA.Priority.CompareTo(lockerB.Priority));

        public event Action<TimeManager, float> TimeScaleChanged;

        [TabGroup("Tabs", "Settings", Order = 1)]
        [SerializeField]
        private float _defaultTimeScale = 1;

        [HideInInspector]
        [SerializeField]
        private float _minTimeScale = 0;

        [HideInInspector]
        [SerializeField]
        private float _maxTimeScale = 5;

        [TabGroup("Tabs", "Runtime", Order = 10)]
        [ShowInInspector]
        [HideInEditorMode]
        private SortedSet<IClient> _clients = new(_comparaison);

        [TabGroup("Tabs", "Settings", Order = 1)]
        [ShowInInspector]
        [DisableIf(nameof(IsLocked))]
        public float MinTimeScale
        {
            get
            {
                return this._minTimeScale;
            }
            
            set
            {
                this._minTimeScale = Mathf.Max(0, value);
                float clampedTimescale = this.ClampTimeScale(this.TimeScale);
                if (Time.timeScale != clampedTimescale)
                {
                    Time.timeScale = clampedTimescale;
                    this.TimeScaleChanged?.Invoke(this, Time.timeScale);
                }
            }
        }

        [TabGroup("Tabs", "Settings", Order = 1)]
        [ShowInInspector]
        [DisableIf(nameof(IsLocked))]
        public float MaxTimeScale
        {
            get
            {
                return this._maxTimeScale;
            }
            
            set
            {
                this._maxTimeScale = Mathf.Min(10, value);
                float clampedTimescale = this.ClampTimeScale(this.TimeScale);
                if (Time.timeScale != clampedTimescale)
                {
                    Time.timeScale = clampedTimescale;
                    this.TimeScaleChanged?.Invoke(this, Time.timeScale);
                }
            }
        }

        [TabGroup("Tabs", "Runtime", Order = 10)]
        [ShowInInspector]
        [ReadOnly]
        public float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
        }

        public bool IsLocked => this._clients.Count > 0;

        public bool Has(IClient client)
        {
            return this._clients.Contains(client);
        }

        public void Add(IClient locker)
        {
            this._clients.Add(locker);
            float clampedTimescale = Mathf.Clamp(this._clients.Min.GetTimeScale(), this._minTimeScale, this._maxTimeScale);
            if (Time.timeScale != clampedTimescale)
            {
                Time.timeScale = clampedTimescale;
                this.TimeScaleChanged?.Invoke(this, Time.timeScale);
            }
        }

        public void Remove(IClient locker)
        {
            this._clients.Remove(locker);
            float clampedTimescale = Mathf.Clamp(this.IsLocked ? this._clients.Min.GetTimeScale() : this._defaultTimeScale, this._minTimeScale, this._maxTimeScale);
            if (Time.timeScale != clampedTimescale)
            {
                Time.timeScale = clampedTimescale;
                this.TimeScaleChanged?.Invoke(this, Time.timeScale);
            }
        }

        protected void FixedUpdate()
        {
            float clampedTimescale = Mathf.Clamp(GetTargetTimeScale(), this._minTimeScale, this._maxTimeScale);
            if (Time.timeScale != clampedTimescale)
            {
                Time.timeScale = clampedTimescale;
                this.TimeScaleChanged?.Invoke(this, Time.timeScale);
            }
        }

        private float GetTargetTimeScale()
        {
            if (this._clients == null || this._clients.Count == 0)
            {
                return this._defaultTimeScale;
            }

            return this._clients.Min.GetTimeScale();
        }

        public override void Load()
        {
            Time.timeScale = this._defaultTimeScale;
        }

        public override void Unload()
        {
        }
        
        private float ClampTimeScale(float value)
        {
            return Mathf.Clamp(value, this._minTimeScale, this._maxTimeScale);
        }
    }
}